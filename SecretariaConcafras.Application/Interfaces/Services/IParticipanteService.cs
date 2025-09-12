using SecretariaConcafras.Application.DTOs.Participantes;

public interface IParticipanteService
{
    Task<ParticipanteResultadoDto> UpsertPorCpfAsync(ParticipanteCreateDto dto, CancellationToken ct = default);
    Task<ParticipanteResponseDto?> ObterPorIdAsync(Guid id);
}
