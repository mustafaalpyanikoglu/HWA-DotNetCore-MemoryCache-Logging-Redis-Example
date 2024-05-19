using MediatR;

namespace Application.Features.CreateProduct;

public class CreateProductCommand : IRequest
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
}