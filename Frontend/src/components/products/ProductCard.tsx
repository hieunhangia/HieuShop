import { Link } from "react-router-dom";
import type { ProductSummary } from "../../types/products/dtos/ProductSummary";
import { PAGES } from "../../config/page";

interface ProductCardProps {
  product: ProductSummary;
}

export default function ProductCard({ product }: ProductCardProps) {
  return (
    <div className="group relative bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-200 dark:border-gray-800 overflow-hidden hover:shadow-md transition-shadow">
      <div className="aspect-[1/1] w-full overflow-hidden bg-gray-200 lg:aspect-none group-hover:opacity-75 relative">
        <img
          src={product.mainImageUrl}
          alt={product.name}
          className="h-full w-full object-cover object-center lg:h-full lg:w-full"
        />
      </div>
      <div className="p-4">
        <h3 className="text-base font-semibold text-gray-900 dark:text-white truncate">
          <Link
            to={`${PAGES.PRODUCTS.DETAIL.PATH.replace(":slug", product.slug)}`}
          >
            <span aria-hidden="true" className="absolute inset-0" />
            {product.name}
          </Link>
        </h3>
        <div className="mt-2 flex items-center gap-2">
          <p className="text-lg font-bold text-brand-600 dark:text-brand-400">
            {product.minPrice === product.maxPrice
              ? new Intl.NumberFormat("vi-VN", {
                  style: "currency",
                  currency: "VND",
                }).format(product.minPrice)
              : `${new Intl.NumberFormat("vi-VN", {
                  style: "currency",
                  currency: "VND",
                }).format(product.minPrice)} - ${new Intl.NumberFormat(
                  "vi-VN",
                  {
                    style: "currency",
                    currency: "VND",
                  },
                ).format(product.maxPrice)}`}
          </p>
        </div>
      </div>
    </div>
  );
}
