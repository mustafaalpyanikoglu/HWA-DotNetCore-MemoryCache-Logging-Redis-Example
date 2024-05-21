using Application.Dtos;
using Application.Services.Repositories;
using AutoMapper;
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
    
    public class GetListProductQueryHandler(IRepository<Product> repository, IMapper mapper) 
        : IRequestHandler<GetListProductQuery , List<ProductListDto>>
    {
        private readonly IRepository<Product> _repository = repository;
        private readonly IMapper _mapper = mapper;
        
        public async Task<List<ProductListDto>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.GetQuery();
            var productsList = await query.AsNoTracking().ToListAsync(cancellationToken);
            var mappedProductList = _mapper.Map<List<ProductListDto>>(productsList);
            return mappedProductList;
        }
    }
}
