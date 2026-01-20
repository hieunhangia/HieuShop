using Domain.Constants;
using Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class WardConfiguration : IEntityTypeConfiguration<Ward>
{
    public void Configure(EntityTypeBuilder<Ward> builder)
    {
        builder.Property(w => w.Name)
            .HasMaxLength(DataSchema.Ward.NameMaxLength);

        builder.HasOne(w => w.Province)
            .WithMany(p => p.Wards)
            .HasForeignKey(w => w.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}