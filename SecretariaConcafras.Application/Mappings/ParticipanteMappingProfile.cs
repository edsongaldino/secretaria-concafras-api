using AutoMapper;
using SecretariaConcafras.Application.DTOs.Participantes;
using SecretariaConcafras.Domain.Entities;

namespace SecretariaConcafras.Application.Mappings
{
    public class ParticipanteMappingProfile : Profile
    {
        public ParticipanteMappingProfile()
        {
            // mapeia entidade -> dto
            CreateMap<Participante, ParticipanteResponseDto>()
                .ForMember(d => d.InstituicaoNome,
                    opt => opt.MapFrom(s => s.Instituicao != null ? s.Instituicao.Nome : null))
                .ForMember(d => d.Endereco, opt => opt.MapFrom(s => s.Endereco));

            CreateMap<Endereco, ParticipanteResponseDto.EnderecoDto>();
        }
    }
}
