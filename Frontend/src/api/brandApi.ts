import axiosClient from "./axiosClient";
import type { Brand } from "../types/brands/dtos/Brand";
import type { GetBrandsQuery } from "../types/brands/dtos/GetBrandsQuery";

export const brandApi = {
  getBrands: (query: GetBrandsQuery) =>
    axiosClient.get<Brand[]>("/brands", { params: query }),

  getBrandBySlug: (slug: string) => axiosClient.get<Brand>(`/brands/${slug}`),
};
