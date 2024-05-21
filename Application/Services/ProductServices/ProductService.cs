using Application.Services.Repositories;
using AutoMapper;
using Domain;

namespace Application.Services.ProductServices;

public class ProductService (IRepository<Product> repository, IMapper mapper) : IProductService
{
    private readonly IRepository<Product> _repository = repository;
    private readonly IMapper _mapper = mapper;
}