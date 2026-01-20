using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class StockReservationConfiguration : IEntityTypeConfiguration<StockReservation>
{
    public void Configure(EntityTypeBuilder<StockReservation> builder)
    {
        builder.HasOne(sr => sr.ProductVariant)
            .WithMany()
            .HasForeignKey(sr => sr.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}