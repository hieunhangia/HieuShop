import { PRODUCT_SORT_COLUMN } from "../enums/productSortColumn.ts";
import { SORT_DIRECTION } from "../../common/enums/sortDirection.ts";

type ProductSortColumnType =
    (typeof PRODUCT_SORT_COLUMN)[keyof typeof PRODUCT_SORT_COLUMN];
type SortDirectionType = (typeof SORT_DIRECTION)[keyof typeof SORT_DIRECTION];

export interface SearchProductsRequest {
    searchText?: string;
    pageIndex?: number;
    pageSize?: number;
    sortColumn?: ProductSortColumnType;
    sortDirection?: SortDirectionType;
}