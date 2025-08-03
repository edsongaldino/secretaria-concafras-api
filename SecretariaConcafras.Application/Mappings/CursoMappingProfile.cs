using AutoMapper;
using SecretariaConcafras.Application.DTOs.Cursos;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Application.Mappings
{
    public class CursoMappingProfile : Profile
    {
        public CursoMappingProfile()
        {
            CreateMap<Curso, CursoResponseDto>()
                .ForMember(dest => dest.EventoTitulo, opt => opt.MapFrom(src => src.Evento.Titulo))
                .ForMember(dest => dest.InstitutoNome, opt => opt.MapFrom(src => src.Instituto.Nome));

            CreateMap<CursoCreateDto, Curso>();
            CreateMap<CursoUpdateDto, Curso>();
        }
    }
}
