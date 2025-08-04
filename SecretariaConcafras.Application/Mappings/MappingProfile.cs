using AutoMapper;
using SecretariaConcafras.Application.DTOs.Usuarios;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Usuario
            CreateMap<Usuario, UsuarioResponseDto>();
            CreateMap<UsuarioCreateDto, Usuario>();
            CreateMap<UsuarioUpdateDto, Usuario>();

            // Aqui você adiciona os outros mapeamentos conforme for criando DTOs
        }
    }
}
