using AutoMapper;
using SecretariaConcafras.Application.DTOs.Usuarios;
using SecretariaConcafras.Domain.Entities;

public class UsuarioMappingProfile : Profile
{
    public UsuarioMappingProfile()
    {
        // Create
        CreateMap<UsuarioCreateDto, Usuario>();
        CreateMap<UsuarioRoleDto, UsuarioRole>(); // 🔥 faltava isso

        // Update
        CreateMap<UsuarioUpdateDto, Usuario>();
        CreateMap<UsuarioRoleDto, UsuarioRole>(); // também pode mapear aqui se precisar

        // Response
        CreateMap<Usuario, UsuarioResponseDto>()
            .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src => src.Roles));

        CreateMap<UsuarioRole, UsuarioRoleDto>(); // 🔥 mapeia o inverso

        // Login
        CreateMap<UsuarioLoginDto, Usuario>();

        // Auth response
        CreateMap<Usuario, UsuarioAuthResponseDto>()
            .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src => src.Roles));
    }
}
