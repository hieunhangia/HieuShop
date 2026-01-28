import axiosClient from "./axiosClient";
import type { Category } from "../types/categories/dtos/Category";
import type { GetCategoriesQuery } from "../types/categories/dtos/GetCategoriesQuery";

export const categoryApi = {
  getCategories: (query: GetCategoriesQuery) =>
    axiosClient.get<Category[]>("/categories", { params: query }),

  getCategoryBySlug: (slug: string) =>
    axiosClient.get<Category>(`/categories/${slug}`),
};
