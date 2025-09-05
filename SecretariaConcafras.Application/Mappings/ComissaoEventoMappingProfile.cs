using AutoMapper;
using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Application.Mappings
{
    public class ComissaoEventoMappingProfile : Profile
    {
        public ComissaoEventoMappingProfile()
        {
            CreateMap<ComissaoEvento, ComissaoTrabalhoResponseDto>()
                .ForMember(dest => dest.EventoTitulo, opt => opt.MapFrom(src => src.Evento.Titulo))
                .ForMember(dest => dest.Usuarios, opt => opt.MapFrom(src => src.InscricoesTrabalhadores));

            CreateMap<UsuarioComissao, UsuarioComissaoResponseDto>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Usuario.Nome))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.Perfil, opt => opt.MapFrom(src => src.Perfil.ToString()));

            CreateMap<ComissaoEventoDto, ComissaoEvento>();
            CreateMap<ComissaoEventoUpdateDto, ComissaoEvento>();
        }
    }
}
