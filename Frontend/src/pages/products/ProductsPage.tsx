import React, { useEffect, useState } from "react";
import MainLayout from "../../layouts/MainLayout";
import { PAGES } from "../../config/page";
import { productApi } from "../../api/productApi";
import { type ProductSummary } from "../../types/products/product";
import { PRODUCT_SORT_COLUMN } from "../../types/products/enums/productSortColumn";
import { SORT_DIRECTION } from "../../types/common/enums/sortDirection";
import { useSearchParams, useParams } from "react-router-dom";
import {
  Search,
  Filter,
  ArrowUp,
  ArrowDown,
  ChevronLeft,
  ChevronRight,
  Loader,
} from "lucide-react";
import ProductCard from "../../components/products/ProductCard";

export default function ProductsPage() {
  const { slug } = useParams();
  const [searchParams, setSearchParams] = useSearchParams();

  // State for data
  const [products, setProducts] = useState<ProductSummary[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(true);

  // State for filters
  const searchText = searchParams.get("search") || "";
  const [localSearchText, setLocalSearchText] = useState(searchText);
  const pageIndex = parseInt(searchParams.get("page") || "1");
  const pageSize = 12; // Hardcoded for simplified UI
  const sortColumn =
    searchParams.get("sortCol") || PRODUCT_SORT_COLUMN.CREATED_AT;
  const sortDirection = searchParams.get("sortDir") || SORT_DIRECTION.DESC;

  useEffect(() => {
    setLocalSearchText(searchText);
  }, [searchText]);

  useEffect(() => {
    document.title = `${slug ? PAGES.PRODUCTS.BY_SLUG.TITLE : PAGES.PRODUCTS.ALL.TITLE} | HieuShop`;
  }, [slug]);

  useEffect(() => {
    const fetchProducts = async () => {
      setIsLoading(true);
      try {
        let response;
        if (slug) {
          response = await productApi.getProductsBySlug(slug, {
            searchText,
            pageIndex,
            pageSize,
            sortColumn: sortColumn as any,
            sortDirection: sortDirection as any,
          });
        } else {
          response = await productApi.searchProductsPagedSorted({
            searchText,
            pageIndex,
            pageSize,
            sortColumn: sortColumn as any,
            sortDirection: sortDirection as any,
          });
        }

        setProducts(response.data.items);
        setTotalCount(response.data.totalCount);
      } catch (error) {
        console.error("Failed to fetch products", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchProducts();
  }, [slug, searchText, pageIndex, sortColumn, sortDirection]);

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setLocalSearchText(e.target.value);
  };

  const handleSearchSubmit = () => {
    const newParams = new URLSearchParams(searchParams);
    if (localSearchText) {
      newParams.set("search", localSearchText);
    } else {
      newParams.delete("search");
    }
    newParams.set("page", "1"); // Reset to first page
    setSearchParams(newParams);
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      handleSearchSubmit();
    }
  };

  const handleSortChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newSortCol = e.target.value;
    const newParams = new URLSearchParams(searchParams);
    newParams.set("sortCol", newSortCol);
    newParams.set("page", "1");
    setSearchParams(newParams);
  };

  const toggleSortDirection = () => {
    const newDir =
      sortDirection === SORT_DIRECTION.ASC
        ? SORT_DIRECTION.DESC
        : SORT_DIRECTION.ASC;
    const newParams = new URLSearchParams(searchParams);
    newParams.set("sortDir", newDir);
    setSearchParams(newParams);
  };

  const handlePageChange = (newPage: number) => {
    const newParams = new URLSearchParams(searchParams);
    newParams.set("page", newPage.toString());
    setSearchParams(newParams);
    window.scrollTo(0, 0);
  };

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <MainLayout>
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-12">
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-8">
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">
            Sản phẩm
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
    </MainLayout>
  );
}
