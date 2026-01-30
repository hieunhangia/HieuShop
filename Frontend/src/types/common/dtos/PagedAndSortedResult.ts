import type { PagedResult } from "./PagedResult";

export interface PagedAndSortedResult<T> extends PagedResult<T> {
  totalPages: number;
}
