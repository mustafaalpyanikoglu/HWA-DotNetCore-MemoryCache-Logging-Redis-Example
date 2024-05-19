using Core.Persistence.Repositories;
using Domain;

namespace Application.Services.Repositories;

public interface IProductRepository  : IAsyncRepository<Product>, IRepository<Product> {}