using AutoMapper;
using SecretariaConcafras.Application.DTOs.Pagamentos;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Application.Mappings
{
    public class PagamentoMappingProfile : Profile
    {
        public PagamentoMappingProfile()
        {
            // Mapeia Pagamento -> PagamentoResponseDto sem acessar propriedades inexistentes
            CreateMap<Pagamento, PagamentoResponseDto>()
                // Se DataPagamento existir no DTO, por ora deixa nulo (ou troque para um campo válido do seu Pagamento, ex.: CriadoEm)
                .ForMember(dest => dest.DataPagamento,
                    opt => opt.MapFrom(_ => (DateTime?)null))
                // Se o DTO espera string: usa o Id como "código" por enquanto (ajuste quando tiver CodigoTransacao na entidade)
                .ForMember(dest => dest.CodigoTransacao,
                    opt => opt.MapFrom(src => src.Id.ToString()));

            // Atualização
            CreateMap<PagamentoUpdateDto, Pagamento>();
        }
    }
}
