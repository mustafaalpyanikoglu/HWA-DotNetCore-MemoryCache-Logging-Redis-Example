using Application.Models;
using Core.Application.Requests;
using MediatR;

namespace Application.Features.GetListProduct;

public class GetListProductQuery : IRequest<ProductListModel>
{
    public PageRequest PageRequest { get; set; }
}