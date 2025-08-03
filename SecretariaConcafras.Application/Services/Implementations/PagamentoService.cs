using AutoMapper;
using SecretariaConcafras.Application.DTOs.Pagamentos;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Domain.Entities;
using SecretariaConcafras.Infrastructure.Repositories;

namespace SecretariaConcafras.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IRepository<Pagamento> _repository;
        private readonly IMapper _mapper;

        public PagamentoService(IRepository<Pagamento> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagamentoResponseDto> CriarAsync(PagamentoCreateDto dto)
        {
            var pagamento = _mapper.Map<Pagamento>(dto);
            await _repository.AddAsync(pagamento);
            return _mapper.Map<PagamentoResponseDto>(pagamento);
        }

        public async Task<PagamentoResponseDto?> AtualizarAsync(Guid id, PagamentoUpdateDto dto)
        {
            var pagamento = await _repository.GetByIdAsync(id);
            if (pagamento == null) return null;

            _mapper.Map(dto, pagamento);
            await _repository.UpdateAsync(pagamento);

            return _mapper.Map<PagamentoResponseDto>(pagamento);
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            var pagamento = await _repository.GetByIdAsync(id);
            if (pagamento == null) return false;

            await _repository.DeleteAsync(pagamento);
            return true;
        }

        public async Task<PagamentoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var pagamento = await _repository.GetByIdAsync(id);
            return pagamento == null ? null : _mapper.Map<PagamentoResponseDto>(pagamento);
        }

        public async Task<IEnumerable<PagamentoResponseDto>> ObterTodosAsync()
        {
            var pagamentos = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PagamentoResponseDto>>(pagamentos);
        }

        public async Task<IEnumerable<PagamentoResponseDto>> ObterPorInscricaoAsync(Guid inscricaoId)
        {
            var pagamentos = await _repository.FindAsync(p => p.InscricaoId == inscricaoId);
            return _mapper.Map<IEnumerable<PagamentoResponseDto>>(pagamentos);
        }
    }
}
