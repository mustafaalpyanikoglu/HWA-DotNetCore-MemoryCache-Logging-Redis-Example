using Core.Application.Caching;
using MediatR;

namespace Application.Features.CreateProduct;

public class CreateProductCommand : IRequest, ICacheRemoverRequest
{
    public int CategoryId { get; set; }
    public string Name { get; set; }

    public bool BypassCache { get; set; }
    public string? CacheKey { get; set; }
    public string[] CacheGroupKey => ["GetProducts"];
    
}