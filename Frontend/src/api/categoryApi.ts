import axiosClient from "./axiosClient";
import type { Category } from "../types/categories/category";

interface GetCategoriesQuery {
  top?: number;
}

export const categoryApi = {
  getCategories: (query: GetCategoriesQuery) =>
    axiosClient.get<Category[]>("/categories", { params: query }),

  getCategoryBySlug: (slug: string) =>
    axiosClient.get<Category>(`/categories/${slug}`),
};
