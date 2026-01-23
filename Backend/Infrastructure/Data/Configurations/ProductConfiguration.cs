using Domain.Constants;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(DataSchema.Product.NameMaxLength);

        builder.HasIndex(p => p.Slug).IsUnique();
        builder.Property(p => p.Slug)
            .HasMaxLength(DataSchema.Product.SlugMaxLength)
            .IsUnicode(false);

        builder.HasMany(p => p.Categories)
            .WithMany(c => c.Products)
            .UsingEntity(pc => pc.ToTable("ProductCategories"));

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.DefaultProductImage)
            .WithOne()
            .HasForeignKey<Product>(p => p.DefaultProductImageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.DefaultProductVariant)
            .WithOne()
            .HasForeignKey<Product>(p => p.DefaultProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}