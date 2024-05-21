using Application.Dtos;
using Application.Features.CreateProduct;
using Domain;

namespace Application.Extensions;

public static class MappingExtensions
{
    public static Product ToMap(this CreateProductCommand command)
    {
        return new Product(command.CategoryId, command.Name);
    }
    
    public static List<ProductListDto> ToMap(this List<Product> products)
    {
        return products
            .Select(x => new ProductListDto(x.Id, x.CategoryId, x.Name))
            .ToList();
    }
    
}