
using Core.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.FluentAPIConfigurations;

public class UserRefreshTokenEntityConfiguration: IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable("UserRefreshToken");        
        builder.HasKey(s => s.Id);
        builder.Property(p => p.RefreshToken)
                .HasMaxLength(128);
    }
}
