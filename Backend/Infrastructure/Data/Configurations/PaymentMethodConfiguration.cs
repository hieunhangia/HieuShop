using Domain.Constants;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.HasIndex(pm => pm.Code).IsUnique();
        builder.Property(pm => pm.Code)
            .HasMaxLength(DataSchema.PaymentMethod.CodeMaxLength)
            .IsUnicode(false);

        builder.Property(pm => pm.Name)
            .HasMaxLength(DataSchema.PaymentMethod.NameMaxLength);

        builder.Property(pm => pm.Description)
            .HasMaxLength(DataSchema.PaymentMethod.DescriptionMaxLength);
    }
}