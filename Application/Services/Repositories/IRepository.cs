using Domain;

namespace Application.Services.Repositories;

public interface IRepository<T> where T : Entity
{
    IQueryable<T> GetQuery();
    Task AddAsync(T entity);
}