using System.Linq.Expressions;
using DefaPress.Infrastructure.Context;
using DefaPress.Infrastructure.Modules.Base.Interfaces;
 
using Microsoft.EntityFrameworkCore;

namespace DefaPress.Infrastructure.Modules.Base.Implements;
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }



    public async Task AddAsync(T entity ,CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null, CancellationToken ct = default)
    {
        IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate); // حذف ToListAsync در این مرحله

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp); // حالا Include روی IQueryable اجرا می‌شود

            }
            ;
        }

        return await query.ToListAsync();
    }
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbSet.AnyAsync(predicate);
    }
    public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null, CancellationToken ct = default)
    {
        IQueryable<T> query = _dbSet.AsNoTracking(); // حذف ToListAsync در این مرحله

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp); // حالا Include روی IQueryable اجرا می‌شود

            }
            ;
        }

        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(object id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync(id);
    }

    public void Remove(T entity, CancellationToken ct = default)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity, CancellationToken ct = default)
    {
        _dbSet.Update(entity);
    }

}
