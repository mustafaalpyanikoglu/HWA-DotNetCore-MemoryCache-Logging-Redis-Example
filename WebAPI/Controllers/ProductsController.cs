using Application.Features.CreateProduct;
using Application.Features.GetListProduct;
using Core.Application.Requests;
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
    public async Task<IActionResult> GetProducts([FromQuery]PageRequest pageRequest)
    {
        var result = await _mediator.Send(new GetListProductQuery() { PageRequest = pageRequest });
        return Ok(result);
    }
    
    
}