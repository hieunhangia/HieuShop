using Domain.Constants;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(DataSchema.Category.NameMaxLength);

        builder.HasIndex(c => c.Slug).IsUnique();
        builder.Property(c => c.Slug)
            .HasMaxLength(DataSchema.Category.SlugMaxLength)
            .IsUnicode(false);
        
        builder.HasIndex(c => c.DisplayOrder).IsUnique();

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(DataSchema.Category.ImageUrlMaxLength)
            .IsUnicode(false);
    }
}