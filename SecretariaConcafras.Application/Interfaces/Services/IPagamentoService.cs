using SecretariaConcafras.Application.DTOs.Pagamentos;
using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.Interfaces
{
    public interface IPagamentoService
    {
        /// <summary>
        /// Cria uma cobrança (Checkout Pro do Mercado Pago) para todas as inscrições
        /// do responsável financeiro no evento informado, retornando a URL de checkout.
        /// </summary>
        Task<PagamentoCreateResultDto> CriarParaGrupoCheckoutAsync(Guid eventoId, Guid responsavelId, List<Guid>? excluirInscricaoIds = null);

        /// <summary>
        /// Obtém o status atual do pagamento (Pendente/Aguardando/Pago/...).
        /// </summary>
        Task<PagamentoStatus> ObterStatusAsync(Guid pagamentoId);

        /// <summary>
        /// Marca o pagamento como pago e confirma as inscrições vinculadas (usado pelo webhook).
        /// </summary>
        Task ConfirmarPagoAsync(Guid pagamentoId);
    }
}
