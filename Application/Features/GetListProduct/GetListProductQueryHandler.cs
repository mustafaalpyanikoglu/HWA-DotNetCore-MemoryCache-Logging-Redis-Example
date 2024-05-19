using Application.Extensions;
using Application.Models;
using Application.Services.Repositories;
using MediatR;

namespace Application.Features.GetListProduct;

public class GetListProductQueryHandler(IProductRepository repository) : IRequestHandler<GetListProductQuery , ProductListModel>
{
    private readonly IProductRepository _repository = repository;
    
    public async Task<ProductListModel> Handle(GetListProductQuery request, CancellationToken cancellationToken)
    {
        var productsList = await _repository.GetListAsync();
        return productsList.ToMap();
    }
}