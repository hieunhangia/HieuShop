using System.Reflection;
using Domain.Entities.Addresses;
using Domain.Entities.Carts;
using Domain.Entities.Coupons;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<UserShippingAddress> UserShippingAddresses => Set<UserShippingAddress>();
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<Ward> Wards => Set<Ward>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductOption> ProductOptions => Set<ProductOption>();
    public DbSet<ProductOptionValue> ProductOptionValues => Set<ProductOptionValue>();
    public DbSet<StockReservation> StockReservations => Set<StockReservation>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<UserCoupon> UserCoupons => Set<UserCoupon>();
    public DbSet<CouponApplicable> CouponApplicables => Set<CouponApplicable>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}