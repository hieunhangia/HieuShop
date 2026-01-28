import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { Trash2, Minus, Plus, ShoppingBag, ArrowRight } from "lucide-react";
import MainLayout from "../../layouts/MainLayout";
import { cartApi } from "../../api/cartApi";
import type { CartItemDto } from "../../api/cartApi";
import { useCart } from "../../context/CartContext";
import toast from "react-hot-toast";
import { parseApiError } from "../../utils/error";
import { PAGES } from "../../config/page";

const CartPage = () => {
  const [items, setItems] = useState<CartItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedItems, setSelectedItems] = useState<Set<string>>(new Set());
  const { refreshCartCount } = useCart();

  useEffect(() => {
    const fetchCart = async () => {
      setLoading(true);
      try {
        const response = await cartApi.syncCart();
        setItems(response.data.cartItems || []);
        if (response.data.warningMessage) {
          toast(response.data.warningMessage, { icon: "⚠️" });
        }
      } catch (error) {
        toast.error("Không thể tải giỏ hàng.");
      } finally {
        setLoading(false);
      }
    };
    fetchCart();
    document.title = `${PAGES.CART.TITLE} | HieuShop`;
  }, []);

  const handleQuantityChange = async (id: string, newQuantity: number) => {
    if (newQuantity < 1) return;

    // Optimistic update
    setItems((prev) =>
      prev.map((item) =>
        item.id === id ? { ...item, quantity: newQuantity } : item,
      ),
    );

    try {
      await cartApi.updateCartItemQuantity(id, newQuantity);
    } catch (error: any) {
      // Revert on error
      const apiError = parseApiError(error, "Cập nhật số lượng thất bại");
      toast.error(apiError.message);
      try {
        const response = await cartApi.syncCart();
        setItems(response.data.cartItems || []);
      } catch (e) {}
    }
  };

  const handleRemove = async (id: string) => {
    try {
      await cartApi.removeCartItem(id);
      setItems((prev) => prev.filter((item) => item.id !== id));
      setSelectedItems((prev) => {
        const next = new Set(prev);
        next.delete(id);
        return next;
      });
      refreshCartCount();
      toast.success("Đã xóa sản phẩm khỏi giỏ hàng");
    } catch (error: any) {
      const apiError = parseApiError(error, "Xóa sản phẩm thất bại");
      toast.error(apiError.message);
    }
  };

  const toggleSelect = (id: string) => {
    setSelectedItems((prev) => {
      const next = new Set(prev);
      if (next.has(id)) {
        next.delete(id);
      } else {
        next.add(id);
      }
      return next;
    });
  };

  const toggleSelectAll = () => {
    if (selectedItems.size === items.length) {
      setSelectedItems(new Set());
    } else {
      setSelectedItems(new Set(items.map((i) => i.id)));
    }
  };

  const totalPrice = useMemo(() => {
    return items
      .filter((item) => selectedItems.has(item.id))
      .reduce(
        (sum, item) => sum + item.productVariant.price * item.quantity,
        0,
      );
  }, [items, selectedItems]);

  if (loading) {
    return (
      <MainLayout>
        <div className="flex justify-center items-center py-20 min-h-[60vh]">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-brand-600"></div>
        </div>
      </MainLayout>
    );
  }

  if (items.length === 0) {
    return (
      <MainLayout>
        <div className="flex flex-col justify-center items-center py-20 px-4 min-h-[60vh]">
          <ShoppingBag size={64} className="text-gray-300 mb-6" />
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">
            Giỏ hàng trống
          </h2>
          <p className="text-gray-500 dark:text-gray-400 mb-8">
            Bạn chưa thêm sản phẩm nào vào giỏ hàng.
          </p>
          <Link
            to={PAGES.PRODUCTS.ALL.PATH}
            className="inline-flex items-center justify-center rounded-lg bg-brand-600 px-6 py-3 text-base font-medium text-white shadow-sm hover:bg-brand-700 transition-colors"
          >
            Tiếp tục mua sắm
          </Link>
        </div>
      </MainLayout>
    );
  }

  return (
    <MainLayout>
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-8">
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white mb-8">
          {PAGES.CART.TITLE}
        </h1>

        <div className="lg:grid lg:grid-cols-12 lg:gap-x-12 lg:items-start">
          <div className="lg:col-span-8">
            {/* Header */}
            <div className="flex items-center justify-between py-4 border-b border-gray-200 dark:border-gray-700">
              <div className="flex items-center">
                <input
                  type="checkbox"
                  checked={
                    items.length > 0 && selectedItems.size === items.length
                  }
                  onChange={toggleSelectAll}
                  className="h-4 w-4 text-brand-600 focus:ring-brand-500 border-gray-300 rounded"
                />
                <span className="ml-2 text-sm text-gray-700 dark:text-gray-300">
                  Chọn tất cả ({items.length})
                </span>
              </div>
            </div>

            <ul className="divide-y divide-gray-200 dark:divide-gray-700">
              {items.map((item) => (
                <li key={item.id} className="flex py-6">
                  <div className="flex items-center h-full mr-4">
                    <input
                      type="checkbox"
                      checked={selectedItems.has(item.id)}
                      onChange={() => toggleSelect(item.id)}
                      className="h-4 w-4 text-brand-600 focus:ring-brand-500 border-gray-300 rounded"
                    />
                  </div>
                  <div className="h-24 w-24 flex-shrink-0 overflow-hidden rounded-md border border-gray-200 dark:border-gray-700">
                    <img
                      src={item.productVariant.imageUrl}
                      alt={item.productVariant.product.name}
                      className="h-full w-full object-cover object-center"
                    />
                  </div>

                  <div className="ml-4 flex flex-1 flex-col">
                    <div>
                      <div className="flex justify-between text-base font-medium text-gray-900 dark:text-white">
                        <h3>
                          <Link
                            to={`/products/${item.productVariant.product.slug}`}
                          >
                            {item.productVariant.product.name}
                          </Link>
                        </h3>
                        <p className="ml-4">
                          {item.productVariant.price.toLocaleString("vi-VN")} ₫
                        </p>
                      </div>
                      <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                        {item.productVariant.productOptionValuesString}
                      </p>
                    </div>
                    <div className="flex flex-1 items-end justify-between text-sm">
                      <div className="flex items-center border border-gray-300 dark:border-gray-600 rounded">
                        <button
                          onClick={() =>
                            handleQuantityChange(item.id, item.quantity - 1)
                          }
                          disabled={item.quantity <= 1}
                          className="p-1 hover:bg-gray-100 dark:hover:bg-gray-700 disabled:opacity-50"
                        >
                          <Minus size={16} />
                        </button>
                        <span className="px-4 py-1 font-medium">
                          {item.quantity}
                        </span>
                        <button
                          onClick={() =>
                            handleQuantityChange(item.id, item.quantity + 1)
                          }
                          className="p-1 hover:bg-gray-100 dark:hover:bg-gray-700"
                        >
                          <Plus size={16} />
                        </button>
                      </div>

                      <div className="flex">
                        <button
                          type="button"
                          onClick={() => handleRemove(item.id)}
                          className="font-medium text-red-600 hover:text-red-500 flex items-center"
                        >
                          <Trash2 size={16} className="mr-1" />
                          Xóa
                        </button>
                      </div>
                    </div>
                  </div>
                </li>
              ))}
            </ul>
          </div>

          <div className="lg:col-span-4 mt-8 lg:mt-0">
            <div className="bg-gray-50 dark:bg-gray-800 rounded-lg p-6 border border-gray-200 dark:border-gray-700 sticky top-24">
              <h2 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
                Tổng quan đơn hàng
              </h2>

              <div className="flow-root">
                <div className="-my-4 divide-y divide-gray-200 dark:divide-gray-700">
                  <div className="flex items-center justify-between py-4">
                    <dt className="text-gray-600 dark:text-gray-400">
                      Tạm tính
                    </dt>
                    <dd className="font-medium text-gray-900 dark:text-white">
                      {totalPrice.toLocaleString("vi-VN")} ₫
                    </dd>
                  </div>
                  <div className="flex items-center justify-between py-4">
                    <dt className="text-base font-bold text-gray-900 dark:text-white">
                      Tổng cộng
                    </dt>
                    <dd className="text-base font-bold text-brand-600 dark:text-brand-400">
                      {totalPrice.toLocaleString("vi-VN")} ₫
                    </dd>
                  </div>
                </div>
              </div>

              <div className="mt-6">
                <button
                  disabled={selectedItems.size === 0}
                  className="w-full flex items-center justify-center rounded-md border border-transparent bg-brand-600 px-6 py-3 text-base font-medium text-white shadow-sm hover:bg-brand-700 focus:outline-none focus:ring-2 focus:ring-brand-500 focus:ring-offset-2 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
                >
                  Thanh toán <ArrowRight size={18} className="ml-2" />
                </button>
                {selectedItems.size === 0 && (
                  <p className="mt-2 text-center text-sm text-red-500">
                    Vui lòng chọn sản phẩm để thanh toán
                  </p>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </MainLayout>
  );
};

export default CartPage;
