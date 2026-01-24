import axiosClient from "./axiosClient";
import type { Brand } from "../types/brands/brand";

interface GetBrandsQuery {
  top?: number;
}

export const brandApi = {
  getBrands: (query: GetBrandsQuery) =>
    axiosClient.get<Brand[]>("/brands", { params: query }),
};
