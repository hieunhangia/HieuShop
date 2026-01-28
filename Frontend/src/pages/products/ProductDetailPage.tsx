import { useEffect, useMemo, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { productApi } from "../../api/productApi";
import { cartApi } from "../../api/cartApi";
import { useCart } from "../../context/CartContext";
import { useAuth } from "../../context/AuthContext";
import { PAGES } from "../../config/page";
import toast from "react-hot-toast";
import type { ProductDetail } from "../../types/products/dtos/ProductDetail";
import MainLayout from "../../layouts/MainLayout";
import ProductGallery from "../../components/products/ProductGallery";
import ProductInfo from "../../components/products/ProductInfo";
import ProductOptions from "../../components/products/ProductOptions";
import ProductDescription from "../../components/products/ProductDescription";
import ProductHeader from "../../components/products/ProductHeader";
import { Loader2, AlertCircle } from "lucide-react";
import { parseApiError } from "../../utils/error";

const ProductDetailPage = () => {
  const { slug } = useParams<{ slug: string }>();
  const [product, setProduct] = useState<ProductDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // State for selected options: Record<OptionID, ValueID>
  const [selectedOptions, setSelectedOptions] = useState<
    Record<string, string>
  >({});

  const { refreshCartCount } = useCart();
  const { user } = useAuth();

  useEffect(() => {
    const fetchProduct = async () => {
      if (!slug) return;
      setLoading(true);
      setError(null);
      try {
        const data = await productApi.getProductBySlug(slug);
        setProduct(data);
        document.title = `${data.name} | HieuShop`;

        // Requirement: Default NO options selected
        setSelectedOptions({});
      } catch (err: any) {
        console.error(err);
        if (err.response && err.response.status === 404) {
          setError("Sản phẩm không tồn tại.");
        } else {
          setError("Có lỗi xảy ra khi tải sản phẩm.");
        }
      } finally {
        setLoading(false);
      }
    };

    fetchProduct();
  }, [slug]);

  const handleOptionSelect = (optionId: string, valueId: string) => {
    setSelectedOptions((prev) => {
      const next = { ...prev, [optionId]: valueId };

      // Optional: If user un-selects? No, logic is always select.
      // But if toggling logic is needed:
      if (prev[optionId] === valueId) {
        const { [optionId]: removed, ...rest } = prev;
        return rest;
      }
      return next;
    });
  };

  // Find the currently selected variant based on selectedOptions
  const selectedVariant = useMemo(() => {
    if (!product) return null;

    // Only return a variant if ALL options are selected
    const allOptionsSelected = product.productOptions.every(
      (opt) => selectedOptions[opt.id],
    );
    if (!allOptionsSelected) return null;

    return product.productVariants.find((variant) =>
      variant.productOptionValues.every((val) => {
        const option = product.productOptions.find((opt) =>
          opt.productOptionValues.some((v) => v.id === val.id),
        );
        return option && selectedOptions[option.id] === val.id;
      }),
    );
  }, [product, selectedOptions]);

  // Logic for Price Display
  const priceDisplay = useMemo(() => {
    if (!product) return "";

    // 1. If a specific variant is fully selected, show its price
    if (selectedVariant) {
      return selectedVariant.price.toLocaleString("vi-VN") + " ₫";
    }

    // 2. If valid variants exist (in stock), show range: Min - Max
    // Filter only available variants? Requirement says "cheapest in stock - most expensive in stock"
    const stockVariants = product.productVariants.filter(
      (v) => v.availableStock > 0,
    );

    if (stockVariants.length === 0) {
      // If NO variants have stock... check if any variants exist at all
      // If all OOS, user wants "Hết hàng" in price area?
      // Instruction: "If all OOS, display 'Hết hàng' in price place"
      if (product.productVariants.length > 0) {
        return "Hết hàng";
      }
      return "Liên hệ";
    }

    if (stockVariants.length === 1) {
      return stockVariants[0].price.toLocaleString("vi-VN") + " ₫";
    }

    const prices = stockVariants.map((v) => v.price);
    const minPrice = Math.min(...prices);
    const maxPrice = Math.max(...prices);

    if (minPrice === maxPrice) {
      return minPrice.toLocaleString("vi-VN") + " ₫";
    }

    return `${minPrice.toLocaleString("vi-VN")} ₫ - ${maxPrice.toLocaleString("vi-VN")} ₫`;
  }, [product, selectedVariant]);

  // Use standard product images for the gallery list (thumbnails)
  const currentImages = useMemo(() => {
    return product?.productImages || [];
  }, [product]);

  // Determine the active variant image to display as the main image
  const activeVariantImage = useMemo(() => {
    if (selectedVariant && selectedVariant.imageUrl) {
      return {
        id: `variant-${selectedVariant.id}`,
        imageUrl: selectedVariant.imageUrl,
        displayOrder: -1,
      };
    }
    return null;
  }, [selectedVariant]);

  const allInStock = useMemo(() => {
    return product?.productVariants.some((v) => v.availableStock > 0) ?? false;
  }, [product]);

  // Button Label
  const buttonLabel = useMemo(() => {
    // 1. All Variants OOS
    if (!allInStock) return "Hết hàng";

    // 2. Not all options selected
    const allOptionsSelected = product?.productOptions.every(
      (opt) => selectedOptions[opt.id],
    );
    if (!allOptionsSelected) return "Chọn phân loại";

    // 3. invalid combination (selected options but no matching variant)
    if (!selectedVariant) return "Không khả dụng";

    // 4. Variant exists but OOS
    if (selectedVariant.availableStock <= 0) return "Không khả dụng";

    // 5. Valid
    return "Thêm vào giỏ hàng";
  }, [allInStock, selectedVariant, selectedOptions, product]);

  // Disable if:
  // 1. Not fully selected (we allow 'Select options' to be clickable? No usually disabled)
  // 2. Invalid combo
  // 3. OOS
  const isButtonDisabled =
    !selectedVariant || selectedVariant.availableStock <= 0;

  if (loading) {
    return (
      <MainLayout>
        <div className="flex justify-center items-center py-20">
          <Loader2 className="animate-spin h-8 w-8 text-blue-600" />
        </div>
      </MainLayout>
    );
  }

  if (error || !product) {
    return (
      <MainLayout>
        <div className="flex flex-col justify-center items-center py-20 px-4 text-center min-h-[60vh]">
          <div className="bg-red-50 dark:bg-red-900/10 p-4 rounded-full mb-6">
            <AlertCircle className="h-12 w-12 text-red-500 dark:text-red-400" />
          </div>
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">
            Đã có lỗi xảy ra
          </h2>
          <p className="text-gray-500 dark:text-gray-400 mb-8 max-w-md mx-auto">
            {error ||
              "Sản phẩm bạn đang tìm kiếm không tồn tại hoặc đã bị xóa."}
          </p>
          <Link
            to="/"
            className="inline-flex items-center justify-center rounded-lg bg-blue-600 px-6 py-3 text-base font-medium text-white shadow-sm hover:bg-blue-700 transition-colors focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 dark:focus:ring-offset-gray-900"
          >
            Quay về trang chủ
          </Link>
        </div>
      </MainLayout>
    );
  }

  return (
    <MainLayout>
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-8">
        {/* Brand and Category Header */}
        <ProductHeader brand={product.brand} categories={product.categories} />
        <div className="lg:grid lg:grid-cols-2 lg:gap-x-8 lg:items-start mb-16">
          {/* Gallery - Left Column */}
          <ProductGallery
            images={currentImages}
            activeImage={activeVariantImage}
          />

          {/* Info - Right Column */}
          <div className="mt-10 px-4 sm:mt-16 sm:px-0 lg:mt-0">
            <ProductInfo
              name={product.name}
              priceRange={priceDisplay}
              isAvailable={allInStock}
            />

            <div className="mt-8">
              <div className="flex justify-between items-center mb-4">
                <h3 className="text-sm font-medium text-gray-900 dark:text-gray-200">
                  Tùy chọn
                </h3>
                {Object.keys(selectedOptions).length > 0 && (
                  <button
                    onClick={() => setSelectedOptions({})}
                    className="text-sm text-blue-600 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300 font-medium transition-colors"
                  >
                    Đặt lại
                  </button>
                )}
              </div>
              <ProductOptions
                options={product.productOptions}
                variants={product.productVariants}
                selectedOptions={selectedOptions}
                onOptionSelect={handleOptionSelect}
              />
            </div>

            <div className="mt-8 border-t border-gray-100 pt-8 dark:border-gray-700">
              <button
                disabled={isButtonDisabled}
                onClick={async () => {
                  if (!selectedVariant) return;

                  if (!user) {
                    toast.error(
                      <span>
                        Vui lòng{" "}
                        <Link
                          to={PAGES.IDENTITY.LOGIN.PATH}
                          className="font-bold underline"
                        >
                          đăng nhập
                        </Link>{" "}
                        để thêm vào giỏ hàng.
                      </span>,
                    );
                    return;
                  }

                  try {
                    await cartApi.addToCart(selectedVariant.id);
                    toast.success(
                      <span>
                        Thêm vào giỏ hàng thành công!{" "}
                        <Link
                          to={PAGES.USER.CARTS.PATH}
                          className="font-bold underline"
                        >
                          Xem giỏ hàng
                        </Link>
                      </span>,
                    );
                    await refreshCartCount();
                  } catch (error: any) {
                    if (error.response && error.response.status === 401) {
                      toast.error(
                        <span>
                          Vui lòng{" "}
                          <Link
                            to={PAGES.IDENTITY.LOGIN.PATH}
                            className="font-bold underline"
                          >
                            đăng nhập
                          </Link>{" "}
                          để tiếp tục.
                        </span>,
                      );
                    } else {
                      const apiError = parseApiError(
                        error,
                        "Thêm vào giỏ hàng thất bại.",
                      );
                      toast.error(apiError.message);
                    }
                  }
                }}
                className={`w-full flex items-center justify-center rounded-md border border-transparent px-8 py-3 text-base font-medium text-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 ${
                  !isButtonDisabled
                    ? "bg-blue-600 hover:bg-blue-700 shadow-md hover:shadow-lg transition-all"
                    : "bg-gray-400 cursor-not-allowed dark:bg-gray-600"
                }`}
              >
                {buttonLabel}
              </button>
            </div>
          </div>
        </div>

        {/* Description Section - Full Width Below */}
        <div className="mt-16 bg-white dark:bg-gray-800 rounded-2xl shadow-sm border border-gray-100 dark:border-gray-700 p-6 sm:p-8">
          <ProductDescription description={product.description} />
        </div>
      </div>
    </MainLayout>
  );
};

export default ProductDetailPage;
