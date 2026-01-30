import axiosClient from "./axiosClient";
import type { PagedAndSortedResult } from "../types/common/dtos/PagedAndSortedResult";
import type { SearchActiveCouponsQuery } from "../types/user/coupons/dtos/SearchActiveCouponsQuery";
import type { Coupon } from "../types/user/coupons/dtos/Coupon";
import type { UserCoupon } from "../types/user/coupons/dtos/UserCoupon";
import type { SearchUserCouponsRequest } from "../types/user/coupons/dtos/SearchUserCouponsRequest";

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

  getUserCoupons: async (
    query: SearchUserCouponsRequest,
  ): Promise<PagedAndSortedResult<UserCoupon>> => {
    const response = await axiosClient.get("/users/coupons", { params: query });
    return response.data;
  },

  purchaseCoupon: (couponId: string): Promise<void> => {
    return axiosClient.post(`/coupons/${couponId}/purchase`);
  },
};

export default couponApi;
