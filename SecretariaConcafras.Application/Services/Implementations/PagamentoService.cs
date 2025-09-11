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

        public async Task<PagamentoCreateResultDto> CriarParaGrupoCheckoutAsync(Guid eventoId, Guid responsavelId)
        {
            // 0) Reaproveitar pagamento em aberto/pendente do mesmo evento+responsável
            var pag = await _db.Pagamentos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p =>
                    p.EventoId == eventoId &&
                    p.ResponsavelFinanceiroId == responsavelId &&
                    (p.Status == PagamentoStatus.Aguardando || p.Status == PagamentoStatus.Pendente));

            if (pag is null)
            {
                pag = new Pagamento
                {
                    Id = Guid.NewGuid(),
                    EventoId = eventoId,
                    ResponsavelFinanceiroId = responsavelId,
                    ValorTotal = 0m,
                    Metodo = MetodoPagamento.Checkout,
                    Status = PagamentoStatus.Pendente, // ficará pendente após criar a preferência
                    DataCriacao = DateTime.UtcNow
                };
                _db.Pagamentos.Add(pag);
                await _db.SaveChangesAsync();
            }

            // 1) Buscar inscrições do responsável no evento
            var inscs = await _db.Inscricoes
                .Where(i => i.EventoId == eventoId && i.ResponsavelFinanceiroId == responsavelId)
                .Include(i => i.Cursos).ThenInclude(ic => ic.Curso)
                .Include(i => i.PagamentoItem).ThenInclude(pi => pi.Pagamento)
                .Include(i => i.InscricaoTrabalhador).ThenInclude(it => it.ComissaoEvento)
                .AsNoTracking()
                .ToListAsync();

            // Helpers de negócio
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
                if (EhCrianca(i)) return TemPeloMenosUmCurso(i);         // criança: 1 curso basta
                return TemTemaAtual(i) && TemTemaEspecifico(i);          // jovem/adulto: precisa dos 2 blocos
            }

            // 2) Elegíveis
            var candidatos = inscs.Where(i =>
                NaoPago(i) &&
                (TrabalhadorComComissao(i) || (!EhTrabalhador(i) && ElegivelParticipante(i)))
            );

            // 3) Evitar duplicar itens em outros pagamentos em aberto
            var jaExistentes = new HashSet<Guid>(pag.Itens.Select(x => x.InscricaoId));
            var elegiveis = new List<Inscricao>();

            foreach (var i in candidatos)
            {
                var temItemEmOutroPagamento = i.PagamentoItem != null
                    && i.PagamentoItem.Pagamento != null
                    && i.PagamentoItem.PagamentoId != pag.Id
                    && (i.PagamentoItem.Pagamento.Status == PagamentoStatus.Aguardando
                        || i.PagamentoItem.Pagamento.Status == PagamentoStatus.Pendente);

                if (temItemEmOutroPagamento) continue;

                if (!jaExistentes.Contains(i.Id))
                {
                    elegiveis.Add(i);
                    jaExistentes.Add(i.Id);
                }
            }

            if (!elegiveis.Any())
            {
                return new PagamentoCreateResultDto
                {
                    PagamentoId = pag.Id,
                    EventoId = eventoId,
                    ResponsavelFinanceiroId = responsavelId,
                    Valor = pag.Itens.Sum(x => x.Valor),
                    Status = pag.Status.ToString(),
                    CheckoutUrl = null,
                    Mensagem = "Nenhuma inscrição elegível (ou já associada a outro pagamento em aberto)."
                };
            }

            // 4) Calcular valores (TODO: substitua pelo cálculo real)
            foreach (var i in elegiveis)
            {
                var valorItem = 10m; // TODO: calcule de verdade
                _db.Set<PagamentoItem>().Add(new PagamentoItem
                {
                    Id = Guid.NewGuid(),
                    PagamentoId = pag.Id,
                    InscricaoId = i.Id,
                    Valor = valorItem
                });
            }

            await _db.SaveChangesAsync();

            // 5) Recalcular total
            pag.ValorTotal = await _db.Pagamentos
                .Where(p => p.Id == pag.Id)
                .Select(p => p.Itens.Sum(it => it.Valor))
                .FirstAsync();

            if (pag.ValorTotal <= 0)
                throw new InvalidOperationException("Valor total do pagamento deve ser maior que zero.");

            await _db.SaveChangesAsync();

            // 6) Criar preferência no Mercado Pago (direto no SDK)
            //    AccessToken
            MercadoPagoConfig.AccessToken = _mp.AccessToken;

            // URLs
            var notify = _mp.WebhookBaseUrl; // ex.: https://api.inscribo.com.br/api/pagamentos/mp/webhook
            var success = $"{_mp.AppBaseUrl}/pagamento/sucesso?pid={pag.Id}";
            var failure = $"{_mp.AppBaseUrl}/pagamento/erro?pid={pag.Id}";
            var pending = $"{_mp.AppBaseUrl}/pagamento/pendente?pid={pag.Id}";
            var desc = $"Evento {eventoId} - {elegiveis.Count} inscrição(ões)";

            var prefRequest = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new()
                    {
                        Title      = desc,
                        Quantity   = 1,
                        UnitPrice  = pag.ValorTotal,
                        CurrencyId = "BRL"
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = success,
                    Pending = pending,
                    Failure = failure
                },
                AutoReturn = "all", // "approved" também funciona
                NotificationUrl = notify,
                ExternalReference = pag.Id.ToString(),
                StatementDescriptor = "INSCRIBO" // opcional
            };

            // Idempotência opcional (evita prefs duplicadas em replays)
            // var reqOpts = new MercadoPago.Client.RequestOptions
            // {
            //     CustomHeaders = new Dictionary<string, string>
            //     {
            //         ["X-Idempotency-Key"] = pag.Id.ToString()
            //     }
            // };

            var prefClient = new PreferenceClient();
            var pref = await prefClient.CreateAsync(prefRequest /*, reqOpts*/);

            pag.ProviderReference = pref.Id; // preference_id
            pag.Status = PagamentoStatus.Pendente;
            await _db.SaveChangesAsync();

            return new PagamentoCreateResultDto
            {
                PagamentoId = pag.Id,
                EventoId = eventoId,
                ResponsavelFinanceiroId = responsavelId,
                Valor = pag.ValorTotal,
                Status = pag.Status.ToString(),
                CheckoutUrl = pref.InitPoint // produção usa InitPoint
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
