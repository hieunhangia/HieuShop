import type { PagedAndSortedRequest } from "../../../common/dtos/PagedAndSortedRequest";
import { DISCOUNT_TYPE } from "../enums/DiscountType";
import { COUPON_SORT_COLUMN } from "../enums/CouponSortColumn";

export interface SearchActiveCouponsQuery extends PagedAndSortedRequest {
  discountType?: (typeof DISCOUNT_TYPE)[keyof typeof DISCOUNT_TYPE];
  sortColumn?: (typeof COUPON_SORT_COLUMN)[keyof typeof COUPON_SORT_COLUMN];
}
