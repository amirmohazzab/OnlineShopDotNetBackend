

using Application.CQRS.ProductCommandQuery.Command;
using Application.CQRS.ProductCommandQuery.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductCQRSController : ControllerBase
{
    private readonly IMediator mediator;

    public ProductCQRSController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(SaveProductCommand saveProductCommand) 
    {
        var result = await mediator.Send(saveProductCommand);
        return Ok(result);
    }

    [HttpGet("Id")]
    public async Task<IActionResult> Get([FromQuery]GetProductQuery getProductQuery)
    {
        var result = await mediator.Send(getProductQuery);
        return Ok(result);
    }

}