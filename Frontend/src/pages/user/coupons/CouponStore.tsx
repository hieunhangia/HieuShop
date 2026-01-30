import React, { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import {
  Search,
  Filter,
  AlertCircle,
  ShoppingBag,
  Loader2,
  Info,
  ArrowUp,
  ArrowDown,
  ArrowUpDown,
  Ticket,
} from "lucide-react";
import debounce from "lodash/debounce";
import toast from "react-hot-toast";
import MainLayout from "../../../layouts/MainLayout";
import { PAGES } from "../../../config/page";
import couponApi from "../../../api/couponApi";
import type { Coupon } from "../../../types/user/coupons/dtos/Coupon.ts";
import { DISCOUNT_TYPE } from "../../../types/user/coupons/enums/DiscountType";
import { COUPON_SORT_COLUMN } from "../../../types/user/coupons/enums/CouponSortColumn";
import { SORT_DIRECTION } from "../../../types/common/enums/sortDirection";
import type { SearchActiveCouponsQuery } from "../../../types/user/coupons/dtos/SearchActiveCouponsQuery";
import Modal from "../../../components/Modal";
import { parseApiError } from "../../../utils/error";

export default function CouponStore() {
  const [coupons, setCoupons] = useState<Coupon[]>([]);
  const [loading, setLoading] = useState(false);
  const [purchasingId, setPurchasingId] = useState<string | null>(null);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [selectedCoupon, setSelectedCoupon] = useState<Coupon | null>(null);
  const [loyaltyPoints, setLoyaltyPoints] = useState<number | null>(null);

  const [query, setQuery] = useState<SearchActiveCouponsQuery>({
    pageIndex: 1,
    pageSize: 12,
    sortColumn: COUPON_SORT_COLUMN.DISCOUNT_VALUE,
    sortDirection: SORT_DIRECTION.DESC,
    searchText: "",
  });

  const fetchCoupons = async (searchQuery: SearchActiveCouponsQuery) => {
    setLoading(true);
    try {
      const response = await couponApi.getActiveCoupons(searchQuery);
      setCoupons(response.items);
      setTotalPages(response.totalPages);
      setTotalCount(response.totalCount);
    } catch (error) {
      console.error("Failed to fetch coupons:", error);
      const apiError = parseApiError(
        error,
        "Không thể tải danh sách mã giảm giá",
      );
      toast.error(apiError.message);
    } finally {
      setLoading(false);
    }
  };

  const fetchLoyaltyPoints = async () => {
    try {
      const points = await couponApi.getLoyaltyPoints();
      setLoyaltyPoints(points);
    } catch (error) {
      console.error("Failed to fetch loyalty points:", error);
    }
  };

  // eslint-disable-next-line react-hooks/exhaustive-deps
  const debouncedSearch = useCallback(
    debounce((searchText: string) => {
      setQuery((prev) => ({ ...prev, searchText, pageIndex: 1 }));
    }, 500),
    [],
  );

  useEffect(() => {
    document.title = `${PAGES.USER.COUPONS.STORE.TITLE} | HieuShop`;
    fetchCoupons(query);
    fetchLoyaltyPoints();
  }, [query]);

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    debouncedSearch(e.target.value);
  };

  const handleDiscountTypeChange = (
    e: React.ChangeEvent<HTMLSelectElement>,
  ) => {
    const value = e.target.value;
    setQuery((prev) => ({
      ...prev,
      discountType: value === "" ? undefined : (value as any),
      pageIndex: 1,
    }));
  };

  const handleSortChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const value = e.target
      .value as (typeof COUPON_SORT_COLUMN)[keyof typeof COUPON_SORT_COLUMN];
    setQuery((prev) => ({
      ...prev,
      sortColumn: value,
      pageIndex: 1,
    }));
  };

  const toggleSortDirection = () => {
    setQuery((prev) => ({
      ...prev,
      sortDirection:
        prev.sortDirection === SORT_DIRECTION.ASC
          ? SORT_DIRECTION.DESC
          : SORT_DIRECTION.ASC,
      pageIndex: 1,
    }));
  };

  const handlePurchase = async (coupon: Coupon) => {
    setPurchasingId(coupon.id);
    try {
      await couponApi.purchaseCoupon(coupon.id);
      toast.success(
        "Mua mã giảm giá thành công! Bạn có thể xem trong ví của mình.",
      );
      await fetchLoyaltyPoints(); // Refresh points
      setSelectedCoupon(null); // Close modal on success
    } catch (error) {
      console.error(error);
      const apiError = parseApiError(
        error,
        "Mua thất bại. Vui lòng kiểm tra lại điểm tích lũy.",
      );
      toast.error(apiError.message);
    } finally {
      setPurchasingId(null);
    }
  };

  const handlePageChange = (newPage: number) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setQuery((prev) => ({ ...prev, pageIndex: newPage }));
    }
  };

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex flex-col md:flex-row md:items-center md:justify-between mb-8">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 dark:text-white mb-2">
              {PAGES.USER.COUPONS.STORE.TITLE}
            </h1>
            <p className="text-gray-500 dark:text-gray-400">
              Đổi điểm tích lũy lấy mã giảm giá hấp dẫn
            </p>
          </div>
          <div className="mt-4 md:mt-0 flex items-center gap-4">
            <div className="bg-amber-100 dark:bg-amber-900/40 border border-amber-200 dark:border-amber-700/50 rounded-xl px-5 py-2 flex flex-col items-end">
              <span className="text-xs font-semibold text-amber-700 dark:text-amber-400 uppercase tracking-wide">
                Số điểm đang có
              </span>
              <div className="flex items-center text-amber-600 dark:text-amber-500 font-bold text-2xl">
                <span>{loyaltyPoints?.toLocaleString() ?? 0}</span>
                <span className="text-sm ml-1 font-semibold text-amber-600/80 dark:text-amber-500/80">
                  điểm
                </span>
              </div>
            </div>
            <div className="relative flex gap-2">
              <Link
                to={PAGES.USER.COUPONS.MY.PATH}
                className="px-4 py-2 bg-white dark:bg-gray-800 border border-gray-300 dark:border-gray-600 rounded-lg text-brand-600 dark:text-brand-400 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors font-medium flex items-center gap-2"
              >
                <Ticket size={18} />
                <span className="hidden sm:inline">Mã của tôi</span>
              </Link>
            </div>
          </div>
        </div>

        {/* Filters & Search */}
        <div className="bg-white dark:bg-gray-800 rounded-xl shadow-sm p-4 mb-8 border border-gray-100 dark:border-gray-700 relative">
          <div className="flex flex-col md:flex-row gap-4 pt-2 md:pt-0">
            <div className="flex-1 relative">
              <Search
                className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400"
                size={20}
              />
              <input
                type="text"
                placeholder="Tìm kiếm mã giảm giá..."
                className="w-full pl-10 pr-4 py-2 rounded-lg border border-gray-200 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 outline-none transition-all"
                onChange={handleSearchChange}
              />
            </div>
            <div className="flex gap-2">
              <div className="relative min-w-[160px]">
                <Filter
                  className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400"
                  size={18}
                />
                <select
                  className="w-full pl-10 pr-4 py-2 rounded-lg border border-gray-200 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 outline-none appearance-none cursor-pointer"
                  value={query.discountType || ""}
                  onChange={handleDiscountTypeChange}
                >
                  <option value="">Tất cả loại</option>
                  <option value={DISCOUNT_TYPE.PERCENTAGE}>Giảm theo %</option>
                  <option value={DISCOUNT_TYPE.FIXED_AMOUNT}>
                    Giảm số tiền
                  </option>
                </select>
              </div>
              <div className="relative min-w-[180px]">
                <ArrowUpDown
                  className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400"
                  size={18}
                />
                <select
                  className="w-full pl-10 pr-4 py-2 rounded-lg border border-gray-200 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 outline-none appearance-none cursor-pointer"
                  value={query.sortColumn}
                  onChange={handleSortChange}
                >
                  <option value={COUPON_SORT_COLUMN.DISCOUNT_VALUE}>
                    Giá trị giảm
                  </option>
                  <option value={COUPON_SORT_COLUMN.LOYALTY_POINTS_COST}>
                    Điểm cần đổi
                  </option>
                  <option value={COUPON_SORT_COLUMN.MIN_ORDER_AMOUNT}>
                    Đơn tối thiểu
                  </option>
                  <option value={COUPON_SORT_COLUMN.MAX_DISCOUNT_AMOUNT}>
                    Số tiền tối đa giảm
                  </option>
                </select>
              </div>
              <div className="relative">
                <button
                  onClick={toggleSortDirection}
                  className="h-full flex items-center gap-2 px-4 py-2 rounded-lg border border-gray-200 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-900 dark:text-white hover:bg-gray-100 dark:hover:bg-gray-600 transition-colors"
                >
                  {query.sortDirection === SORT_DIRECTION.ASC ? (
                    <>
                      <ArrowUp size={18} />
                    </>
                  ) : (
                    <>
                      <ArrowDown size={18} />
                    </>
                  )}
                </button>
              </div>
            </div>
          </div>
        </div>

        {/* Coupon Grid */}
        {loading ? (
          <div className="flex justify-center items-center py-20">
            <Loader2 className="w-8 h-8 animate-spin text-brand-600" />
          </div>
        ) : coupons.length === 0 ? (
          <div className="text-center py-20 bg-white dark:bg-gray-800 rounded-xl border border-dashed border-gray-300 dark:border-gray-700">
            <div className="w-16 h-16 bg-gray-100 dark:bg-gray-700 rounded-full flex items-center justify-center mx-auto mb-4">
              <AlertCircle className="text-gray-400" size={32} />
            </div>
            <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-1">
              Không tìm thấy mã giảm giá
            </h3>
            <p className="text-gray-500 dark:text-gray-400">
              Thử thay đổi bộ lọc hoặc tìm kiếm từ khóa khác
            </p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {coupons.map((coupon) => (
              <div
                key={coupon.id}
                className="bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-100 dark:border-gray-700 overflow-hidden hover:shadow-lg transition-shadow duration-300 flex flex-col"
              >
                <div className="p-1 bg-gradient-to-r from-brand-500 to-indigo-500"></div>
                <div className="p-6 flex-grow flex flex-col">
                  <div className="flex justify-between items-start mb-4">
                    <span
                      className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                        coupon.discountType === DISCOUNT_TYPE.PERCENTAGE
                          ? "bg-purple-100 text-purple-800 dark:bg-purple-900/30 dark:text-purple-300"
                          : "bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-300"
                      }`}
                    >
                      {coupon.discountType === DISCOUNT_TYPE.PERCENTAGE
                        ? "Giảm %"
                        : "Giảm tiền"}
                    </span>
                    <span className="flex items-center text-amber-500 font-bold text-sm">
                      {coupon.loyaltyPointsCost.toLocaleString()} điểm
                    </span>
                  </div>

                  <h3 className="text-xl font-medium text-gray-900 dark:text-white mb-2 line-clamp-2">
                    {coupon.description}
                  </h3>

                  <div className="space-y-2 mb-6 flex-grow">
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-500 dark:text-gray-400">
                        Giá trị giảm:
                      </span>
                      <span className="font-semibold text-gray-900 dark:text-white">
                        {coupon.discountValue.toLocaleString()}
                        {coupon.discountType === DISCOUNT_TYPE.PERCENTAGE
                          ? "%"
                          : "đ"}
                      </span>
                    </div>
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-500 dark:text-gray-400">
                        Giảm tối đa:
                      </span>
                      <span className="font-medium text-gray-900 dark:text-white">
                        {coupon.maxDiscountAmount
                          ? `${coupon.maxDiscountAmount.toLocaleString()}đ`
                          : "Không giới hạn"}
                      </span>
                    </div>
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-500 dark:text-gray-400">
                        Đơn tối thiểu:
                      </span>
                      <span className="font-medium text-gray-900 dark:text-white">
                        {coupon.minOrderAmount
                          ? `${coupon.minOrderAmount.toLocaleString()}đ`
                          : "0đ"}
                      </span>
                    </div>
                  </div>

                  <button
                    onClick={() => setSelectedCoupon(coupon)}
                    className="w-full flex items-center justify-center space-x-2 py-2.5 rounded-lg border border-brand-200 text-brand-600 hover:bg-brand-50 dark:border-brand-900 dark:text-brand-400 dark:hover:bg-brand-900/10 font-medium transition-colors"
                  >
                    <Info size={18} />
                    <span>Chi tiết</span>
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}

        {/* Pagination */}
        {!loading && totalPages > 1 && (
          <div className="mt-8 flex justify-center">
            <nav className="flex items-center space-x-2">
              <button
                onClick={() => handlePageChange(query.pageIndex! - 1)}
                disabled={query.pageIndex === 1}
                className="px-3 py-1 rounded-md border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-200 hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Trước
              </button>
              <span className="text-sm text-gray-600 dark:text-gray-400">
                Trang {query.pageIndex} / {totalPages} ({totalCount} kết quả)
              </span>
              <button
                onClick={() => handlePageChange(query.pageIndex! + 1)}
                disabled={query.pageIndex === totalPages}
                className="px-3 py-1 rounded-md border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-200 hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Sau
              </button>
            </nav>
          </div>
        )}

        {/* Detail Modal */}
        <Modal
          isOpen={!!selectedCoupon}
          onClose={() => setSelectedCoupon(null)}
          title="Thông tin mã giảm giá"
        >
          {selectedCoupon && (
            <div className="flex flex-col items-center">
              <div className="w-full mb-6">
                <span
                  className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold mb-3 ${
                    selectedCoupon.discountType === DISCOUNT_TYPE.PERCENTAGE
                      ? "bg-purple-100 text-purple-800 dark:bg-purple-900/30 dark:text-purple-300"
                      : "bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-300"
                  }`}
                >
                  {selectedCoupon.discountType === DISCOUNT_TYPE.PERCENTAGE
                    ? "Giảm %"
                    : "Giảm tiền"}
                </span>
                <h4 className="text-xl font-bold text-gray-900 dark:text-white leading-snug">
                  {selectedCoupon.description}
                </h4>
              </div>

              <div className="bg-amber-50 dark:bg-amber-900/10 rounded-2xl p-6 w-full mb-6 flex flex-col items-center justify-center border border-amber-100 dark:border-amber-800/30">
                <span className="text-sm font-medium text-amber-600 dark:text-amber-400 uppercase tracking-wide mb-1">
                  Điểm cần đổi
                </span>
                <div className="text-4xl font-extrabold text-amber-500">
                  {selectedCoupon.loyaltyPointsCost.toLocaleString()}
                  <span className="text-xl ml-1 font-bold text-amber-500/80">
                    điểm
                  </span>
                </div>
              </div>

              <div className="w-full bg-gray-50 dark:bg-gray-800/50 rounded-xl p-4 border border-gray-100 dark:border-gray-700/50 mb-6 space-y-3">
                <div className="flex justify-between items-center py-1">
                  <span className="text-gray-500 dark:text-gray-400 text-sm">
                    Giá trị giảm
                  </span>
                  <span className="font-semibold text-gray-900 dark:text-white">
                    {selectedCoupon.discountValue.toLocaleString()}
                    {selectedCoupon.discountType === DISCOUNT_TYPE.PERCENTAGE
                      ? "%"
                      : "đ"}
                  </span>
                </div>
                <div className="flex justify-between items-center py-1 border-t border-gray-100 dark:border-gray-700/50 pt-2">
                  <span className="text-gray-500 dark:text-gray-400 text-sm">
                    Giảm tối đa
                  </span>
                  <span className="font-semibold text-gray-900 dark:text-white">
                    {selectedCoupon.maxDiscountAmount
                      ? `${selectedCoupon.maxDiscountAmount.toLocaleString()}đ`
                      : "Không giới hạn"}
                  </span>
                </div>
                <div className="flex justify-between items-center py-1 border-t border-gray-100 dark:border-gray-700/50 pt-2">
                  <span className="text-gray-500 dark:text-gray-400 text-sm">
                    Đơn tối thiểu
                  </span>
                  <span className="font-semibold text-gray-900 dark:text-white">
                    {selectedCoupon.minOrderAmount
                      ? `${selectedCoupon.minOrderAmount.toLocaleString()}đ`
                      : "0đ"}
                  </span>
                </div>
              </div>

              <div className="flex w-full gap-3">
                <button
                  onClick={() => setSelectedCoupon(null)}
                  className="flex-1 py-2.5 rounded-xl border border-gray-200 dark:border-gray-700 text-gray-700 dark:text-gray-300 font-medium hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-colors"
                >
                  Đóng
                </button>
                <button
                  onClick={() => handlePurchase(selectedCoupon)}
                  disabled={purchasingId === selectedCoupon.id}
                  className="flex-[2] flex items-center justify-center space-x-2 py-2.5 rounded-xl bg-brand-600 hover:bg-brand-700 text-white font-bold shadow-lg shadow-brand-500/20 transition-all disabled:opacity-50 disabled:cursor-not-allowed disabled:shadow-none"
                >
                  {purchasingId === selectedCoupon.id ? (
                    <Loader2 className="animate-spin" size={20} />
                  ) : (
                    <ShoppingBag size={20} />
                  )}
                  <span>
                    {purchasingId === selectedCoupon.id
                      ? "Đang xử lý..."
                      : "Đổi ngay"}
                  </span>
                </button>
              </div>
            </div>
          )}
        </Modal>
      </div>
    </MainLayout>
  );
}
