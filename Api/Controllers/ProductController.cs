using Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("[Controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;

    public ProductController(IProductService ProductService)
    {
        productService = ProductService;
    }

    [HttpGet("{Id}")]
    [SwaggerOperation(
        Summary = "Get a Product",
        Description = "Get a Product with Id",
        OperationId = "Products.Get",
        Tags = new[] {"ProductController"})
    ]
    public async Task<IActionResult> Get(int Id) {

        var result = await productService.Get(Id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {

        var result = await productService.GetAll();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDto model) {

        var result = await productService.Add(model);
        return Ok(result);
    }
    
}