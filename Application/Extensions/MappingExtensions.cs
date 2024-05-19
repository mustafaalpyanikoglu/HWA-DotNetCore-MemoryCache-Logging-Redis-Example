using Application.Dtos;
using Application.Features.CreateProduct;
using Application.Models;
using Core.Persistence.Paging;
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
    
    public static ProductListModel ToMap(this IPaginate<Product> paginate)
    {
        return new ProductListModel()
        {
            Index = paginate.Index,
            Size = paginate.Size,
            Count = paginate.Count,
            Pages = paginate.Pages,
            Items = paginate.Items.Select(item => 
                new ProductListDto(
                    item.Id, item.CategoryId, item.Name
                )).ToList(),
            HasPrevious = paginate.HasPrevious,
            HasNext = paginate.HasNext
        };
    }

    
}