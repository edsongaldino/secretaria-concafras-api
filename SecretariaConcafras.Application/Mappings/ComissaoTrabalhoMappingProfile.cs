using AutoMapper;
using SecretariaConcafras.Application.DTOs.Comissoes;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Application.Mappings
{
    public class ComissaoTrabalhoMappingProfile : Profile
    {
        public ComissaoTrabalhoMappingProfile()
        {
            CreateMap<ComissaoTrabalho, ComissaoTrabalhoResponseDto>()
                .ForMember(dest => dest.EventoTitulo, opt => opt.MapFrom(src => src.Evento.Titulo))
                .ForMember(dest => dest.Usuarios, opt => opt.MapFrom(src => src.Trabalhadores));

            CreateMap<UsuarioComissao, UsuarioComissaoResponseDto>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Usuario.Nome))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.Perfil, opt => opt.MapFrom(src => src.Perfil.ToString()));

            CreateMap<ComissaoTrabalhoCreateDto, ComissaoTrabalho>();
            CreateMap<ComissaoTrabalhoUpdateDto, ComissaoTrabalho>();
        }
    }
}
