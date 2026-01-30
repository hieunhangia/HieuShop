import { SORT_DIRECTION } from "../enums/sortDirection";

export interface PagedAndSortedRequest {
  pageIndex?: number;
  pageSize?: number;
  sortColumn?: any; // Generic sort column or string
  sortDirection?: (typeof SORT_DIRECTION)[keyof typeof SORT_DIRECTION];
  searchText?: string;
}
