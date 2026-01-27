using Domain.Constants;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductOptionValueConfiguration : IEntityTypeConfiguration<ProductOptionValue>
{
    public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
    {
        builder.Property(pov => pov.Value)
            .HasMaxLength(DataSchema.ProductOptionValue.ValueMaxLength);

        builder.HasOne(pov => pov.ProductOption)
            .WithMany(po => po.ProductOptionValues)
            .HasForeignKey(pov => pov.ProductOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}