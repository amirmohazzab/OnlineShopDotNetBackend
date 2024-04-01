
using AutoMapper;
using Core;
using Core.Entities;
using Infrastructure.Dto;
using Infrastructure.Models;
using Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;
public class ProductService : IProductService
{
    private readonly OnlineShopDbContext dbContext;
    private readonly IMapper mapper;
    private readonly MyFileUtility fileUtility;
    private readonly ILogger logger;

    public ProductService(OnlineShopDbContext dbContext, IMapper mapper, MyFileUtility fileUtility, ILogger<ProductService> logger)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.fileUtility = fileUtility;
        this.logger = logger;
    }
    public async Task<ProductDto> Add(ProductDto model)
    {
        
        //var product = mapper.Map<Product>(model);
        logger.LogInformation("Call add from Product Service");

        var product = new Product 
        {
            Price = model.Price,
            ProductName = model.ProductName,
            // save in Folder
            ThumbnailFileName = fileUtility.SaveFileInFolder(model.Thumbnail, nameof(Product), true),
            // save in db -? byte[]
            Thumbnail = fileUtility.EncryptFile(fileUtility.ConvertToByteArray(model.Thumbnail)),
            ThumbnailFileExtension = fileUtility.GetFileExtension(model.Thumbnail.FileName),
            ThumbnailFileSize = model.Thumbnail.Length,
        };


        await dbContext.AddAsync(product);
        await dbContext.SaveChangesAsync();

        model.Id = product.Id;

        return model;

    }

    public async Task<ProductDto> Get(int Id)
    {
        var product = await dbContext.Products.FindAsync(Id);
        //var model = mapper.Map<ProductDto>(product);

        var model = new ProductDto {
            Id = product.Id,
            Price = product.Price,
            ProductName = product.ProductName,
            PriceWithComma = product.Price.ToString("###.###"),

            ThumbnailBase64 = fileUtility.ConvertToBase64(fileUtility.DecryptFile(product.Thumbnail)),
            ThumbnailUrl = fileUtility.GetEncryptedFileActionUrl(product.ThumbnailFileName, nameof(Product))
        };
        
        return model;
    }

    public async Task<ShopActionResult<List<ProductDto>>> GetAll(int page=1, int size=3)
    {
        // var result = await dbContext.Products.Select(product => new ProductDto{
        //     Id = product.Id,
        //     Price = product.Price,
        //     ProductName = product.ProductName,
        //     PriceWithComma = product.Price.ToString("###.###"),
        // }).ToListAsync();
        logger.LogInformation("Call add from Product Service");
        var result = new ShopActionResult<List<ProductDto>>();

        try
        {
            var products = await dbContext.Products
            .Skip((page-1)*size).Take(size)
            .AsNoTracking()
            .Select(q => new ProductDto {
                Id = q.Id,
                Price = q.Price,
                ProductName = q.ProductName
            })
            .ToListAsync();

            var totalRecordCount = await dbContext.Products.CountAsync();

            result.isSuccess = true;
            result.Data = products;
            result.Page = page;
            result.Size = size;
            result. Total = totalRecordCount;

            logger.LogInformation("Getall from ProductService success call");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            result.isSuccess = false;
            result.Message = ex.Message;
        }

        //var result = mapper.Map<List<ProductDto>>(products);

        return result;
    }
}