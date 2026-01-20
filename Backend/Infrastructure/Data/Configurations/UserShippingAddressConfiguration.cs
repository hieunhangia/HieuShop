using Domain.Constants;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserShippingAddressConfiguration : IEntityTypeConfiguration<UserShippingAddress>
{
    public void Configure(EntityTypeBuilder<UserShippingAddress> builder)
    {
        builder.Property(a => a.RecipientName)
            .HasMaxLength(DataSchema.UserShippingAddress.RecipientNameMaxLength);

        builder.Property(a => a.RecipientPhone)
            .HasMaxLength(DataSchema.UserShippingAddress.RecipientPhoneLength)
            .IsFixedLength()
            .IsUnicode(false);

        builder.Property(a => a.DetailAddress)
            .HasMaxLength(DataSchema.UserShippingAddress.DetailAddressMaxLength);

        builder.HasOne(a => a.Ward)
            .WithMany()
            .HasForeignKey(a => a.WardId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.User)
            .WithMany(u => u.ShippingAddresses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}