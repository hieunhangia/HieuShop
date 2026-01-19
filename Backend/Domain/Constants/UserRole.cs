namespace Domain.Constants;

public static class UserRole
{
    public const string Administrator = "Quản trị viên";
    public const string Manager = "Quản lý";
    public const string SalesStaff = "Nhân viên bán hàng";
    public const string DeliveryStaff = "Nhân viên giao hàng";
    public const string SupportStaff = "Nhân viên hỗ trợ";
    public const string Customer = "Khách hàng";

    public static readonly IReadOnlyCollection<string> All =
        [Administrator, Manager, SalesStaff, DeliveryStaff, SupportStaff, Customer];
}