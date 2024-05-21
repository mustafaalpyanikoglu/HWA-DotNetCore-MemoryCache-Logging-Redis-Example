using Application.Extensions;
using Application.Services.Repositories;
using Core.Application.Caching;
using Domain;
using MediatR;

namespace Application.Features.CreateProduct;

public class CreateProductCommand : IRequest, ICacheRemoverRequest
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public bool BypassCache { get; set; }
    public string? CacheKey { get; set; }
    public string[] CacheGroupKey => ["GetProducts"];
    
    public class CreateProductCommandHandler(IRepository<Product> repository) 
        : IRequestHandler<CreateProductCommand>
    {
        private readonly IRepository<Product> _repository = repository;
        public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var mappedProduct = request.ToMap();
            await _repository.AddAsync(mappedProduct);
        }
    }
}

