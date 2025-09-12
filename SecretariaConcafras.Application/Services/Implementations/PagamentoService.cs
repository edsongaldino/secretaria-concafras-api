using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using SecretariaConcafras.Application.DTOs.Pagamentos;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Application.Options;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Enums;
using SecretariaConcafras.Infrastructure;

namespace SecretariaConcafras.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly ApplicationDbContext _db;
        private readonly MpOptions _mp;

        public PagamentoService(ApplicationDbContext db, IOptions<MpOptions> mp)
        {
            _db = db;
            _mp = mp.Value;
        }

        public async Task<PagamentoCreateResultDto> CriarParaGrupoCheckoutAsync(
    Guid eventoId,
    Guid responsavelId,                       // <- PARTICIPANTE (FK em pagamentos)
    List<Guid>? excluirInscricaoIds = null)
        {
            excluirInscricaoIds ??= new();

            // ===== 1) Coletar inscrições já vinculadas a pagamentos em aberto (para não duplicar) =====
            var inscricoesEmPagamentoAberto = await _db.Pagamentos
                .Where(p => p.EventoId == eventoId
                         && (p.Status == PagamentoStatus.Aguardando || p.Status == PagamentoStatus.Pendente))
                .SelectMany(p => p.Itens.Select(it => it.InscricaoId))
                .ToListAsync();

            var emAbertoSet = inscricoesEmPagamentoAberto.ToHashSet();
            var excluirSet = excluirInscricaoIds.ToHashSet();

            // ===== 2) Trazer TODAS as inscrições do evento (sem filtrar por responsável ainda) =====
            var todasInscricoesEvento = await _db.Inscricoes
                .Where(i => i.EventoId == eventoId)
                .Include(i => i.Cursos).ThenInclude(ic => ic.Curso)
                .Include(i => i.PagamentoItem).ThenInclude(pi => pi.Pagamento)
                .Include(i => i.InscricaoTrabalhador).ThenInclude(it => it.ComissaoEvento)
                .AsNoTracking()
                .ToListAsync();

            // ===== Helpers de negócio (iguais aos seus) =====
            bool NaoPago(Inscricao i) =>
                i.PagamentoItem?.Pagamento == null ||
                i.PagamentoItem.Pagamento.Status != PagamentoStatus.Pago;

            bool TemTemaAtual(Inscricao i) =>
                i.Cursos?.Any(c => c.Curso != null && c.Curso.Bloco == BlocoCurso.TemaAtual) == true;

            bool TemTemaEspecifico(Inscricao i) =>
                i.Cursos?.Any(c => c.Curso != null && c.Curso.Bloco == BlocoCurso.TemaEspecifico) == true;

            bool TemPeloMenosUmCurso(Inscricao i) =>
                i.Cursos?.Any(c => c.Curso != null) == true;

            bool EhTrabalhador(Inscricao i) =>
                i.InscricaoTrabalhador != null;

            bool TrabalhadorComComissao(Inscricao i) =>
                EhTrabalhador(i) && (i.InscricaoTrabalhador.ComissaoEvento != null || i.InscricaoTrabalhador.ComissaoEventoId != null);

            bool EhCrianca(Inscricao i) =>
                i.Cursos?.Any(c => c.Curso != null && c.Curso.Publico == PublicoCurso.Crianca) == true;

            bool ElegivelParticipante(Inscricao i)
            {
                if (EhCrianca(i)) return TemPeloMenosUmCurso(i); // criança: 1 curso basta
                return TemTemaAtual(i) && TemTemaEspecifico(i);  // jovem/adulto: precisa dos 2 blocos
            }

            // ===== 3) Candidatos elegíveis do evento (sem responsável), respeitando exclusões e "em aberto" =====
            var candidatos = todasInscricoesEvento.Where(i =>
                !excluirSet.Contains(i.Id) &&
                !emAbertoSet.Contains(i.Id) &&
                NaoPago(i) &&
                (TrabalhadorComComissao(i) || (!EhTrabalhador(i) && ElegivelParticipante(i)))
            ).ToList();

            if (!candidatos.Any())
            {
                // nada para cobrar de ninguém neste evento
                return new PagamentoCreateResultDto
                {
                    PagamentoId = Guid.Empty,
                    EventoId = eventoId,
                    ResponsavelFinanceiroId = responsavelId,
                    Valor = 0m,
                    Status = "SemItens",
                    CheckoutUrl = null,
                    Mensagem = "Nenhuma inscrição elegível (todas excluídas, pagas, ou em outro pagamento em aberto)."
                };
            }

            // ===== 4) Resolver o responsável final =====
            //   - Se houver ao menos 1 candidato do responsável atual, mantém;
            //   - Caso contrário, reatribui para o participante do primeiro candidato.
            var existemDoResponsavelAtual = candidatos.Any(i => i.ResponsavelFinanceiroId == responsavelId);
            var responsavelFinal = existemDoResponsavelAtual
                ? responsavelId
                : candidatos.First().ResponsavelFinanceiroId;

            // Subconjunto elegível do responsável final
            var elegiveis = candidatos
                .Where(i => i.ResponsavelFinanceiroId == responsavelFinal)
                .ToList();

            // Sanidade extra (não deveria acontecer após a lógica acima)
            if (!elegiveis.Any())
            {
                return new PagamentoCreateResultDto
                {
                    PagamentoId = Guid.Empty,
                    EventoId = eventoId,
                    ResponsavelFinanceiroId = responsavelFinal,
                    Valor = 0m,
                    Status = "SemItens",
                    CheckoutUrl = null,
                    Mensagem = "Nenhuma inscrição elegível para o responsável selecionado."
                };
            }

            // ===== 5) Reaproveitar ou criar pagamento do (evento + responsável FINAL) =====
            var pag = await _db.Pagamentos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p =>
                    p.EventoId == eventoId &&
                    p.ResponsavelFinanceiroId == responsavelFinal &&
                    (p.Status == PagamentoStatus.Aguardando || p.Status == PagamentoStatus.Pendente));

            if (pag is null)
            {
                pag = new Pagamento
                {
                    Id = Guid.NewGuid(),
                    EventoId = eventoId,
                    ResponsavelFinanceiroId = responsavelFinal,
                    ValorTotal = 0m,
                    Metodo = MetodoPagamento.Checkout,
                    Status = PagamentoStatus.Pendente,
                    DataCriacao = DateTime.UtcNow
                };
                _db.Pagamentos.Add(pag);
                await _db.SaveChangesAsync();
            }

            // ===== 6) Adicionar itens evitando duplicação =====
            var jaNoPagamento = pag.Itens.Select(x => x.InscricaoId).ToHashSet();
            foreach (var i in elegiveis)
            {
                var valorItem = i.ValorInscricao;
                if (valorItem <= 0) continue;

                if (jaNoPagamento.Contains(i.Id)) continue;

                _db.Set<PagamentoItem>().Add(new PagamentoItem
                {
                    Id = Guid.NewGuid(),
                    PagamentoId = pag.Id,
                    InscricaoId = i.Id,
                    Valor = valorItem
                });
            }

            await _db.SaveChangesAsync();

            // ===== 7) Recalcular total =====
            pag.ValorTotal = await _db.PagamentoItens
                .Where(pi => pi.PagamentoId == pag.Id)
                .Select(pi => (decimal?)pi.Valor)
                .SumAsync() ?? 0m;

            if (pag.ValorTotal <= 0)
            {
                return new PagamentoCreateResultDto
                {
                    PagamentoId = pag.Id,
                    EventoId = eventoId,
                    ResponsavelFinanceiroId = responsavelFinal,
                    Valor = 0m,
                    Status = pag.Status.ToString(),
                    CheckoutUrl = null,
                    Mensagem = "Nenhuma inscrição elegível após aplicar regras de valor."
                };
            }

            await _db.SaveChangesAsync();

            // ===== 8) Criar preferência MP (idempotência opcional) =====
            MercadoPagoConfig.AccessToken = _mp.AccessToken;

            var notify = _mp.WebhookBaseUrl;
            var success = $"{_mp.AppBaseUrl}/pagamento/sucesso?pid={pag.Id}";
            var failure = $"{_mp.AppBaseUrl}/pagamento/erro?pid={pag.Id}";
            var pending = $"{_mp.AppBaseUrl}/pagamento/pendente?pid={pag.Id}";
            var desc = $"Evento {eventoId} - {elegiveis.Count} inscrição(ões)";

            var prefRequest = new MercadoPago.Client.Preference.PreferenceRequest
            {
                Items = new List<MercadoPago.Client.Preference.PreferenceItemRequest>
        {
            new()
            {
                Title      = desc,
                Quantity   = 1,
                UnitPrice  = pag.ValorTotal,
                CurrencyId = "BRL"
            }
        },
                BackUrls = new MercadoPago.Client.Preference.PreferenceBackUrlsRequest
                {
                    Success = success,
                    Pending = pending,
                    Failure = failure
                },
                AutoReturn = "all",
                NotificationUrl = notify,
                ExternalReference = pag.Id.ToString(),
                StatementDescriptor = "INSCRIBO"
            };

            var prefClient = new MercadoPago.Client.Preference.PreferenceClient();
            var pref = await prefClient.CreateAsync(prefRequest);

            pag.ProviderReference = pref.Id;
            pag.Status = PagamentoStatus.Pendente;
            await _db.SaveChangesAsync();

            return new PagamentoCreateResultDto
            {
                PagamentoId = pag.Id,
                EventoId = eventoId,
                ResponsavelFinanceiroId = responsavelFinal,   // <- pode ter sido reatribuído
                Valor = pag.ValorTotal,
                Status = pag.Status.ToString(),
                CheckoutUrl = pref.InitPoint
            };
        }


        public async Task<PagamentoStatus> ObterStatusAsync(Guid pagamentoId)
        {
            var p = await _db.Pagamentos.FindAsync(pagamentoId) ?? throw new KeyNotFoundException();
            return p.Status;
        }

        public async Task ConfirmarPagoAsync(Guid pagamentoId)
        {
            var p = await _db.Pagamentos
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(x => x.Id == pagamentoId) ?? throw new KeyNotFoundException();

            if (p.Status == PagamentoStatus.Pago) return;

            p.Status = PagamentoStatus.Pago;

            var insIds = p.Itens.Select(i => i.InscricaoId).ToList();
            var inscricoes = await _db.Inscricoes.Where(i => insIds.Contains(i.Id)).ToListAsync();
            foreach (var i in inscricoes) i.PagamentoConfirmado = true;

            await _db.SaveChangesAsync();
        }
    }
}
