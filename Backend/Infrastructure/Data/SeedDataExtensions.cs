using Domain.Constants;
using Domain.Entities.Addresses;
using Domain.Entities.Coupons;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Entities.Users;
using Domain.Enums.Coupons;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class SeedDataExtensions
{
    public static async Task SeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var scopeService = scope.ServiceProvider;

        await using var dbContext = scopeService.GetRequiredService<AppDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var roleManager = scopeService.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scopeService.GetRequiredService<UserManager<AppUser>>();

        await SeedIdentityDataAsync(roleManager, userManager);

        SeedAddressesData(dbContext);

        SeedUserShippingAddressesData(dbContext);

        SeedProductsData(dbContext);

        SeedCouponsData(dbContext);

        SeedUserCouponData(dbContext);

        SeedPaymentMethodsData(dbContext);
    }

    private static async Task SeedIdentityDataAsync(RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<AppUser> userManager)
    {
        foreach (var role in UserRole.All)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        ICollection<(string Email, string Password, bool LockoutEnabled, string[] Roles)> users =
        [
            (
                "admin@app.com",
                "Admin@123",
                false,
                UserRole.All.ToArray()
            ),
            (
                "manager@app.com",
                "Manager@123",
                true,
                [UserRole.Manager]
            )
        ];
        for (var i = 1; i <= 5; i++)
        {
            var item = new ValueTuple<string, string, bool, string[]>
            {
                Item1 = "customer" + i + "@app.com",
                Item2 = "Customer@123",
                Item3 = true,
                Item4 = [UserRole.Customer]
            };
            users.Add(item);
        }

        foreach (var userData in users)
        {
            var user = new AppUser
            {
                UserName = userData.Email,
                Email = userData.Email
            };
            await userManager.CreateAsync(user, userData.Password);
            await userManager.SetLockoutEnabledAsync(user, userData.LockoutEnabled);
            await userManager.ConfirmEmailAsync(user, await userManager.GenerateEmailConfirmationTokenAsync(user));
            await userManager.AddToRolesAsync(user, userData.Roles);
        }
    }

    private static void SeedAddressesData(AppDbContext dbContext)
    {
        IReadOnlyCollection<string> hanoiWards =
        [
            "Phường Ba Đình", "Phường Ngọc Hà", "Phường Giảng Võ", "Phường Hoàn Kiếm", "Phường Cửa Nam",
            "Phường Phú Thượng", "Phường Hồng Hà", "Phường Tây Hồ", "Phường Bồ Đề", "Phường Việt Hưng",
            "Phường Phúc Lợi", "Phường Long Biên", "Phường Nghĩa Đô", "Phường Cầu Giấy", "Phường Yên Hòa",
            "Phường Ô Chợ Dừa", "Phường Láng", "Phường Văn Miếu - Quốc Tử Giám", "Phường Kim Liên", "Phường Đống Đa",
            "Phường Hai Bà Trưng", "Phường Vĩnh Tuy", "Phường Bạch Mai", "Phường Vĩnh Hưng", "Phường Định Công",
            "Phường Tương Mai", "Phường Lĩnh Nam", "Phường Hoàng Mai", "Phường Hoàng Liệt", "Phường Yên Sở",
            "Phường Phương Liệt", "Phường Khương Đình", "Phường Thanh Xuân", "Xã Sóc Sơn", "Xã Kim Anh", "Xã Trung Giã",
            "Xã Đa Phúc", "Xã Nội Bài", "Xã Đông Anh", "Xã Phúc Thịnh", "Xã Thư Lâm", "Xã Thiên Lộc", "Xã Vĩnh Thanh",
            "Xã Phù Đổng", "Xã Thuận An", "Xã Gia Lâm", "Xã Bát Tràng"
        ];
        IReadOnlyCollection<string> caobangWards =
        [
            "Phường Thục Phán", "Phường Nùng Trí Cao", "Phường Tân Giang", "Xã Bảo Lâm", "Xã Lý Bôn", "Xã Nam Quang",
            "Xã Quảng Lâm", "Xã Yên Thổ", "Xã Bảo Lạc", "Xã Cốc Pàng", "Xã Cô Ba", "Xã Khánh Xuân", "Xã Xuân Trường",
            "Xã Hưng Đạo", "Xã Huy Giáp", "Xã Sơn Lộ", "Xã Thông Nông", "Xã Cần Yên", "Xã Thanh Long", "Xã Trường Hà",
            "Xã Lũng Nặm", "Xã Tổng Cọt", "Xã Hà Quảng", "Xã Trà Lĩnh", "Xã Quang Hán", "Xã Quang Trung",
            "Xã Trùng Khánh", "Xã Đình Phong", "Xã Đàm Thủy", "Xã Đoài Dương", "Xã Lý Quốc", "Xã Quang Long",
            "Xã Hạ Lang", "Xã Vinh Quý", "Thành phố Cao Bằng"
        ];
        IReadOnlyCollection<string> thanhhoaWards =
        [
            "Xã Thanh Quân", "Xã Thượng Ninh", "Xã Như Thanh", "Xã Xuân Du", "Xã Mậu Lâm", "Xã Xuân Thái", "Xã Yên Thọ",
            "Xã Thanh Kỳ", "Xã Nông Cống", "Xã Trung Chính", "Xã Thắng Lợi", "Xã Thăng Bình", "Xã Trường Văn",
            "Xã Tượng Lĩnh", "Xã Công Chính", "Phường Đông Sơn", "Phường Đông Quang", "Xã Lưu Vệ", "Xã Quảng Yên",
            "Xã Quảng Chính", "Xã Quảng Ngọc", "Phường Nam Sầm Sơn", "Phường Quảng Phú", "Phường Sầm Sơn",
            "Xã Quảng Ninh", "Xã Quảng Bình", "Xã Tiên Trang", "Phường Tĩnh Gia", "Phường Ngọc Sơn", "Xã Các Sơn",
            "Phường Tân Dân", "Phường Hải Lĩnh", "Phường Đào Duy Từ", "Phường Trúc Lâm", "Xã Trường Lâm",
            "Phường Hải Bình", "Phường Nghi Sơn"
        ];

        dbContext.AddRange(new List<Province>
        {
            new() { Name = "Thành phố Hà Nội", Wards = hanoiWards.Select(w => new Ward { Name = w }).ToList() },
            new() { Name = "Tỉnh Cao Bằng", Wards = caobangWards.Select(w => new Ward { Name = w }).ToList() },
            new() { Name = "Tỉnh Thanh Hóa", Wards = thanhhoaWards.Select(w => new Ward { Name = w }).ToList() }
        });
        dbContext.SaveChanges();
    }

    private static void SeedUserShippingAddressesData(AppDbContext dbContext)
    {
        var users = dbContext.Users.ToList();
        foreach (var user in users)
        {
            user.ShippingAddresses = new List<UserShippingAddress>
            {
                new()
                {
                    RecipientName = "Tay Trừ Tà",
                    RecipientPhone = "0888888888",
                    DetailAddress = "Biên giới",
                    Ward = dbContext.Wards.First(w => w.Name == "Thành phố Cao Bằng")
                },
                new()
                {
                    RecipientName = "Hóa Thanh Sư",
                    RecipientPhone = "0363636363",
                    DetailAddress = "Đường Tàu",
                    Ward = dbContext.Wards.First(w => w.Name == "Xã Nông Cống")
                }
            };
        }

        dbContext.Users.UpdateRange(users);
        dbContext.SaveChanges();
    }

    private static void SeedProductsData(AppDbContext dbContext)
    {
        var displayOrder = 1;
        var appleBrand = CreateBrand("Apple", "apple", displayOrder++);
        var samsungBrand = CreateBrand("Samsung", "samsung", displayOrder++);
        var xiaomiBrand = CreateBrand("Xiaomi", "xiaomi", displayOrder++);
        var oppoBrand = CreateBrand("OPPO", "oppo", displayOrder++);
        var googleBrand = CreateBrand("Google Pixel", "google-pixel", displayOrder++);
        var sonyBrand = CreateBrand("Sony", "sony", displayOrder++);
        var nokiaBrand = CreateBrand("Nokia", "nokia", displayOrder);
        dbContext.Brands.Add(appleBrand);
        dbContext.Brands.Add(samsungBrand);
        dbContext.Brands.Add(xiaomiBrand);
        dbContext.Brands.Add(oppoBrand);
        dbContext.Brands.Add(googleBrand);
        dbContext.Brands.Add(sonyBrand);
        dbContext.Brands.Add(nokiaBrand);

        displayOrder = 1;
        var flagshipCategory = CreateCategory("Flagship", "flagship", displayOrder++);
        var midRangeCategory = CreateCategory("Tầm trung", "tam-trung", displayOrder++);
        var budgetCategory = CreateCategory("Giá rẻ", "gia-re", displayOrder++);
        var gamingCategory = CreateCategory("Gaming Phone", "gaming-phone", displayOrder++);
        var cameraCategory = CreateCategory("Camera Phone", "camera-phone", displayOrder++);
        var batteryCategory = CreateCategory("Pin khủng", "pin-khung", displayOrder++);
        var category5g = CreateCategory("Hỗ trợ 5G", "5g", displayOrder++);
        var foldCategory = CreateCategory("Điện thoại gập", "fold", displayOrder++);
        var compactCategory = CreateCategory("Nhỏ gọn", "nho-gon", displayOrder++);
        var newCategory = CreateCategory("Sản phẩm mới ra mắt", "moi-ra-mat", displayOrder++);
        var popularCategory = CreateCategory("Điện thoại phổ thông", "pho-thong", displayOrder++);
        var usedCategory = CreateCategory("Điện thoại cũ (Like New)", "like-new", displayOrder);
        dbContext.Categories.Add(flagshipCategory);
        dbContext.Categories.Add(midRangeCategory);
        dbContext.Categories.Add(budgetCategory);
        dbContext.Categories.Add(gamingCategory);
        dbContext.Categories.Add(cameraCategory);
        dbContext.Categories.Add(batteryCategory);
        dbContext.Categories.Add(category5g);
        dbContext.Categories.Add(foldCategory);
        dbContext.Categories.Add(compactCategory);
        dbContext.Categories.Add(newCategory);
        dbContext.Categories.Add(popularCategory);
        dbContext.Categories.Add(usedCategory);

        dbContext.Products.AddRange(new List<Product>
        {
            // --- APPLE IPHONE ---
            CreateProduct(
                "iPhone 15 Pro Max", "iphone-15-pro-max", "Vỏ Titan, Chip A17 Pro, Nút Action.",
                34_990_000,
                [flagshipCategory, cameraCategory, category5g, newCategory],
                appleBrand,
                ["Titan Tự Nhiên", "Titan Xanh", "Titan Đen", "Titan Trắng"],
                ["256GB", "512GB", "1TB"]
            ),
            CreateProduct(
                "iPhone 15 Pro", "iphone-15-pro", "Nhỏ gọn, mạnh mẽ, Titan Grade 5.",
                29_990_000,
                [flagshipCategory, cameraCategory, category5g, newCategory],
                appleBrand,
                ["Titan Tự Nhiên", "Titan Xanh"],
                ["128GB", "256GB", "512GB"]
            ),
            CreateProduct(
                "iPhone 15 Plus", "iphone-15-plus", "Màn hình lớn, Pin trâu nhất dòng iPhone.",
                24_990_000,
                [flagshipCategory, batteryCategory, category5g, newCategory],
                appleBrand,
                ["Hồng", "Vàng", "Xanh lá", "Đen"],
                ["128GB", "256GB"]
            ),
            CreateProduct(
                "iPhone 15", "iphone-15", "Dynamic Island, Camera 48MP, USB-C.",
                19_990_000,
                [flagshipCategory, cameraCategory, category5g, newCategory],
                appleBrand,
                ["Hồng", "Xanh Blue", "Đen"],
                ["128GB", "256GB"]
            ),
            CreateProduct(
                "iPhone 14 Pro Max", "iphone-14-pro-max", "Flagship đời trước, vẫn cực mạnh.",
                23_990_000,
                [flagshipCategory, cameraCategory, category5g],
                appleBrand,
                ["Tím Deep Purple", "Vàng Gold", "Đen"],
                ["128GB", "256GB", "512GB", "1TB"]
            ),
            CreateProduct(
                "iPhone 14", "iphone-14", "Thiết kế bền bỉ, pin ổn định.",
                14_990_000,
                [midRangeCategory, category5g],
                appleBrand,
                ["Tím", "Đỏ", "Xanh", "Trắng"],
                ["128GB", "256GB"]
            ),
            CreateProduct(
                "iPhone 13", "iphone-13", "Chiếc iPhone quốc dân, giá tốt nhất.",
                11_990_000,
                [midRangeCategory, category5g],
                appleBrand,
                ["Hồng", "Trắng", "Xanh Midnight"],
                ["64GB", "128GB"]
            ),
            CreateProduct(
                "iPhone 12", "iphone-12", "Thiết kế vuông vức, màn hình OLED.",
                9_990_000,
                [midRangeCategory, category5g],
                appleBrand,
                ["Tím", "Xanh Mint", "Đen"],
                ["64GB", "128GB"]
            ),
            CreateProduct(
                "iPhone SE 2022", "iphone-se-2022", "Nhỏ gọn, nút Home, Chip A15.",
                7_490_000,
                [budgetCategory, compactCategory, category5g],
                appleBrand,
                ["Đỏ", "Trắng", "Đen"],
                ["64GB", "128GB", "256GB"]
            ),

            // --- SAMSUNG GALAXY ---


            // --- XIAOMI ---
            // --- FLAGSHIP S SERIES (Thường cố định RAM, chỉ chọn Storage/Color) ---
            CreateProduct(
                "Samsung Galaxy S24 Ultra 5G", "samsung-galaxy-s24-ultra",
                "Quyền năng Galaxy AI, Khung viền Titan, Camera 200MP zoom 100x.",
                29_990_000,
                [flagshipCategory, cameraCategory, category5g, newCategory],
                samsungBrand,
                ["Xám Titan", "Đen Titan", "Tím Titan", "Vàng Titan"],
                ["256GB", "512GB", "1TB"]
            ),
            CreateProduct(
                "Samsung Galaxy S24+ 5G", "samsung-galaxy-s24-plus",
                "Màn hình QHD+ sắc nét, Galaxy AI, Pin 4900mAh.",
                23_990_000,
                [flagshipCategory, category5g, newCategory],
                samsungBrand,
                ["Đen Onyx", "Xám Marble", "Tím Cobalt", "Vàng Amber"],
                ["256GB", "512GB"]
            ),
            CreateProduct(
                "Samsung Galaxy S24 5G", "samsung-galaxy-s24",
                "Thiết kế nhỏ gọn, hiệu năng mạnh mẽ với Exynos 2400.",
                19_990_000,
                [flagshipCategory, compactCategory, category5g],
                samsungBrand,
                ["Đen Onyx", "Xám Marble", "Tím Cobalt", "Vàng Amber"],
                ["256GB", "512GB"]
            ),
            CreateProduct(
                "Samsung Galaxy S23 Ultra 5G", "samsung-galaxy-s23-ultra",
                "Siêu phẩm mắt thần bóng đêm, bút S-Pen quyền năng.",
                21_990_000,
                [flagshipCategory, cameraCategory, category5g],
                samsungBrand,
                ["Xanh Botanic", "Đen Phantom", "Tím Lilac", "Kem Cotton"],
                ["256GB", "512GB"]
            ),
            CreateProduct(
                "Samsung Galaxy S23 FE 5G", "samsung-galaxy-s23-fe",
                "Phiên bản Fan Edition, thiết kế cao cấp, hiệu năng ổn định.",
                11_890_000,
                [midRangeCategory, category5g],
                samsungBrand,
                ["Xanh Mint", "Trắng Cream", "Xám Graphite", "Tím Purple"],
                ["128GB", "256GB"]
            ),
            CreateProduct(
                "Samsung Galaxy Z Fold6 5G", "samsung-galaxy-z-fold6",
                "Gập mở quyền năng AI, thiết kế mỏng nhẹ nhất dòng Fold.",
                41_990_000,
                [foldCategory, flagshipCategory, category5g, newCategory],
                samsungBrand,
                ["Xám Metal", "Hồng Rosé", "Xanh Navy"],
                ["256GB", "512GB", "1TB"]
            ),
            CreateProduct(
                "Samsung Galaxy Z Flip6 5G", "samsung-galaxy-z-flip6",
                "Biểu tượng thời trang, Camera 50MP, FlexCam thông minh.",
                26_990_000,
                [foldCategory, compactCategory, flagshipCategory, newCategory],
                samsungBrand,
                ["Xanh Maya", "Vàng Solar", "Xám Metal", "Xanh Mint"],
                ["256GB", "512GB"]
            ),
            CreateProduct(
                "Samsung Galaxy Z Fold5 5G", "samsung-galaxy-z-fold5",
                "Bản lề Flex không kẽ hở, đa nhiệm cực đỉnh.",
                30_990_000,
                [foldCategory, flagshipCategory, category5g],
                samsungBrand,
                ["Xanh Icy", "Đen Phantom", "Kem Ivory"],
                ["256GB", "512GB"]
            ),
            CreateProduct(
                "Samsung Galaxy Z Flip5 5G", "samsung-galaxy-z-flip5",
                "Màn hình phụ Flex Window lớn 3.4 inch tiện lợi.",
                16_990_000,
                [foldCategory, compactCategory, midRangeCategory],
                samsungBrand,
                ["Xanh Mint", "Tím Fancy", "Kem Latte", "Xám Indie"],
                ["256GB", "512GB"]
            ),
            CreateProduct(
                "Samsung Galaxy A55 5G", "samsung-galaxy-a55",
                "Viền kim loại sang trọng, chụp đêm Nightography ấn tượng.",
                9_690_000,
                [midRangeCategory, category5g, cameraCategory],
                samsungBrand,
                ["Tím Lilac", "Xanh Iceblue", "Xanh Navy"],
                ["128GB", "256GB"],
                ["8GB", "12GB"]
            ),
            CreateProduct(
                "Samsung Galaxy A35 5G", "samsung-galaxy-a35",
                "Mặt lưng kính cao cấp, màn hình Super AMOLED 120Hz.",
                7_990_000,
                [midRangeCategory, category5g],
                samsungBrand,
                ["Xanh Iceblue", "Tím Lilac", "Xanh Navy"],
                ["128GB", "256GB"]
            ),
            CreateProduct(
                "Samsung Galaxy A25 5G", "samsung-galaxy-a25",
                "Màn hình Super AMOLED, Camera chống rung OIS.",
                6_290_000,
                [midRangeCategory, budgetCategory, category5g],
                samsungBrand,
                ["Vàng", "Xanh", "Đen", "Xanh Nhạt"],
                rams: ["6GB", "8GB"]
            ),
            CreateProduct(
                "Samsung Galaxy A15 5G", "samsung-galaxy-a15-5g",
                "Điện thoại 5G giá rẻ nhất, hiệu năng ổn định.",
                5_490_000,
                [budgetCategory, category5g],
                samsungBrand,
                ["Vàng", "Xanh", "Đen"]
            ),
            CreateProduct(
                "Samsung Galaxy A15 LTE", "samsung-galaxy-a15",
                "Màn hình Super AMOLED sống động, pin 5000mAh.",
                4_490_000,
                [budgetCategory, batteryCategory],
                samsungBrand,
                ["Vàng", "Xanh", "Đen"],
                rams: ["4GB", "6GB", "8GB"]
            ),
            CreateProduct(
                "Samsung Galaxy A05s", "samsung-galaxy-a05s",
                "Chip Snapdragon 680 mạnh mẽ trong tầm giá, màn hình 90Hz.",
                3_590_000,
                [budgetCategory, popularCategory],
                samsungBrand,
                ["Xanh", "Bạc", "Đen"],
                rams: ["4GB", "6GB"]
            ),
            CreateProduct(
                "Samsung Galaxy A05", "samsung-galaxy-a05",
                "Màn hình lớn 6.7 inch, sạc nhanh 25W.",
                2_890_000,
                [budgetCategory, popularCategory, batteryCategory],
                samsungBrand,
                ["Xanh", "Bạc", "Đen"],
                rams: ["4GB", "6GB"]
            ),
            CreateProduct(
                "Samsung Galaxy M54 5G", "samsung-galaxy-m54",
                "Mãnh thú pin 6000mAh, Camera 108MP chống rung OIS.",
                8_990_000,
                [midRangeCategory, batteryCategory, category5g],
                samsungBrand,
                ["Xanh Navy", "Bạc"]
            ),
            CreateProduct(
                "Samsung Galaxy M34 5G", "samsung-galaxy-m34",
                "Pin khủng 6000mAh dùng thả ga, màn hình 120Hz.",
                6_990_000,
                [budgetCategory, batteryCategory, category5g],
                samsungBrand,
                ["Xanh Midnight", "Xanh Waterfall"]
            ),

            // --- XIAOMI ---
            CreateProduct(
                "Xiaomi 14 Ultra", "xiaomi-14-ultra",
                "Ống kính Leica huyền thoại, cảm biến 1 inch, quay 8K.",
                29_990_000,
                [flagshipCategory, cameraCategory, category5g, newCategory],
                xiaomiBrand,
                ["Đen", "Trắng", "Xanh Dragon Crystal"],
                ["256GB", "512GB"]
            ),
            CreateProduct(
                "Xiaomi Redmi Note 13 Pro+ 5G", "xiaomi-redmi-note-13-pro-plus",
                "Camera 200MP, màn hình cong 1.5K, sạc 120W.",
                10_990_000,
                [midRangeCategory, category5g],
                xiaomiBrand,
                ["Đen Bán Dạ", "Trắng Ngọc", "Tím Cực Quang"],
                rams: ["8GB", "12GB"]
            ),
            CreateProduct(
                "Xiaomi Redmi A3", "xiaomi-redmi-a3",
                "Thiết kế lưng kính sang trọng trong tầm giá rẻ, màn hình 90Hz.",
                2_490_000,
                [budgetCategory, popularCategory, batteryCategory],
                xiaomiBrand,
                ["Xanh", "Đen", "Xanh Lá"],
                ["32GB", "64GB", "128GB"]
            ),

            // --- OPPO ---
            CreateProduct(
                "OPPO Find N3 5G", "oppo-find-n3",
                "Bậc thầy gập mở, Camera Hasselblad chuyên nghiệp.",
                41_990_000,
                [foldCategory, flagshipCategory, cameraCategory, category5g],
                oppoBrand,
                ["Vàng Cổ Điển", "Đen Kim Cương"],
                ["512GB", "1TB"]
            ),
            CreateProduct(
                "OPPO Reno11 F 5G", "oppo-reno11-f",
                "Chuyên gia chân dung, thiết kế mặt lưng vân đá tự nhiên.",
                8_990_000,
                [midRangeCategory, category5g, cameraCategory],
                oppoBrand,
                ["Xanh Dương", "Tím", "Xanh Đen"]
            ),
            CreateProduct(
                "OPPO A18", "oppo-a18",
                "Thiết kế rực rỡ, pin lớn 5000mAh, hoạt động mượt mà.",
                3_290_000,
                [budgetCategory, batteryCategory, popularCategory],
                oppoBrand,
                ["Xanh", "Đen"],
                ["64GB", "128GB"]
            ),

            // --- GOOGLE PIXEL ---
            CreateProduct(
                "Google Pixel 8 Pro", "google-pixel-8-pro",
                "Sức mạnh AI từ Google Tensor G3, camera chụp đêm đỉnh cao.",
                22_500_000,
                [flagshipCategory, cameraCategory, category5g, newCategory],
                googleBrand,
                ["Xanh Bay", "Đen Obsidian", "Kem Porcelain"],
                ["128GB", "256GB", "512GB"]
            ),
            CreateProduct(
                "Google Pixel 8", "google-pixel-8",
                "Nhỏ gọn, mạnh mẽ, cập nhật Android sớm nhất.",
                15_900_000,
                [flagshipCategory, compactCategory, category5g],
                googleBrand,
                ["Hồng Rose", "Xám Hazel", "Đen Obsidian"],
                ["128GB", "256GB"]
            ),
            CreateProduct(
                "Google Pixel 7a", "google-pixel-7a",
                "Vua tầm trung, camera xuất sắc nhất phân khúc.",
                9_500_000,
                [midRangeCategory, compactCategory, cameraCategory],
                googleBrand,
                ["Xanh Sea", "Cam Coral", "Trắng", "Đen"]
            ),

            // --- SONY ---
            CreateProduct(
                "Sony Xperia 1 V", "sony-xperia-1-v",
                "Cảm biến Exmor T mới, màn hình 4K OLED tỉ lệ 21:9.",
                29_990_000,
                [flagshipCategory, cameraCategory, category5g],
                sonyBrand,
                ["Đen", "Bạc Platinum", "Xanh Khaki"],
                ["256GB", "512GB"]
            ),
            CreateProduct(
                "Sony Xperia 5 V", "sony-xperia-5-v",
                "Flagship nhỏ gọn, pin trâu, âm thanh Hi-Res.",
                22_490_000,
                [flagshipCategory, compactCategory, category5g],
                sonyBrand,
                ["Đen", "Xanh", "Bạc"],
                ["128GB", "256GB"]
            ),
            CreateProduct(
                "Sony Xperia 10 V", "sony-xperia-10-v",
                "Siêu nhẹ 159g, pin 5000mAh dùng 2 ngày.",
                10_490_000,
                [midRangeCategory, batteryCategory, compactCategory],
                sonyBrand,
                ["Tím Lavender", "Xanh Sage", "Trắng", "Đen"]
            ),

            // --- NOKIA ---
            CreateProduct(
                "Nokia XR21 5G", "nokia-xr21",
                "Siêu bền chuẩn quân đội, chống nước IP69K, chịu va đập.",
                12_990_000,
                [midRangeCategory, category5g, batteryCategory],
                nokiaBrand,
                ["Đen Midnight", "Xanh Pine"]
            ),
            CreateProduct(
                "Nokia G42 5G", "nokia-g42",
                "Thiết kế QuickFix dễ sửa chữa, bảo hành 3 năm.",
                4_990_000,
                [budgetCategory, category5g],
                nokiaBrand,
                ["Tím So Purple", "Xám So Grey"],
                ["128GB", "256GB"]
            ),
            CreateProduct(
                "Nokia C32", "nokia-c32",
                "Mặt lưng kính cường lực, pin 3 ngày, giá siêu rẻ.",
                2_190_000,
                [budgetCategory, popularCategory, batteryCategory],
                nokiaBrand,
                ["Xanh Beach", "Hồng", "Đen Charcoal"],
                ["32GB", "64GB", "128GB"],
                ["3GB", "4GB"]
            ),
        });
        dbContext.SaveChanges();

        return;

        static Brand CreateBrand(string name, string slug, int displayOrder) =>
            new()
            {
                Name = name, Slug = slug, DisplayOrder = displayOrder,
                LogoUrl = "https://images.unsplash.com/photo-1670808439268-79d2cb00a46e"
            };

        static Category CreateCategory(string name, string slug, int displayOrder) =>
            new()
            {
                Name = name, Slug = slug, DisplayOrder = displayOrder,
                ImageUrl = "https://images.unsplash.com/photo-1685062428514-2164290b3322"
            };

        static Product CreateProduct(string name, string slug, string desc, long basePrice,
            ICollection<Category> categories, Brand brand, string[]? colors = null, string[]? storages = null,
            string[]? rams = null)
        {
            var product = new Product
            {
                Name = name,
                Slug = slug,
                Description = desc,
                IsActive = true,
                ProductImages =
                [
                    new ProductImage
                        { ImageUrl = "https://images.unsplash.com/photo-1571771894821-ce9b6c11b08e", DisplayOrder = 1 }
                ],
                MainImageUrl = "https://images.unsplash.com/photo-1571771894821-ce9b6c11b08e",
                Brand = brand,
                Categories = categories,
                ProductOptions = new List<ProductOption>(),
                ProductVariants = new List<ProductVariant>(),
                MinPrice = 0,
                MaxPrice = 0
            };

            var optColor = new ProductOption { Name = "Màu sắc" };
            if (colors != null)
            {
                optColor.ProductOptionValues =
                    colors.Select(color => new ProductOptionValue { Value = color }).ToList();
                product.ProductOptions.Add(optColor);
            }
            else
            {
                optColor.ProductOptionValues = [new ProductOptionValue { Value = "Null" }];
            }

            var optStorage = new ProductOption { Name = "Dung lượng" };
            if (storages != null)
            {
                optStorage.ProductOptionValues =
                    storages.Select(storage => new ProductOptionValue { Value = storage }).ToList();
                product.ProductOptions.Add(optStorage);
            }
            else
            {
                optStorage.ProductOptionValues = [new ProductOptionValue { Value = "Null" }];
            }

            var optRam = new ProductOption { Name = "RAM" };
            if (rams != null)
            {
                optRam.ProductOptionValues = rams.Select(ram => new ProductOptionValue { Value = ram }).ToList();
                product.ProductOptions.Add(optRam);
            }
            else
            {
                optRam.ProductOptionValues = [new ProductOptionValue { Value = "Null" }];
            }

            var rd = new Random();
            foreach (var colorVal in optColor.ProductOptionValues)
            {
                foreach (var storageVal in optStorage.ProductOptionValues)
                {
                    var storageSurcharge = storageVal.Value switch
                    {
                        "32GB" => 500_000,
                        "64GB" => 1_000_000,
                        "128GB" => 1_500_000,
                        "256GB" => 3_000_000,
                        "512GB" => 6_000_000,
                        "1TB" => 12_000_000,
                        _ => 0
                    };
                    foreach (var ramVal in optRam.ProductOptionValues)
                    {
                        var ramSurcharge = ramVal.Value switch
                        {
                            "4GB" => 500_000,
                            "6GB" => 1_000_000,
                            "8GB" => 2_000_000,
                            "12GB" => 4_000_000,
                            "16GB" => 7_000_000,
                            _ => 0
                        };
                        var variantOptions = new List<ProductOptionValue>();
                        if (colorVal.Value != "Null") variantOptions.Add(colorVal);
                        if (storageVal.Value != "Null") variantOptions.Add(storageVal);
                        if (ramVal.Value != "Null") variantOptions.Add(ramVal);

                        var finalPrice = basePrice + storageSurcharge + ramSurcharge;

                        var variant = new ProductVariant
                        {
                            Price = finalPrice,
                            AvailableStock = rd.Next(36, 169),
                            ImageUrl = product.MainImageUrl,
                            ProductOptionValues = variantOptions
                        };
                        product.ProductVariants.Add(variant);
                    }
                }
            }

            product.MinPrice = product.ProductVariants.Min(v => v.Price);
            product.MaxPrice = product.ProductVariants.Max(v => v.Price);

            return product;
        }
    }

    private static void SeedCouponsData(AppDbContext dbContext)
    {
        var fixedAmountCoupons = new List<Coupon>();
        var fixedAmountNoMinOrderAmountCoupons = new List<Coupon>();
        var percentageCoupons = new List<Coupon>();
        var percentageUnlimitedMaxDiscountAmountCoupons = new List<Coupon>();
        var percentageNoMinOrderAmountCoupons = new List<Coupon>();
        var percentageUnlimitedMaxDiscountAmountNoMinOrderAmountCoupons = new List<Coupon>();
        for (var i = 1; i <= 6; i++)
        {
            fixedAmountCoupons.Add(new Coupon
            {
                Description = $"Giảm {i * 50_000}đ cho đơn từ {i * 500_000}đ",
                DiscountType = DiscountType.FixedAmount,
                DiscountValue = i * 50_000,
                MaxDiscountAmount = i * 50_000,
                MinOrderAmount = i * 500_000,
                LoyaltyPointsCost = i * 300
            });
            fixedAmountNoMinOrderAmountCoupons.Add(new Coupon
            {
                Description = $"Giảm {i * 100_000}đ cho đơn hàng từ 0đ",
                DiscountType = DiscountType.FixedAmount,
                DiscountValue = i * 10_000,
                MaxDiscountAmount = i * 10_000,
                MinOrderAmount = null,
                LoyaltyPointsCost = i * 1000
            });
            percentageCoupons.Add(new Coupon
            {
                Description = $"Giảm {i * 5}% tối đa {i * 200_000}đ cho đơn từ {i * 2_000_000}đ",
                DiscountType = DiscountType.Percentage,
                DiscountValue = i * 5,
                MaxDiscountAmount = i * 200_000,
                MinOrderAmount = i * 2_000_000,
                LoyaltyPointsCost = i * 120,
            });
            percentageUnlimitedMaxDiscountAmountCoupons.Add(new Coupon
            {
                Description = $"Giảm {i * 5}% cho đơn từ {i * 1_000_000}đ",
                DiscountType = DiscountType.Percentage,
                DiscountValue = i * 5,
                MaxDiscountAmount = null,
                MinOrderAmount = i * 1_000_000,
                LoyaltyPointsCost = i * 150,
            });
            percentageNoMinOrderAmountCoupons.Add(new Coupon
            {
                Description = $"Giảm {i * 2}% tối đa {i * 100_000}đ cho đơn hàng từ 0đ",
                DiscountType = DiscountType.Percentage,
                DiscountValue = i * 2,
                MaxDiscountAmount = i * 100_000,
                MinOrderAmount = null,
                LoyaltyPointsCost = i * 50
            });
            percentageUnlimitedMaxDiscountAmountNoMinOrderAmountCoupons.Add(new Coupon
            {
                Description = $"Giảm {i * 2}% cho mọi đơn hàng",
                DiscountType = DiscountType.Percentage,
                DiscountValue = i * 2,
                MaxDiscountAmount = null,
                MinOrderAmount = null,
                LoyaltyPointsCost = i * 200
            });
        }

        dbContext.Coupons.AddRange(fixedAmountCoupons);
        dbContext.Coupons.AddRange(fixedAmountNoMinOrderAmountCoupons);
        dbContext.Coupons.AddRange(percentageCoupons);
        dbContext.Coupons.AddRange(percentageUnlimitedMaxDiscountAmountCoupons);
        dbContext.Coupons.AddRange(percentageNoMinOrderAmountCoupons);
        dbContext.Coupons.AddRange(percentageUnlimitedMaxDiscountAmountNoMinOrderAmountCoupons);
        dbContext.SaveChanges();
    }

    private static void SeedUserCouponData(AppDbContext dbContext)
    {
        var users = dbContext.Users.ToList();
        foreach (var user in users)
        {
            user.Coupons = dbContext.Coupons.Take(5).Select(c => new UserCoupon { Coupon = c }).ToList();
            user.LoyaltyPoints = 5000;
        }

        dbContext.Users.UpdateRange(users);
        dbContext.SaveChanges();
    }

    private static void SeedPaymentMethodsData(AppDbContext dbContext)
    {
        dbContext.PaymentMethods.AddRange(new List<PaymentMethod>
        {
            new()
            {
                Name = "Thanh toán khi nhận hàng (COD)",
                Code = PaymentMethodCode.COD,
                Description = "Thanh toán trực tiếp cho nhân viên giao hàng khi bạn nhận được sản phẩm.",
                IsActive = true
            },
            new()
            {
                Name = "Chuyển khoản bằng mã QR (payOS)",
                Code = PaymentMethodCode.PayOsQr,
                Description = "Quét mã QR payOS để thanh toán nhanh chóng và an toàn qua ứng dụng ngân hàng của bạn.",
                IsActive = true
            }
        });
        dbContext.SaveChanges();
    }
}