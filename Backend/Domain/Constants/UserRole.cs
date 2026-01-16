namespace Domain.Constants;

public static class UserRole
{
    public const string Administrator = "Administrator";
    public const string Manager = "Manager";
    public const string SalesStaff = "Sales Staff";
    public const string DeliveryStaff = "Delivery Staff";
    public const string SupportStaff = "Support Staff";
    public const string Customer = "Customer";

    public static readonly IReadOnlyCollection<string> All =
        [Administrator, Manager, SalesStaff, DeliveryStaff, SupportStaff, Customer];
}