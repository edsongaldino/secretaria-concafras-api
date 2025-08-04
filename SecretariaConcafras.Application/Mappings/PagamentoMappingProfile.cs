using AutoMapper;
using SecretariaConcafras.Application.DTOs.Pagamentos;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Enums;

namespace SecretariaConcafras.Application.Mappings
{
    public class PagamentoMappingProfile : Profile
    {
        public PagamentoMappingProfile()
        {
            CreateMap<Pagamento, PagamentoResponseDto>();

            CreateMap<Pagamento, PagamentoResponseDto>()
            .ForMember(dest => dest.DataPagamento, opt =>
                opt.MapFrom(src => src.Historicos
                    .Where(h => h.Status == StatusPagamento.Aprovado)
                    .OrderBy(h => h.Data)
                    .FirstOrDefault().Data))
            .ForMember(dest => dest.CodigoTransacao, opt => opt.MapFrom(src => src.CodigoTransacao));


            CreateMap<PagamentoUpdateDto, Pagamento>();
        }
    }
}
