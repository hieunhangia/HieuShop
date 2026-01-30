export interface Coupon {
  id: string;
  description: string;
  discountType: string;
  discountValue: number;
  maxDiscountAmount: number | null;
  minOrderAmount: number | null;
  loyaltyPointsCost: number;
}
