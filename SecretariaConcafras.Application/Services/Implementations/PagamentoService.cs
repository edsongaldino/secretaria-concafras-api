using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SecretariaConcafras.Application.DTOs.Pagamentos;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Application.Options;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Enums;
using SecretariaConcafras.Domain.Interfaces;
using SecretariaConcafras.Infrastructure;

namespace SecretariaConcafras.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly ApplicationDbContext _db;
        private readonly IGatewayPagamento _gateway;
        private readonly MpOptions _mp;

        public PagamentoService(ApplicationDbContext db, IGatewayPagamento gateway, IOptions<MpOptions> mp)
        { _db = db; _gateway = gateway; _mp = mp.Value; }

        public async Task<PagamentoCreateResultDto> CriarParaGrupoCheckoutAsync(Guid eventoId, Guid responsavelId)
        {
            // 0) Reaproveitar pagamento em aberto/pendente do mesmo evento+responsável
            //    (evita criar um novo e bater no UNIQUE(inscricao_id))
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
                    Status = PagamentoStatus.Pendente, // só muda para Pendente depois de criar a preferência
                    DataCriacao = DateTime.UtcNow
                };
                _db.Pagamentos.Add(pag);
                await _db.SaveChangesAsync();
            }

            // 1) Buscar inscrições do responsável no evento (com navegações necessárias)
            var inscs = await _db.Inscricoes
                .Where(i => i.EventoId == eventoId && i.ResponsavelFinanceiroId == responsavelId)
                .Include(i => i.Cursos).ThenInclude(ic => ic.Curso)
                .Include(i => i.PagamentoItem).ThenInclude(pi => pi.Pagamento)
                .Include(i => i.InscricaoTrabalhador).ThenInclude(it => it.ComissaoEvento)
                .AsNoTracking()
                .ToListAsync();

            // Helpers (iguais aos seus)
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
                if (EhCrianca(i)) return TemPeloMenosUmCurso(i);          // criança: 1 curso basta
                return TemTemaAtual(i) && TemTemaEspecifico(i);           // jovem/adulto: precisa dos 2 blocos
            }

            // 2) Elegíveis pelo seu critério de negócio (não pago + regras)
            var candidatos = inscs.Where(i =>
                NaoPago(i) &&
                (TrabalhadorComComissao(i) || (!EhTrabalhador(i) && ElegivelParticipante(i)))
            );

            // 3) Dos candidatos, pegue apenas os que ainda NÃO têm item em outro pagamento
            //    (se já tiver PagamentoItem em pagamento aberto/pendente diferente, NÃO adiciona para não violar UNIQUE)
            var jaExistentes = new HashSet<Guid>(pag.Itens.Select(x => x.InscricaoId)); // itens deste pagamento
            var elegiveis = new List<Inscricao>();

            foreach (var i in candidatos)
            {
                var temItemEmOutroPagamento = i.PagamentoItem != null
                                              && i.PagamentoItem.Pagamento != null
                                              && i.PagamentoItem.PagamentoId != pag.Id
                                              && (i.PagamentoItem.Pagamento.Status == PagamentoStatus.Aguardando
                                                  || i.PagamentoItem.Pagamento.Status == PagamentoStatus.Pendente);

                if (temItemEmOutroPagamento)
                {
                    // pula: essa inscrição já está “presa” em outro pagamento em aberto
                    continue;
                }

                if (!jaExistentes.Contains(i.Id))
                {
                    elegiveis.Add(i);
                    jaExistentes.Add(i.Id);
                }
            }

            if (!elegiveis.Any())
                return new PagamentoCreateResultDto
                {
                    PagamentoId = pag.Id,
                    EventoId = eventoId,
                    ResponsavelFinanceiroId = responsavelId,
                    Valor = pag.Itens.Sum(x => x.Valor),
                    Status = pag.Status.ToString(),
                    CheckoutUrl = null, // nada novo a adicionar
                    Mensagem = "Nenhuma inscrição elegível (ou já associada a outro pagamento em aberto)."
                };

            // 4) Calcular valores (substitua pelo seu cálculo real)
            foreach (var i in elegiveis)
            {
                var valorItem = 0m; // TODO: calcule de verdade
                _db.Set<PagamentoItem>().Add(new PagamentoItem
                {
                    Id = Guid.NewGuid(),
                    PagamentoId = pag.Id,
                    InscricaoId = i.Id,
                    Valor = 10
                });
            }

            await _db.SaveChangesAsync(); // <- agora não deve mais disparar 23505

            // 5) Recalcular total do pagamento
            pag.ValorTotal = await _db.Pagamentos
                .Where(p => p.Id == pag.Id)
                .Select(p => p.Itens.Sum(it => it.Valor))
                .FirstAsync();

            if (pag.ValorTotal <= 0)
                throw new InvalidOperationException("Valor total do pagamento deve ser maior que zero.");

            await _db.SaveChangesAsync();

            // 6) Criar preferência no Mercado Pago
            //    ⚠ NÃO DUPLIQUE a URL do webhook: _mp.WebhookBaseUrl já deve estar completo!
            var notify = _mp.WebhookBaseUrl; // ex.: https://api.inscribo.com.br/api/pagamentos/mp/webhook
            var success = $"{_mp.AppBaseUrl}/pagamento/sucesso?pid={pag.Id}";
            var failure = $"{_mp.AppBaseUrl}/pagamento/erro?pid={pag.Id}";
            var pending = $"{_mp.AppBaseUrl}/pagamento/pendente?pid={pag.Id}";
            var desc = $"Evento {eventoId} - {elegiveis.Count} inscrição(ões)";

            var pref = await _gateway.CriarCheckoutGrupoAsync(pag.Id, pag.ValorTotal, desc, notify, success, failure, pending);

            pag.ProviderReference = pref.ProviderRef; // preference_id
            pag.Status = PagamentoStatus.Pendente;    // agora sim, já tem preferência criada
            await _db.SaveChangesAsync();

            return new PagamentoCreateResultDto
            {
                PagamentoId = pag.Id,
                EventoId = eventoId,
                ResponsavelFinanceiroId = responsavelId,
                Valor = pag.ValorTotal,
                Status = pag.Status.ToString(),
                CheckoutUrl = pref.CheckoutUrl
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
