using AutoMapper;
using SecretariaConcafras.Application.DTOs.Participantes;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Application.Mappings
{
    public class ParticipanteMappingProfile : Profile
    {
        public ParticipanteMappingProfile()
        {
            CreateMap<ParticipanteCreateDto, Participante>();
            CreateMap<ParticipanteUpdateDto, Participante>();

            CreateMap<Participante, ParticipanteResponseDto>()
                .ForMember(dest => dest.ResponsavelNome, opt => opt.MapFrom(src => src.Responsavel != null ? src.Responsavel.Nome : null))
                .ForMember(dest => dest.InstituicaoNome, opt => opt.MapFrom(src => src.Instituicao != null ? src.Instituicao.Nome : null))
                .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.Endereco.Cidade.Nome))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Endereco.Cidade.Estado.Nome));
        }
    }
}
