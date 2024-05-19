using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Domain;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class CategoryRepository(AppDbContext dbContext)
    : EfRepositoryBase<Category, AppDbContext>(dbContext), ICategoryRepository;