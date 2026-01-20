using Domain.Constants;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductOptionValueConfiguration : IEntityTypeConfiguration<ProductOptionValue>
{
    public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
    {
        builder.Property(ov => ov.Value)
            .HasMaxLength(DataSchema.ProductOptionValue.ValueMaxLength);

        builder.HasOne<ProductOption>()
            .WithMany(o => o.ProductOptionValues)
            .HasForeignKey(ov => ov.ProductOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}