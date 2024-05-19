using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Domain;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ProductRepository(AppDbContext dbContext)
    : EfRepositoryBase<Product, AppDbContext>(dbContext), IProductRepository;