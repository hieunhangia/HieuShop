import axiosClient from "./axiosClient";
import type { ProductSummary } from "../types/products/dtos/ProductSummary";
import type { ProductDetail } from "../types/products/dtos/ProductDetail";
import type { PagedResult } from "../types/common/dtos/PagedResult";
import type { SearchProductsRequest } from "../types/products/dtos/SearchProductsRequest.ts";

export const productApi = {
  searchProducts: (query: SearchProductsRequest) =>
    axiosClient.get<PagedResult<ProductSummary>>("/products", {
      params: query,
    }),

  searchProductsByBrand: (brandSlug: string, query: SearchProductsRequest) =>
    axiosClient.get<PagedResult<ProductSummary>>(
      `/brands/${brandSlug}/products`,
      { params: query },
    ),

  searchProductsByCategory: (
    categorySlug: string,
    query: SearchProductsRequest,
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
