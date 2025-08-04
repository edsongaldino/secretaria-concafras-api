using AutoMapper;
using SecretariaConcafras.Application.DTOs.Institutos;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Domain.Interfaces;

namespace SecretariaConcafras.Application.Services;

public class InstitutoService : IInstitutoService
{
    private readonly IRepository<Instituto> _repository;
    private readonly IMapper _mapper;

    public InstitutoService(IRepository<Instituto> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<InstitutoResponseDto> CriarAsync(InstitutoCreateDto dto)
    {
        var instituto = _mapper.Map<Instituto>(dto);
        await _repository.AddAsync(instituto);
        return _mapper.Map<InstitutoResponseDto>(instituto);
    }

    public async Task<InstitutoResponseDto?> AtualizarAsync(Guid id, InstitutoUpdateDto dto)
    {
        var instituto = await _repository.GetByIdAsync(id);
        if (instituto == null) return null;

        _mapper.Map(dto, instituto);
        await _repository.UpdateAsync(instituto);

        return _mapper.Map<InstitutoResponseDto>(instituto);
    }

    public async Task<bool> DeletarAsync(Guid id)
    {
        var instituto = await _repository.GetByIdAsync(id);
        if (instituto == null) return false;

        await _repository.DeleteAsync(instituto);
        return true;
    }

    public async Task<InstitutoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var instituto = await _repository.GetByIdAsync(id);
        return instituto == null ? null : _mapper.Map<InstitutoResponseDto>(instituto);
    }

    public async Task<IEnumerable<InstitutoResponseDto>> ObterTodosAsync()
    {
        var institutos = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<InstitutoResponseDto>>(institutos);
    }
}
