namespace Domain.Constants;

public static class DataSchema
{
    public static class UserShippingAddress
    {
        public const int RecipientNameMaxLength = 100;
        public const int RecipientPhoneLength = 10;
        public const int DetailAddressMaxLength = 500;
    }

    public static class Province
    {
        public const int NameMaxLength = 10;
    }

    public static class Ward
    {
        public const int NameMaxLength = 50;
    }

    public static class Product
    {
        public const int NameMaxLength = 200;
        public const int SlugMaxLength = 100;
    }

    public static class Category
    {
        public const int NameMaxLength = 100;
        public const int SlugMaxLength = 100;
        public const int ImageUrlMaxLength = 500;
    }

    public static class Brand
    {
        public const int NameMaxLength = 100;
        public const int SlugMaxLength = 100;
        public const int LogoUrlMaxLength = 500;
    }

    public static class ProductImage
    {
        public const int ImageUrlMaxLength = 500;
    }

    public static class ProductOption
    {
        public const int NameMaxLength = 50;
    }

    public static class ProductOptionValue
    {
        public const int ValueMaxLength = 50;
    }

    public static class Order
    {
        public const int OrderCodeLength = 12;
    }

    public static class OrderItem
    {
        public const int ProductNameMaxLength = Product.NameMaxLength;
    }

    public static class PaymentMethod
    {
        public const int CodeMaxLength = 50;
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;
    }

    public static class Coupon
    {
        public const int CodeMaxLength = 50;
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;
    }
}