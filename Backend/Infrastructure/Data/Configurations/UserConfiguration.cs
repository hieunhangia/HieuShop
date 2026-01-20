using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasOne(u => u.DefaultShippingAddress)
            .WithOne()
            .HasForeignKey<AppUser>(u => u.DefaultShippingAddressId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}