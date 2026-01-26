import axiosClient from "./axiosClient";
import type { ProductSummary } from "../types/products/product";
import type { PagedResult } from "../types/common/pageResult";
import { PRODUCT_SORT_COLUMN } from "../types/products/enums/productSortColumn";
import { SORT_DIRECTION } from "../types/common/enums/sortDirection";

// Define values for types based on the constant objects values
type ProductSortColumnType =
  (typeof PRODUCT_SORT_COLUMN)[keyof typeof PRODUCT_SORT_COLUMN];
type SortDirectionType = (typeof SORT_DIRECTION)[keyof typeof SORT_DIRECTION];

interface SearchProductsPagedSortedQuery {
  searchText?: string;
  pageIndex?: number;
  pageSize?: number;
  sortColumn?: ProductSortColumnType;
  sortDirection?: SortDirectionType;
}

export const productApi = {
  searchProductsPagedSorted: (query: SearchProductsPagedSortedQuery) =>
    axiosClient.get<PagedResult<ProductSummary>>("/products", {
      params: query,
    }),

  getProductsBySlug: (slug: string, query: SearchProductsPagedSortedQuery) =>
    axiosClient.get<PagedResult<ProductSummary>>(`/${slug}/products`, {
      params: query,
    }),
};
