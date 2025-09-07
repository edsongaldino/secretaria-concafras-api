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
            var inscs = await _db.Inscricoes
                .Include(i => i.Cursos).ThenInclude(ic => ic.Curso)
                .Include(i => i.PagamentoItem)
                .Where(i => i.EventoId == eventoId && i.ResponsavelFinanceiroId == responsavelId)
                .ToListAsync();

            var elegiveis = inscs.Where(i =>
                i.Cursos.Any(c => c.Curso.Bloco == BlocoCurso.TemaAtual) &&
                i.Cursos.Any(c => c.Curso.Bloco == BlocoCurso.TemaEspecifico) &&
                (i.PagamentoItem == null ||
                 i.PagamentoItem.Pagamento == null ||
                 i.PagamentoItem.Pagamento.Status != PagamentoStatus.Pago)
            ).ToList();

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
