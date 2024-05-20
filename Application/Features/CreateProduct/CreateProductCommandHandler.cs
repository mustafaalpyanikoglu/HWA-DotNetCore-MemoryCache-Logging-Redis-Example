using Application.Extensions;
using Application.Services.Repositories;
using MediatR;

namespace Application.Features.CreateProduct;

public class CreateProductCommandHandler(IProductRepository repository) : IRequestHandler<CreateProductCommand>
{
    private readonly IProductRepository _repository = repository;
    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        throw new Exception();
        var mappedProduct = request.ToMap();
        await _repository.AddAsync(mappedProduct);
    }
}