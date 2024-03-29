using Core.IRepositories;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DIRegister 
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }

    public static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddInfraUtility(this IServiceCollection services)
    {
        services.AddSingleton<EncryptionUtility>();
    }

}