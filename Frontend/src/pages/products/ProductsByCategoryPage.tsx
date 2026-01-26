import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import MainLayout from "../../layouts/MainLayout";
import { productApi } from "../../api/productApi";
import { categoryApi } from "../../api/categoryApi";
import { type Category } from "../../types/categories/category";
import { type ProductSummary } from "../../types/products/product";
import { useProductFilter } from "../../hooks/useProductFilter";
import ProductListFeature from "../../components/products/ProductListFeature";

export default function ProductsByCategoryPage() {
  const { slug } = useParams<{ slug: string }>();
  const { filter, handlers } = useProductFilter();
  const { searchText, pageIndex, pageSize, sortColumn, sortDirection } = filter;

  const [category, setCategory] = useState<Category | null>(null);
  const [products, setProducts] = useState<ProductSummary[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchCategory = async () => {
      if (!slug) return;
      try {
        const response = await categoryApi.getCategoryBySlug(slug);
        setCategory(response.data);
        document.title = `${response.data.name} | HieuShop`;
      } catch (error) {
        console.error("Failed to fetch category", error);
      }
    };
    fetchCategory();
  }, [slug]);

  useEffect(() => {
    const fetchProducts = async () => {
      if (!slug) return;
      setIsLoading(true);
      try {
        const response = await productApi.searchProductsByCategory(slug, {
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
          category ? (
            <span className="flex items-center gap-4">
              <span>Danh mục: {category.name}</span>
              {category.imageUrl && (
                <img
                  src={category.imageUrl}
                  alt={category.name}
                  className="h-10 w-10 object-cover rounded-full bg-white border border-gray-100 shadow-sm inline-block"
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
