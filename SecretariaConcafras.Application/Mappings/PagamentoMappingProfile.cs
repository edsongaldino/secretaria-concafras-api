using AutoMapper;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Application.DTOs.Pagamentos;

namespace SecretariaConcafras.Application.Mappings
{
    public class PagamentoMappingProfile : Profile
    {
        public PagamentoMappingProfile()
        {
            CreateMap<Pagamento, PagamentoResponseDto>();

            CreateMap<PagamentoCreateDto, Pagamento>()
                .ForMember(dest => dest.DataPagamento, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Pendente"));

            CreateMap<PagamentoUpdateDto, Pagamento>();
        }
    }
}
