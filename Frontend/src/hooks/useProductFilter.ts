import { useState, useEffect } from "react";
import { useSearchParams } from "react-router-dom";
import { PRODUCT_SORT_COLUMN } from "../types/products/enums/productSortColumn";
import { SORT_DIRECTION } from "../types/common/enums/sortDirection";

export const useProductFilter = () => {
  const [searchParams, setSearchParams] = useSearchParams();

  // State for filters
  const searchText = searchParams.get("search") || "";
  const pageIndex = parseInt(searchParams.get("page") || "1");
  const pageSize = 12; // Hardcoded for simplified UI
  const sortColumn =
    (searchParams.get("sortCol") as string) || PRODUCT_SORT_COLUMN.CREATED_AT;
  const sortDirection =
    (searchParams.get("sortDir") as string) || SORT_DIRECTION.DESC;

  // Local state for search input to avoid debounce lag vs url update
  const [localSearchText, setLocalSearchText] = useState(searchText);

  useEffect(() => {
    setLocalSearchText(searchText);
  }, [searchText]);

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setLocalSearchText(e.target.value);
  };

  const handleSearchSubmit = () => {
    const newParams = new URLSearchParams(searchParams);
    if (localSearchText) {
      newParams.set("search", localSearchText);
    } else {
      newParams.delete("search");
    }
    newParams.set("page", "1"); // Reset to first page
    setSearchParams(newParams);
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      handleSearchSubmit();
    }
  };

  const handleSortChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newSortCol = e.target.value;
    const newParams = new URLSearchParams(searchParams);
    newParams.set("sortCol", newSortCol);
    newParams.set("page", "1");
    setSearchParams(newParams);
  };

  const toggleSortDirection = () => {
    const newDir =
      sortDirection === SORT_DIRECTION.ASC
        ? SORT_DIRECTION.DESC
        : SORT_DIRECTION.ASC;
    const newParams = new URLSearchParams(searchParams);
    newParams.set("sortDir", newDir);
    newParams.set("page", "1");
    setSearchParams(newParams);
  };

  const handlePageChange = (newPage: number) => {
    const newParams = new URLSearchParams(searchParams);
    newParams.set("page", newPage.toString());
    setSearchParams(newParams);
    window.scrollTo(0, 0);
  };

  return {
    filter: {
      searchText,
      localSearchText,
      pageIndex,
      pageSize,
      sortColumn,
      sortDirection,
    },
    handlers: {
      handleSearchChange,
      handleSearchSubmit,
      handleKeyDown,
      handleSortChange,
      toggleSortDirection,
      handlePageChange,
    },
  };
};
