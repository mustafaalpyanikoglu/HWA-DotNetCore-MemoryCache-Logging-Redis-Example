using Application.Services.Repositories;
using Domain;

namespace Application.Services.ProductServices;

public class ProductService (IRepository<Product> repository) : IProductService
{
    private readonly IRepository<Product> _repository = repository;
}