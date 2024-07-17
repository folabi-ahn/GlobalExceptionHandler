using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalExceptionHandler.Api;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);
        builder.Property(product => product.Name).HasMaxLength(60);
        builder.HasIndex(product => product.Name).IsUnique();
        builder.Property(product => product.Price).HasPrecision(18, 2);
    }
}
