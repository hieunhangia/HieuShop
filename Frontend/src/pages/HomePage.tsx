import { useEffect, useState } from "react";
import MainLayout from "../layouts/MainLayout";
import { PAGES } from "../config/page";
import { brandApi } from "../api/brandApi";
import { categoryApi } from "../api/categoryApi";
import { productApi } from "../api/productApi";
import { type Brand } from "../types/brands/dtos/Brand";
import { type Category } from "../types/categories/dtos/Category";
import { type ProductSummary } from "../types/products/dtos/ProductSummary";
import { Link } from "react-router-dom";
import { ArrowRight, ShoppingBag, Tag, Layers } from "lucide-react";
import { PRODUCT_SORT_COLUMN } from "../types/products/enums/productSortColumn";
import { SORT_DIRECTION } from "../types/common/enums/sortDirection";
import backgroundShop from "../assets/background_shop.webp";
import ProductCard from "../components/products/ProductCard";

export default function HomePage() {
  const [brands, setBrands] = useState<Brand[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [newArrivals, setNewArrivals] = useState<ProductSummary[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    document.title = `${PAGES.HOME.TITLE} | HieuShop`;

    const fetchData = async () => {
      try {
        const [brandsRes, categoriesRes, productsRes] = await Promise.all([
          brandApi.getBrands({ top: 5 }),
          categoryApi.getCategories({ top: 5 }),
          productApi.searchProducts({
            pageSize: 4,
            sortColumn: PRODUCT_SORT_COLUMN.CREATED_AT,
            sortDirection: SORT_DIRECTION.DESC,
          }),
        ]);

        setBrands(brandsRes.data);
        setCategories(categoriesRes.data);
        setNewArrivals(productsRes.data.items);
      } catch (error) {
        console.error("Failed to fetch home data", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, []);

  return (
    <MainLayout>
      <section className="relative isolate px-4 sm:px-6 lg:px-8 py-20 lg:py-32 overflow-hidden">
        <div className="absolute inset-0">
          <img
            src={backgroundShop}
            alt="Background"
            className="w-full h-full object-cover object-center"
          />
          <div className="absolute inset-0 bg-white/50 dark:bg-gray-900/50" />
        </div>

        <div className="relative z-10 w-fit mx-auto text-center bg-white/20 dark:bg-black/20 backdrop-blur-sm rounded-3xl p-6 sm:p-8 shadow-2xl ring-1 ring-gray-900/5 inset-0">
          <h1 className="text-4xl md:text-6xl font-extrabold tracking-tight text-gray-900 dark:text-white mb-6">
            HieuShop
          </h1>
          <p className="mt-4 text-xl text-gray-600 dark:text-gray-300 max-w-2xl mx-auto">
            Khám phá những sản phẩm công nghệ đỉnh cao, chính hãng với mức giá
            tốt nhất thị trường. Trải nghiệm mua sắm thông minh ngay hôm nay.
          </p>
        </div>
      </section>

      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-12 space-y-16">
        {/* Categories Section */}
        <section>
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
              <Layers className="text-brand-600" /> Danh mục hàng đầu
            </h2>
          </div>
          {isLoading ? (
            <div className="animate-pulse h-32 bg-gray-200 dark:bg-gray-800 rounded-lg"></div>
          ) : (
            <div className="grid grid-cols-2 md:grid-cols-5 gap-4">
              {categories.map((category) => (
                <Link
                  to={PAGES.CATEGORIES.PRODUCTS.PATH.replace(
                    ":slug",
                    category.slug,
                  )}
                  key={category.id}
                  className="group relative overflow-hidden rounded-lg bg-gray-100 dark:bg-gray-800 aspect-[4/3] flex items-center justify-center p-4 hover:shadow-lg transition-all"
                >
                  <img
                    src={category.imageUrl}
                    alt={category.name}
                    className="absolute inset-0 w-full h-full object-cover opacity-60 group-hover:opacity-40 transition-opacity"
                  />
                  <span className="relative z-10 font-medium text-gray-900 dark:text-gray-100 text-center text-lg">
                    {category.name}
                  </span>
                </Link>
              ))}
            </div>
          )}
        </section>

        {/* Brands Section */}
        <section>
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
              <Tag className="text-brand-600" /> Thương hiệu nổi bật
            </h2>
          </div>
          {isLoading ? (
            <div className="animate-pulse h-24 bg-gray-200 dark:bg-gray-800 rounded-lg"></div>
          ) : (
            <div className="flex flex-wrap gap-4 justify-center md:justify-between items-center py-4">
              {brands.map((brand) => (
                <Link
                  to={PAGES.BRANDS.PRODUCTS.PATH.replace(":slug", brand.slug)}
                  key={brand.id}
                  className="grayscale hover:grayscale-0 transition-all duration-300 opacity-60 hover:opacity-100"
                >
                  <img
                    src={brand.logoUrl}
                    alt={brand.name}
                    className="h-12 object-contain"
                    title={brand.name}
                  />
                </Link>
              ))}
            </div>
          )}
        </section>

        {/* New Arrivals (Products) Section */}
        <section>
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
              <ShoppingBag className="text-brand-600" /> Sản phẩm mới
            </h2>
            <Link
              to={PAGES.PRODUCTS.ALL.PATH}
              className="text-brand-600 hover:text-brand-500 font-medium flex items-center gap-1"
            >
              Xem tất cả <ArrowRight className="w-4 h-4" />
            </Link>
          </div>
          {isLoading ? (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
              {[1, 2, 3, 4].map((i) => (
                <div
                  key={i}
                  className="animate-pulse bg-gray-200 dark:bg-gray-800 rounded-lg h-80"
                ></div>
              ))}
            </div>
          ) : (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
              {newArrivals.map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>
          )}
        </section>
      </div>
    </MainLayout>
  );
}
