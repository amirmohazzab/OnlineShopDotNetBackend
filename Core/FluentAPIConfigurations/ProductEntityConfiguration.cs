using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.FluentAPIConfigurations;

public class ProductEntityConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(s => s.Id);
        builder.Property(p => p.ProductName)
                .HasColumnName("Title")
                .HasMaxLength(256)
                .HasColumnOrder(1);
    }
}
