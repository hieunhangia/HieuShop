using Domain.Constants;
using Domain.Entities.Addresses;
using Domain.Entities.Carts;
using Domain.Entities.Coupons;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
{
    //Users
    public DbSet<UserShippingAddress> UserShippingAddresses => Set<UserShippingAddress>();

    //Addresses
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<Ward> Wards => Set<Ward>();

    //Products
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductOption> ProductOptions => Set<ProductOption>();
    public DbSet<ProductOptionValue> ProductOptionValues => Set<ProductOptionValue>();
    public DbSet<StockReservation> StockReservations => Set<StockReservation>();

    //Carts
    public DbSet<CartItem> CartItems => Set<CartItem>();

    //Orders
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();

    //Coupons
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponApplicable> CouponApplicables => Set<CouponApplicable>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureAppUser(modelBuilder.Entity<AppUser>());

        ConfigureUserShippingAddress(modelBuilder.Entity<UserShippingAddress>());

        ConfigureProvince(modelBuilder.Entity<Province>());

        ConfigureWard(modelBuilder.Entity<Ward>());

        ConfigureProduct(modelBuilder.Entity<Product>());

        ConfigureCategory(modelBuilder.Entity<Category>());

        ConfigureBrand(modelBuilder.Entity<Brand>());

        ConfigureProductVariant(modelBuilder.Entity<ProductVariant>());

        ConfigureProductOption(modelBuilder.Entity<ProductOption>());

        ConfigureProductOptionValue(modelBuilder.Entity<ProductOptionValue>());

        ConfigureStockReservation(modelBuilder.Entity<StockReservation>());

        ConfigureCartItem(modelBuilder.Entity<CartItem>());

        ConfigureOrder(modelBuilder.Entity<Order>());

        ConfigureOrderItem(modelBuilder.Entity<OrderItem>());

        ConfigurePaymentMethod(modelBuilder.Entity<PaymentMethod>());

        ConfigureCoupon(modelBuilder.Entity<Coupon>());

        ConfigureCouponApplicable(modelBuilder.Entity<CouponApplicable>());

        ConfigureCouponApplicableProduct(modelBuilder.Entity<CouponApplicableProduct>());

        ConfigureCouponApplicableCategory(modelBuilder.Entity<CouponApplicableCategory>());

        ConfigureCouponApplicableBrand(modelBuilder.Entity<CouponApplicableBrand>());

        ConfigureIdentityTablesName(modelBuilder);
    }

    private static void ConfigureAppUser(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasOne(u => u.DefaultShippingAddress)
            .WithOne()
            .HasForeignKey<AppUser>(u => u.DefaultShippingAddressId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureUserShippingAddress(EntityTypeBuilder<UserShippingAddress> builder)
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

    private static void ConfigureProvince(EntityTypeBuilder<Province> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(DataSchema.Province.NameMaxLength);
    }

    private static void ConfigureWard(EntityTypeBuilder<Ward> builder)
    {
        builder.Property(w => w.Name)
            .HasMaxLength(DataSchema.Ward.NameMaxLength);

        builder.HasOne(w => w.Province)
            .WithMany(p => p.Wards)
            .HasForeignKey(w => w.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureProduct(EntityTypeBuilder<Product> builder)
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

        builder.HasOne(p => p.DefaultProductVariant)
            .WithOne()
            .HasForeignKey<Product>(p => p.DefaultProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCategory(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(DataSchema.Category.NameMaxLength);

        builder.HasIndex(c => c.Slug).IsUnique();
        builder.Property(c => c.Slug)
            .HasMaxLength(DataSchema.Category.SlugMaxLength)
            .IsUnicode(false);

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(DataSchema.Category.ImageUrlMaxLength)
            .IsUnicode(false);
    }

    private static void ConfigureBrand(EntityTypeBuilder<Brand> builder)
    {
        builder.Property(b => b.Name)
            .HasMaxLength(DataSchema.Brand.NameMaxLength);

        builder.HasIndex(b => b.Slug).IsUnique();
        builder.Property(b => b.Slug)
            .HasMaxLength(DataSchema.Brand.SlugMaxLength)
            .IsUnicode(false);

        builder.Property(b => b.LogoUrl)
            .HasMaxLength(DataSchema.Brand.LogoUrlMaxLength)
            .IsUnicode(false);
    }

    private static void ConfigureProductVariant(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.Property(v => v.ImageUrl)
            .HasMaxLength(DataSchema.ProductVariant.ImageUrlMaxLength)
            .IsUnicode(false);

        builder.HasOne<Product>()
            .WithMany(p => p.ProductVariants)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.ProductOptionValues)
            .WithMany()
            .UsingEntity(vov => vov.ToTable("ProductVariantOptionsValues"));
    }

    private static void ConfigureProductOption(EntityTypeBuilder<ProductOption> builder)
    {
        builder.Property(o => o.Name)
            .HasMaxLength(DataSchema.ProductOption.NameMaxLength);

        builder.HasOne<Product>()
            .WithMany(p => p.ProductOptions)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureProductOptionValue(EntityTypeBuilder<ProductOptionValue> builder)
    {
        builder.Property(ov => ov.Value)
            .HasMaxLength(DataSchema.ProductOptionValue.ValueMaxLength);

        builder.HasOne<ProductOption>()
            .WithMany(o => o.ProductOptionValues)
            .HasForeignKey(ov => ov.ProductOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureStockReservation(EntityTypeBuilder<StockReservation> builder)
    {
        builder.HasOne(sr => sr.ProductVariant)
            .WithMany()
            .HasForeignKey(sr => sr.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCartItem(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasOne(ci => ci.User)
            .WithMany(u => u.CartItems)
            .HasForeignKey(ci => ci.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ci => ci.ProductVariant)
            .WithMany()
            .HasForeignKey(ci => ci.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureOrder(EntityTypeBuilder<Order> builder)
    {
        builder.HasIndex(o => o.OrderCode).IsUnique();
        builder.Property(o => o.OrderCode)
            .HasMaxLength(DataSchema.Order.OrderCodeLength)
            .IsFixedLength()
            .IsUnicode(false);

        builder.HasOne(o => o.PaymentMethod)
            .WithMany()
            .HasForeignKey(o => o.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Coupon)
            .WithMany()
            .HasForeignKey(o => o.CouponId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureOrderItem(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(oi => oi.ProductName)
            .HasMaxLength(DataSchema.OrderItem.ProductNameMaxLength);

        builder.HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oi => oi.ProductVariant)
            .WithMany()
            .HasForeignKey(oi => oi.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigurePaymentMethod(EntityTypeBuilder<PaymentMethod> builder)
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

    private static void ConfigureCoupon(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasIndex(c => c.Code).IsUnique();
        builder.Property(c => c.Code)
            .HasMaxLength(DataSchema.Coupon.CodeMaxLength)
            .IsUnicode(false);

        builder.Property(c => c.Name)
            .HasMaxLength(DataSchema.Coupon.NameMaxLength);

        builder.Property(c => c.Description)
            .HasMaxLength(DataSchema.Coupon.DescriptionMaxLength);
    }

    private static void ConfigureCouponApplicable(EntityTypeBuilder<CouponApplicable> builder)
    {
        builder.HasOne(ca => ca.Coupon)
            .WithMany(c => c.CouponApplicables)
            .HasForeignKey(ca => ca.CouponId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasDiscriminator<string>("ApplicableType")
            .HasValue<CouponApplicableAll>("All")
            .HasValue<CouponApplicableProduct>("Product")
            .HasValue<CouponApplicableCategory>("Category")
            .HasValue<CouponApplicableBrand>("Brand");
    }

    private static void ConfigureCouponApplicableProduct(EntityTypeBuilder<CouponApplicableProduct> builder)
    {
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(cap => cap.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCouponApplicableCategory(EntityTypeBuilder<CouponApplicableCategory> builder)
    {
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(cac => cac.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCouponApplicableBrand(EntityTypeBuilder<CouponApplicableBrand> builder)
    {
        builder.HasOne<Brand>()
            .WithMany()
            .HasForeignKey(cab => cab.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureIdentityTablesName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(b => b.ToTable("Users"));
        modelBuilder.Entity<IdentityUserClaim<Guid>>(b => b.ToTable("UserClaims"));
        modelBuilder.Entity<IdentityUserLogin<Guid>>(b => b.ToTable("UserLogins"));
        modelBuilder.Entity<IdentityUserToken<Guid>>(b => b.ToTable("UserTokens"));
        modelBuilder.Entity<IdentityRole<Guid>>(b => b.ToTable("Roles"));
        modelBuilder.Entity<IdentityRoleClaim<Guid>>(b => b.ToTable("RoleClaims"));
        modelBuilder.Entity<IdentityUserRole<Guid>>(b => b.ToTable("UserRoles"));
    }
}