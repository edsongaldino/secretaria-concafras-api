using AutoMapper;
using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Application.Mappings
{
    public class ComissaoMappingProfile : Profile
    {
        public ComissaoMappingProfile()
        {
            // Catálogo (Comissao)
            CreateMap<Comissao, ComissaoDto>();
            CreateMap<ComissaoCreateDto, Comissao>();
            CreateMap<ComissaoUpdateDto, Comissao>();
        }
    }
}
