using Core.Entities;
using Core.FluentAPIConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Core;
public class OnlineShopDbContext: DbContext
{
    public OnlineShopDbContext(DbContextOptions options):base(options)
    {
        
    }


    //public DbSet<Product> Products {get; set;}
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Product>()
        // .Property(b => b.ProductName)
        // .HasColumnName("Title")
        // .IsRequired();

        // modelBuilder.Entity<Product>().ToTable("PProduct");

        modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
        
    }

}