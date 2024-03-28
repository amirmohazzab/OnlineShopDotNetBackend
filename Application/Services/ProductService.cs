
using AutoMapper;
using Core;
using Core.Entities;
using Infrastructure.Dto;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;
public class ProductService : IProductService
{
    private readonly OnlineShopDbContext dbContext;
    private readonly IMapper mapper;

    public ProductService(OnlineShopDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    public async Task<ProductDto> Add(ProductDto model)
    {
        var product = mapper.Map<Product>(model);
        // var product = new Product {
        //     ProductName = model.ProductName,
        //     Price = model.Price
        // };

        await dbContext.AddAsync(product);
        await dbContext.SaveChangesAsync();

        model.Id = product.Id;

        return model;

    }

    public async Task<ProductDto> Get(int Id)
    {
        var product = await dbContext.Products.FindAsync(Id);
        var model = mapper.Map<ProductDto>(product);
        // var model = new ProductDto {
        //     Id = product.Id,
        //     Price = product.Price,
        //     ProductName = product.ProductName,
        //     PriceWithComma = product.Price.ToString("###.###"),
        // };
        
        return model;
    }

    public async Task<List<ProductDto>> GetAll()
    {
        // var result = await dbContext.Products.Select(product => new ProductDto{
        //     Id = product.Id,
        //     Price = product.Price,
        //     ProductName = product.ProductName,
        //     PriceWithComma = product.Price.ToString("###.###"),
        // }).ToListAsync();

        var products = await dbContext.Products.ToListAsync();
        var result = mapper.Map<List<ProductDto>>(products);

        return result;
    }
}