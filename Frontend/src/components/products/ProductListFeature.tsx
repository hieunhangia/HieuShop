import React from "react";
import {
  Search,
  Filter,
  ArrowUp,
  ArrowDown,
  ChevronLeft,
  ChevronRight,
  Loader,
} from "lucide-react";
import ProductCard from "./ProductCard";
import { type ProductSummary } from "../../types/products/dtos/ProductSummary";
import { PRODUCT_SORT_COLUMN } from "../../types/products/enums/productSortColumn";
import { SORT_DIRECTION } from "../../types/common/enums/sortDirection";

interface ProductListFeatureProps {
  title: string | React.ReactNode;
  products: ProductSummary[];
  isLoading: boolean;
  totalCount: number;
  filter: {
    localSearchText: string;
    pageIndex: number;
    pageSize: number;
    sortColumn: string;
    sortDirection: string;
  };
  handlers: {
    handleSearchChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    handleSearchSubmit: () => void;
    handleKeyDown: (e: React.KeyboardEvent<HTMLInputElement>) => void;
    handleSortChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
    toggleSortDirection: () => void;
    handlePageChange: (newPage: number) => void;
  };
}

export default function ProductListFeature({
  title,
  products,
  isLoading,
  totalCount,
  filter,
  handlers,
}: ProductListFeatureProps) {
  const { localSearchText, pageIndex, pageSize, sortColumn, sortDirection } =
    filter;

  const {
    handleSearchChange,
    handleSearchSubmit,
    handleKeyDown,
    handleSortChange,
    toggleSortDirection,
    handlePageChange,
  } = handlers;

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-12">
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-8">
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white">
          {title}
        </h1>

        {/* Filters */}
        <div className="flex flex-col sm:flex-row gap-3">
          <div className="relative flex gap-2">
            <div className="relative flex-1">
              <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <Search className="h-5 w-5 text-gray-400" />
              </div>
              <input
                type="text"
                placeholder="Tìm kiếm sản phẩm..."
                value={localSearchText}
                onChange={handleSearchChange}
                onKeyDown={handleKeyDown}
                className="pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-700 rounded-lg focus:ring-2 focus:ring-brand-500 focus:border-brand-500 bg-white dark:bg-gray-800 text-gray-900 dark:text-white w-full sm:w-64"
              />
            </div>
            <button
              onClick={handleSearchSubmit}
              className="px-4 py-2 bg-brand-600 hover:bg-brand-700 text-white font-medium rounded-lg transition-colors flex items-center gap-2"
            >
              Tìm kiếm
            </button>
          </div>

          <div className="flex items-center gap-2">
            <select
              value={sortColumn}
              onChange={handleSortChange}
              className="py-2 pl-3 pr-8 border border-gray-300 dark:border-gray-700 rounded-lg focus:ring-brand-500 focus:border-brand-500 bg-white dark:bg-gray-800 text-gray-900 dark:text-white h-[42px]"
            >
              <option value={PRODUCT_SORT_COLUMN.CREATED_AT}>Mới nhất</option>
              <option value={PRODUCT_SORT_COLUMN.PRICE}>Giá</option>
              <option value={PRODUCT_SORT_COLUMN.NAME}>Tên</option>
            </select>

            <button
              onClick={toggleSortDirection}
              className="p-2 border border-gray-300 dark:border-gray-700 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 bg-white dark:bg-gray-800 h-[42px] w-[42px] flex items-center justify-center transition-colors"
              title={
                sortDirection === SORT_DIRECTION.ASC ? "Tăng dần" : "Giảm dần"
              }
            >
              {sortDirection === SORT_DIRECTION.ASC ? (
                <ArrowUp className="h-5 w-5 text-gray-600 dark:text-gray-300" />
              ) : (
                <ArrowDown className="h-5 w-5 text-gray-600 dark:text-gray-300" />
              )}
            </button>
          </div>
        </div>
      </div>

      {isLoading ? (
        <div className="flex justify-center items-center h-64">
          <Loader className="w-10 h-10 text-brand-600 animate-spin" />
        </div>
      ) : products.length > 0 ? (
        <>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
            {products.map((product) => (
              <ProductCard key={product.id} product={product} />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="flex justify-center items-center gap-2 mt-8">
              <button
                onClick={() => handlePageChange(pageIndex - 1)}
                disabled={pageIndex <= 1}
                className="p-2 border border-gray-300 dark:border-gray-700 rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-50 dark:hover:bg-gray-700 bg-white dark:bg-gray-800 transition-colors"
              >
                <ChevronLeft className="h-5 w-5 text-gray-600 dark:text-gray-300" />
              </button>

              <div className="flex items-center gap-1">
                {Array.from({ length: totalPages }, (_, i) => i + 1)
                  .filter(
                    (page) =>
                      page === 1 ||
                      page === totalPages ||
                      (page >= pageIndex - 1 && page <= pageIndex + 1),
                  )
                  .map((page, index, array) => (
                    <React.Fragment key={page}>
                      {index > 0 && array[index - 1] !== page - 1 && (
                        <span className="px-2 text-gray-500">...</span>
                      )}
                      <button
                        onClick={() => handlePageChange(page)}
                        className={`w-10 h-10 rounded-lg flex items-center justify-center font-medium transition-colors ${
                          pageIndex === page
                            ? "bg-brand-600 text-white"
                            : "bg-white dark:bg-gray-800 border border-gray-300 dark:border-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700"
                        }`}
                      >
                        {page}
                      </button>
                    </React.Fragment>
                  ))}
              </div>

              <button
                onClick={() => handlePageChange(pageIndex + 1)}
                disabled={pageIndex >= totalPages}
                className="p-2 border border-gray-300 dark:border-gray-700 rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-50 dark:hover:bg-gray-700 bg-white dark:bg-gray-800 transition-colors"
              >
                <ChevronRight className="h-5 w-5 text-gray-600 dark:text-gray-300" />
              </button>
            </div>
          )}
        </>
      ) : (
        <div className="text-center py-12">
          <Filter className="mx-auto h-12 w-12 text-gray-400" />
          <h3 className="mt-2 text-sm font-semibold text-gray-900 dark:text-white">
            Không tìm thấy sản phẩm
          </h3>
          <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
            Hãy thử thay đổi bộ lọc hoặc từ khóa tìm kiếm của bạn.
          </p>
        </div>
      )}
    </div>
  );
}
