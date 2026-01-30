import axiosClient from "./axiosClient";
import type { PagedAndSortedResult } from "../types/common/dtos/PagedAndSortedResult";
import type { SearchActiveCouponsQuery } from "../types/user/coupons/dtos/SearchActiveCouponsQuery";
import type { Coupon } from "../types/user/coupons/dtos/Coupon.ts";
import type { UserCoupon } from "../types/user/coupons/dtos/UserCoupon.ts";

const couponApi = {
  getActiveCoupons: async (
    query: SearchActiveCouponsQuery,
  ): Promise<PagedAndSortedResult<Coupon>> => {
    const response = await axiosClient.get("/coupons", { params: query });
    return response.data;
  },

  getLoyaltyPoints: async () => {
    const response = await axiosClient.get<number>("/loyalty-points");
    return response.data;
  },

  getUserCoupons: async () => {
    const response = await axiosClient.get<UserCoupon[]>("/users/coupons");
    return response.data;
  },

  purchaseCoupon: (couponId: string): Promise<void> => {
    return axiosClient.post(`/coupons/${couponId}/purchase`);
  },
};

export default couponApi;
