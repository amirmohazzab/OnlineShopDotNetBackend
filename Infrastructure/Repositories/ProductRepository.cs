

using Core;
using Core.Entities;
using Core.IRepositories;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly OnlineShopDbContext onlineShopDbContext;

    public ProductRepository(OnlineShopDbContext onlineShopDbContext)
    {
        this.onlineShopDbContext = onlineShopDbContext;
    }
    public async Task<Product> GetAsync(int Id)
    {
        return await onlineShopDbContext.Products.FindAsync(Id);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> InsertAsync(Product product)
    {
        await onlineShopDbContext.AddAsync(product);

        return product.Id;
    }
}