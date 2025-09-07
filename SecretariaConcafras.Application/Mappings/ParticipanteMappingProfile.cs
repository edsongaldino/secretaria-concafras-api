// Application/Mapping/ParticipanteProfile.cs
using AutoMapper;
using SecretariaConcafras.Application.DTOs.Enderecos;
using SecretariaConcafras.Application.DTOs.Participantes;
using SecretariaConcafras.Domain.Entities;

public class ParticipanteProfile : Profile
{
    public ParticipanteProfile()
    {
        // Endereço DTO -> Entidade
        CreateMap<EnderecoCreateDto, Endereco>();

        // Entidade -> DTO de saída (se você tiver ParticipanteDto/ResponseDto, adicione aqui)
        // CreateMap<Participante, ParticipanteDto>()...

        // OBS: NÃO mapeamos navegação Instituto por aqui; resolvemos InstitutoId no service
    }
}
