


using Core;
using Infrastructure.Interfaces;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly OnlineShopDbContext onlineShopDbContext;

    public UnitOfWork(OnlineShopDbContext onlineShopDbContext)
    {
        this.onlineShopDbContext = onlineShopDbContext;
    }

    public void Dispose()
    {
        onlineShopDbContext.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await onlineShopDbContext.SaveChangesAsync();
    }
}