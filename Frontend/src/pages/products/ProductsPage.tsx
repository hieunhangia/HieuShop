import { useEffect, useState } from "react";
import MainLayout from "../../layouts/MainLayout";
import { PAGES } from "../../config/page";
import { productApi } from "../../api/productApi";
import { type ProductSummary } from "../../types/products/dtos/ProductSummary";
import { useProductFilter } from "../../hooks/useProductFilter";
import ProductListFeature from "../../components/products/ProductListFeature";

export default function ProductsPage() {
  const { filter, handlers } = useProductFilter();
  const { searchText, pageIndex, pageSize, sortColumn, sortDirection } = filter;

  // State for data
  const [products, setProducts] = useState<ProductSummary[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    document.title = `${PAGES.PRODUCTS.ALL.TITLE} | HieuShop`;
  }, []);

  useEffect(() => {
    const fetchProducts = async () => {
      setIsLoading(true);
      try {
        const response = await productApi.searchProducts({
          searchText,
          pageIndex,
          pageSize,
          sortColumn: sortColumn as any,
          sortDirection: sortDirection as any,
        });

        setProducts(response.data.items);
        setTotalCount(response.data.totalCount);
      } catch (error) {
        console.error("Failed to fetch products", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchProducts();
  }, [searchText, pageIndex, sortColumn, sortDirection]);

  return (
    <MainLayout>
      <ProductListFeature
        title="Sản phẩm"
        products={products}
        isLoading={isLoading}
        totalCount={totalCount}
        filter={filter}
        handlers={handlers}
      />
    </MainLayout>
  );
}
