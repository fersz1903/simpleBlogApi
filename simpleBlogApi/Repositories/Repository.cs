using Microsoft.EntityFrameworkCore;
using simpleBlogApi.Data;
using simpleBlogApi.Repositories.Interfaces;

namespace simpleBlogApi.Repositories;

public class Repository<T> : IRepository<T>
    where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<T?> GetByPublicIdAsync(Guid publicId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => EF.Property<Guid>(x, "PublicId") == publicId);
    }
}
