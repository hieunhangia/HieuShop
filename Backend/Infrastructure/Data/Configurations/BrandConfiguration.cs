using Domain.Constants;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.Property(b => b.Name)
            .HasMaxLength(DataSchema.Brand.NameMaxLength);

        builder.HasIndex(b => b.Slug).IsUnique();
        builder.Property(b => b.Slug)
            .HasMaxLength(DataSchema.Brand.SlugMaxLength)
            .IsUnicode(false);
        
        builder.HasIndex(b => b.DisplayOrder).IsUnique();

        builder.Property(b => b.LogoUrl)
            .HasMaxLength(DataSchema.Brand.LogoUrlMaxLength)
            .IsUnicode(false);
    }
}