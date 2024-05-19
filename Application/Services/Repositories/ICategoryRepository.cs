using Core.Persistence.Repositories;
using Domain;

namespace Application.Services.Repositories;

public interface ICategoryRepository : IAsyncRepository<Category>, IRepository<Category> {}