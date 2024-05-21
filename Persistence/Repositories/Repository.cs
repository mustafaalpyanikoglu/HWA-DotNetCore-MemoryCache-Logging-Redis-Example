using Application.Services.Repositories;
using Persistence.Contexts;
using Domain;

namespace Persistence.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : Entity
{
    private readonly AppDbContext _context = context;

    public IQueryable<T> GetQuery()
    {
        return _context.Set<T>().AsQueryable();
    }
    
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}