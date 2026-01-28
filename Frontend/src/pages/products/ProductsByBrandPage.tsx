import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import MainLayout from "../../layouts/MainLayout";
import { productApi } from "../../api/productApi";
import { brandApi } from "../../api/brandApi";
import { type Brand } from "../../types/brands/dtos/Brand";
import { type ProductSummary } from "../../types/products/dtos/ProductSummary";
import { useProductFilter } from "../../hooks/useProductFilter";
import ProductListFeature from "../../components/products/ProductListFeature";

export default function ProductsByBrandPage() {
  const { slug } = useParams<{ slug: string }>();
  const { filter, handlers } = useProductFilter();
  const { searchText, pageIndex, pageSize, sortColumn, sortDirection } = filter;

  const [brand, setBrand] = useState<Brand | null>(null);
  const [products, setProducts] = useState<ProductSummary[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchBrand = async () => {
      if (!slug) return;
      try {
        const response = await brandApi.getBrandBySlug(slug);
        setBrand(response.data);
        document.title = `${response.data.name} | HieuShop`;
      } catch (error) {
        console.error("Failed to fetch brand", error);
      }
    };
    fetchBrand();
  }, [slug]);

  useEffect(() => {
    const fetchProducts = async () => {
      if (!slug) return;
      setIsLoading(true);
      try {
        const response = await productApi.searchProductsByBrand(slug, {
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
  }, [slug, searchText, pageIndex, sortColumn, sortDirection]);

  if (!slug) return null;

  return (
    <MainLayout>
      <ProductListFeature
        title={
          brand ? (
            <span className="flex items-center gap-4">
              <span>Thương hiệu: {brand.name}</span>
              {brand.logoUrl && (
                <img
                  src={brand.logoUrl}
                  alt={brand.name}
                  className="h-10 w-10 object-contain rounded-full bg-white border border-gray-100 p-1 shadow-sm inline-block"
                />
              )}
            </span>
          ) : (
            "Sản phẩm"
          )
        }
        products={products}
        isLoading={isLoading}
        totalCount={totalCount}
        filter={filter}
        handlers={handlers}
      />
    </MainLayout>
  );
}
