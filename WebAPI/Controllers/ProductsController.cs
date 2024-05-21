using Application.Features.CreateProduct;
using Application.Features.GetListProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _mediator.Send(new GetListProductQuery() { });
        return Ok(result);
    }
    
    
}