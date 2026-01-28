import axiosClient from "./axiosClient";
import type { Province } from "../types/address/dtos/Province";
import type { Ward } from "../types/address/dtos/Ward";

const addressApi = {
  getProvinces: async (): Promise<Province[]> => {
    const response = await axiosClient.get("/provinces");
    return response.data;
  },
  getWardsByProvinceId: async (provinceId: number): Promise<Ward[]> => {
    const response = await axiosClient.get(`/provinces/${provinceId}/wards`);
    return response.data;
  },
};

export default addressApi;
