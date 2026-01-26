import axiosClient from "./axiosClient";
import type { ProductSummary } from "../types/products/product";
import type { ProductDetail } from "../types/products/productDetail";
import type { PagedResult } from "../types/common/pageResult";
import { PRODUCT_SORT_COLUMN } from "../types/products/enums/productSortColumn";
import { SORT_DIRECTION } from "../types/common/enums/sortDirection";

// Define values for types based on the constant objects values
type ProductSortColumnType =
  (typeof PRODUCT_SORT_COLUMN)[keyof typeof PRODUCT_SORT_COLUMN];
type SortDirectionType = (typeof SORT_DIRECTION)[keyof typeof SORT_DIRECTION];

interface SearchProductsQuery {
  searchText?: string;
  pageIndex?: number;
  pageSize?: number;
  sortColumn?: ProductSortColumnType;
  sortDirection?: SortDirectionType;
}

export const productApi = {
  searchProducts: (query: SearchProductsQuery) =>
    axiosClient.get<PagedResult<ProductSummary>>("/products", {
      params: query,
    }),

  searchProductsByBrand: (brandSlug: string, query: SearchProductsQuery) =>
    axiosClient.get<PagedResult<ProductSummary>>(
      `/brands/${brandSlug}/products`,
      { params: query },
    ),

  searchProductsByCategory: (
    categorySlug: string,
    query: SearchProductsQuery,
  ) =>
    axiosClient.get<PagedResult<ProductSummary>>(
      `/categories/${categorySlug}/products`,
      { params: query },
    ),

  getProductBySlug: async (slug: string): Promise<ProductDetail> => {
    const response = await axiosClient.get(`/products/${slug}`);
    return response.data;
  },
};
