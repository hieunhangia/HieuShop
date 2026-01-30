import type { Coupon } from "./Coupon";

export interface UserCoupon {
  id: string;
  coupon: Coupon;
  used: boolean;
  createdAt: Date;
}
