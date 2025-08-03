using AutoMapper;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Application.DTOs.Institutos;

namespace SecretariaConcafras.Application.Mappings
{
    public class InstitutoMappingProfile : Profile
    {
        public InstitutoMappingProfile()
        {
            CreateMap<Instituto, InstitutoResponseDto>()
                .ForMember(dest => dest.Cursos, opt => opt.MapFrom(src => src.Cursos));

            CreateMap<InstitutoCreateDto, Instituto>();
            CreateMap<InstitutoUpdateDto, Instituto>();
        }
    }
}
