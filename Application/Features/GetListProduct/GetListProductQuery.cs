using Application.Models;
using Core.Application.Caching;
using Core.Application.Requests;
using MediatR;

namespace Application.Features.GetListProduct;

public class GetListProductQuery : IRequest<ProductListModel>, ICachableRequest
{
    public PageRequest PageRequest { get; set; }
    public bool BypassCache { get; set; }
    public string CacheKey => $"GetProducts({PageRequest.Page},{PageRequest.PageSize})";
    public string CacheGroupKey => "GetProducts";
    public TimeSpan? SlidingExpiration { get; set; }
}