using Application.Dtos;
using Core.Persistence.Paging;

namespace Application.Models;

public class ProductListModel : BasePageableModel
{
    public IList<ProductListDto> Items { get; set; }
}