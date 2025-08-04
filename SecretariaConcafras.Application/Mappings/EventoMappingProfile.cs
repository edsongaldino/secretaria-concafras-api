using AutoMapper;
using SecretariaConcafras.Application.DTOs.Cursos;
using SecretariaConcafras.Application.DTOs.Eventos;
using SecretariaConcafras.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SecretariaConcafras.Application.Mappings
{
    public class EventoMappingProfile : Profile
    {
        public EventoMappingProfile()
        {
            CreateMap<Evento, EventoResponseDto>()
                .ForMember(dest => dest.EnderecoCompleto,
                           opt => opt.MapFrom(src =>
                               $"{src.Endereco.Logradouro}, {src.Endereco.Numero} - {src.Endereco.Cidade.Nome}/{src.Endereco.Cidade.Estado.Sigla}"));

            CreateMap<Curso, CursoResponseDto>()
                .ForMember(dest => dest.InstitutoNome, opt => opt.MapFrom(src => src.Instituto.Nome));

            CreateMap<EventoCreateDto, Evento>();
            CreateMap<EventoUpdateDto, Evento>();
        }
    }
}
