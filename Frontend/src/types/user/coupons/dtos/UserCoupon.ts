import type { Coupon } from "./Coupon.ts";

export interface UserCoupon {
  id: string;
  coupon: Coupon;
  used: boolean;
}
