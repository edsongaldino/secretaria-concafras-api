using System.Linq.Expressions;

namespace SecretariaConcafras.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        // Se precisar de consultas mais flexíveis
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
