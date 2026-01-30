export const DISCOUNT_TYPE = {
  PERCENTAGE: "Percentage",
  FIXED_AMOUNT: "FixedAmount",
} as const;

export type DiscountType = (typeof DISCOUNT_TYPE)[keyof typeof DISCOUNT_TYPE];
