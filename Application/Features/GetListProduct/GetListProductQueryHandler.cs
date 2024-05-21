using Application.Dtos;
using Application.Extensions;
using Application.Services.Repositories;
using Core.Application.Caching;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.GetListProduct;

public class GetListProductQuery : IRequest<List<ProductListDto>>, ICachableRequest
{
    public bool BypassCache { get; set; }
    public string CacheKey => $"GetListProductQuery()";
    public string CacheGroupKey => "GetProducts";
    public TimeSpan? SlidingExpiration { get; set; }
    
    public class GetListProductQueryHandler(IRepository<Product> repository) 
        : IRequestHandler<GetListProductQuery , List<ProductListDto>>
    {
        private readonly IRepository<Product> _repository = repository;
    
        public async Task<List<ProductListDto>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.GetQuery();
            var productsList = await query.AsNoTracking().ToListAsync(cancellationToken);
            return productsList.ToMap();
        }
    }
}
