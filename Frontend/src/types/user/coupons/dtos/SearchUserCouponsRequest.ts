import type { PagedAndSortedRequest } from "../../../common/dtos/PagedAndSortedRequest";
import type { DiscountType } from "../enums/DiscountType";
import type { UserCouponSortColumn } from "../enums/UserCouponSortColumn";

export interface SearchUserCouponsRequest extends PagedAndSortedRequest {
  discountType?: DiscountType;
  sortColumn?: UserCouponSortColumn;
}
