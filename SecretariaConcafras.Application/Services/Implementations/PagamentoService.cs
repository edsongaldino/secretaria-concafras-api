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
            // 1) buscar inscrições elegíveis
            // Carregamento
            var inscs = await _db.Inscricoes
                .Where(i => i.EventoId == eventoId && i.ResponsavelFinanceiroId == responsavelId)
                .Include(i => i.Cursos).ThenInclude(ic => ic.Curso)
                .Include(i => i.PagamentoItem).ThenInclude(pi => pi.Pagamento)
                .Include(i => i.InscricaoTrabalhador).ThenInclude(it => it.ComissaoEvento)
                .AsNoTracking()
                .ToListAsync();

            // Helpers
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
                    EhTrabalhador(i) && (i.InscricaoTrabalhador.ComissaoEvento != null
                         || i.InscricaoTrabalhador.ComissaoEventoId != null);

            bool EhCrianca(Inscricao i) =>
                // se o "público" vem do(s) curso(s):
                i.Cursos?.Any(c => c.Curso != null && c.Curso.Publico == PublicoCurso.Crianca) == true;
            // ou, se o público vem do próprio participante, troque por:
            //    i.Participante?.Publico == PublicoCurso.Crianca;

            // Regra por tipo
            bool ElegivelParticipante(Inscricao i)
            {
                // Criança: 1 curso basta
                if (EhCrianca(i)) return TemPeloMenosUmCurso(i);
                // Jovem/Adulto: precisa dos dois blocos
                return TemTemaAtual(i) && TemTemaEspecifico(i);
            }

            // FINAL: não pago E (trabalhador válido OU participante válido)
            var elegiveis = inscs
                .Where(i => NaoPago(i) &&
                            (TrabalhadorComComissao(i) ||
                             (!EhTrabalhador(i) && ElegivelParticipante(i))))
                .ToList();

            // Evite 500 para caso esperado:
            if (!elegiveis.Any())
                throw new InvalidOperationException("Não há inscrições elegíveis para pagamento.");


            // 2) calcular valor (ajuste sua regra)
            decimal total = elegiveis.Sum(_ => 0m); // TODO: coloque o valor real

            // 3) criar registro local
            var pag = new Pagamento
            {
                Id = Guid.NewGuid(),
                EventoId = eventoId,
                ResponsavelFinanceiroId = responsavelId,
                ValorTotal = total,
                Metodo = MetodoPagamento.Checkout,
                Status = PagamentoStatus.Pendente,
                DataCriacao = DateTime.UtcNow
            };
            _db.Pagamentos.Add(pag);
            foreach (var i in elegiveis)
                _db.Set<PagamentoItem>().Add(new PagamentoItem { Id = Guid.NewGuid(), PagamentoId = pag.Id, InscricaoId = i.Id, Valor = 0m });
            await _db.SaveChangesAsync();

            // 4) criar preferência no Mercado Pago
            var notify = $"{_mp.WebhookBaseUrl}/api/pagamentos/mp/webhook";
            var success = $"{_mp.AppBaseUrl}/pagamento/sucesso?pid={pag.Id}";
            var failure = $"{_mp.AppBaseUrl}/pagamento/erro?pid={pag.Id}";
            var pending = $"{_mp.AppBaseUrl}/pagamento/pendente?pid={pag.Id}";
            var desc = $"Evento {eventoId} - {elegiveis.Count} inscrição(ões)";

            var pref = await _gateway.CriarCheckoutGrupoAsync(pag.Id, total, desc, notify, success, failure, pending);

            pag.ProviderReference = pref.ProviderRef; // preference_id
            await _db.SaveChangesAsync();

            return new PagamentoCreateResultDto
            {
                PagamentoId = pag.Id,
                EventoId = eventoId,
                ResponsavelFinanceiroId = responsavelId,
                Valor = total,
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
