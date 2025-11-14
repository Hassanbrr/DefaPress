using System.Linq.Expressions;

namespace DefaPress.Infrastructure.Modules.Base.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(object id , string? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, CancellationToken ct = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity, CancellationToken ct = default);
    void Remove(T entity, CancellationToken ct = default);
}
