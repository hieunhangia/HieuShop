using Domain.Constants;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.Property(pv => pv.ImageUrl)
            .HasMaxLength(DataSchema.ProductVariant.ImageUrlMaxLength)
            .IsUnicode(false);

        builder.HasOne(pv => pv.Product)
            .WithMany(p => p.ProductVariants)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(pv => pv.ProductOptionValues)
            .WithMany()
            .UsingEntity(pvov => pvov.ToTable("ProductVariantOptionsValues"));
    }
}