using AutoMapper;
using SecretariaConcafras.Application.DTOs.Cursos;
using SecretariaConcafras.Application.DTOs.Enderecos;
using SecretariaConcafras.Application.DTOs.Eventos;
using SecretariaConcafras.Application.DTOs.Institutos;
using SecretariaConcafras.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SecretariaConcafras.Application.Mappings
{
    public class EventoMappingProfile : Profile
    {
        public EventoMappingProfile()
        {
            // ----- entrada (create) -----
            CreateMap<EnderecoCreateDto, Endereco>();
            CreateMap<EventoCreateDto, Evento>()
                .ForMember(d => d.EnderecoId, o => o.Ignore())
                .ForMember(d => d.Endereco, o => o.Ignore());

            // ----- saída (response) -----
            CreateMap<Endereco, EnderecoResponseDto>();
            CreateMap<Instituto, InstitutoResponseDto>();

            CreateMap<Curso, CursoResponseDto>();

            CreateMap<Evento, EventoResponseDto>()
                .ForMember(d => d.EnderecoCompleto, o => o.MapFrom(s => s.Endereco))
                .ForMember(d => d.Cursos, o => o.MapFrom(s => s.Cursos));
        }
    }
}
