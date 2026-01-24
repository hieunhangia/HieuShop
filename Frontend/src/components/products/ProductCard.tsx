import { Link } from 'react-router-dom';
import type { ProductSummary } from '../../types/products/product';
import { PAGES } from '../../config/page';

interface ProductCardProps {
    product: ProductSummary;
}

export default function ProductCard({ product }: ProductCardProps) {
    const hasDiscount = !!product.salePrice && product.salePrice < product.price;
    const discountPercentage = hasDiscount
        ? Math.round(((product.price - product.salePrice!) / product.price) * 100)
        : 0;

    return (
        <div className="group relative bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-200 dark:border-gray-800 overflow-hidden hover:shadow-md transition-shadow">
            <div className="aspect-[1/1] w-full overflow-hidden bg-gray-200 lg:aspect-none group-hover:opacity-75 relative">
                <img
                    src={product.imageUrl}
                    alt={product.name}
                    className="h-full w-full object-cover object-center lg:h-full lg:w-full"
                />
                
                {/* Discount Badge */}
                {hasDiscount && (
                    <div className="absolute top-2 left-2 bg-red-600 text-white text-xs font-bold px-2 py-1 rounded-md shadow-sm z-10">
                        -{discountPercentage}%
                    </div>
                )}
            </div>
            <div className="p-4">
                <h3 className="text-base font-semibold text-gray-900 dark:text-white truncate">
                    <Link to={`${PAGES.PRODUCTS.PATH}/${product.slug}`}>
                        <span aria-hidden="true" className="absolute inset-0" />
                        {product.name}
                    </Link>
                </h3>
                <div className="mt-2 flex items-center gap-2">
                    <p className="text-lg font-bold text-brand-600 dark:text-brand-400">
                        {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(product.salePrice || product.price)}
                    </p>
                    {hasDiscount && (
                        <p className="text-sm font-medium text-gray-500 line-through">
                             {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(product.price)}
                        </p>
                    )}
                </div>
            </div>
        </div>
    );
}
