using AutoMapper;
using SecretariaConcafras.Application.DTOs.Inscricoes;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Application.Mappings
{
    public class InscricaoMappingProfile : Profile
    {
        public InscricaoMappingProfile()
        {
            // De DTO -> Entidade
            CreateMap<InscricaoCreateDto, Inscricao>();

            // De Entidade -> DTO de resposta
            CreateMap<Inscricao, InscricaoResponseDto>()
                .ForMember(dest => dest.ParticipanteNome, opt => opt.MapFrom(src => src.Participante.Nome))
                .ForMember(dest => dest.EventoTitulo, opt => opt.MapFrom(src => src.Evento.Titulo))

                // Se for inscrição em curso
                .ForMember(dest => dest.Cursos,
                    opt => opt.MapFrom(src => src.InscricaoCurso != null ? src.InscricaoCurso.Curso.Titulo : null))

                // Se for trabalhador em comissão
                .ForMember(dest => dest.Comissao,
                    opt => opt.MapFrom(src => src.InscricaoTrabalhador != null ? src.InscricaoTrabalhador.ComissaoTrabalho.Nome : null));
        }
    }
}
