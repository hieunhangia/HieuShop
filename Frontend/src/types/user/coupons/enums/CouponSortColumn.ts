export const COUPON_SORT_COLUMN = {
  DISCOUNT_VALUE: "DiscountValue",
  MAX_DISCOUNT_AMOUNT: "MaxDiscountAmount",
  MIN_ORDER_AMOUNT: "MinOrderAmount",
  LOYALTY_POINTS_COST: "LoyaltyPointsCost",
} as const;

export type CouponSortColumn =
  (typeof COUPON_SORT_COLUMN)[keyof typeof COUPON_SORT_COLUMN];
