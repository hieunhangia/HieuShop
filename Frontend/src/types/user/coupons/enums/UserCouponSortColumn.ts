export const USER_COUPON_SORT_COLUMN = {
  CREATED_AT: "CreatedAt",
  DISCOUNT_VALUE: "DiscountValue",
  MAX_DISCOUNT_AMOUNT: "MaxDiscountAmount",
  MIN_ORDER_AMOUNT: "MinOrderAmount",
  USED: "Used",
} as const;

export type UserCouponSortColumn =
  (typeof USER_COUPON_SORT_COLUMN)[keyof typeof USER_COUPON_SORT_COLUMN];
