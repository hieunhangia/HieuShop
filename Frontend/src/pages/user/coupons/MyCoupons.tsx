import { useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import {
  ArrowDown,
  ArrowUp,
  ArrowUpDown,
  Filter,
  Info,
  Loader2,
  Search,
  Ticket,
} from "lucide-react";
import MainLayout from "../../../layouts/MainLayout";
import { PAGES } from "../../../config/page";
import couponApi from "../../../api/couponApi";
import type { UserCoupon } from "../../../types/user/coupons/dtos/UserCoupon";
import {
  DISCOUNT_TYPE,
  type DiscountType,
} from "../../../types/user/coupons/enums/DiscountType";
import Modal from "../../../components/Modal";

import type { SearchUserCouponsRequest } from "../../../types/user/coupons/dtos/SearchUserCouponsRequest";
import {
  USER_COUPON_SORT_COLUMN,
  type UserCouponSortColumn,
} from "../../../types/user/coupons/enums/UserCouponSortColumn";
import { SORT_DIRECTION } from "../../../types/common/enums/sortDirection";
import debounce from "lodash/debounce";

export default function MyCoupons() {
  const [coupons, setCoupons] = useState<UserCoupon[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedCoupon, setSelectedCoupon] = useState<UserCoupon | null>(null);

  const [query, setQuery] = useState<SearchUserCouponsRequest>({
    pageIndex: 1,
    pageSize: 9,
    searchText: "",
    sortColumn: USER_COUPON_SORT_COLUMN.CREATED_AT,
    sortDirection: SORT_DIRECTION.DESC,
  });
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);

  const fetchCoupons = useCallback(
    async (searchQuery: SearchUserCouponsRequest) => {
      try {
        setLoading(true);
        const data = await couponApi.getUserCoupons(searchQuery);
        setCoupons(data.items);
        setTotalPages(data.totalPages);
        setTotalCount(data.totalCount);
      } catch (error) {
        console.error("Failed to fetch user coupons:", error);
      } finally {
        setLoading(false);
      }
    },
    [],
  );

  useEffect(() => {
    document.title = `${PAGES.USER.COUPONS.MY.TITLE} | HieuShop`;
    fetchCoupons(query);
  }, [query, fetchCoupons]);

  const debouncedSearch = useCallback(
    debounce((value: string) => {
      setQuery((prev) => ({ ...prev, searchText: value, pageIndex: 1 }));
    }, 500),
    [],
  );

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
              {PAGES.USER.COUPONS.MY.TITLE}
            </h1>
            <p className="text-gray-500 dark:text-gray-400">
              Danh sách các mã giảm giá bạn đã đổi
            </p>
          </div>
          <div className="mt-4 md:mt-0 flex items-center gap-4">
            <div className="relative flex gap-2">
              <Link
                to={PAGES.USER.COUPONS.STORE.PATH}
                className="px-4 py-2 bg-brand-600 hover:bg-brand-700 text-white rounded-lg transition-colors font-medium flex items-center gap-2"
              >
                <Ticket size={18} />
                <span className="hidden sm:inline">Đến cửa hàng</span>
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
                placeholder="Tìm mã giảm giá..."
                className="w-full pl-10 pr-4 py-2 rounded-lg border border-gray-200 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 outline-none transition-all"
                onChange={(e) => debouncedSearch(e.target.value)}
              />
            </div>

            <div className="flex gap-2">
              <div className="relative min-w-[160px]">
                <Filter
                  className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400"
                  size={18}
                />
                <select
                  value={query.discountType || ""}
                  onChange={(e) =>
                    setQuery((prev) => ({
                      ...prev,
                      discountType: e.target.value
                        ? (e.target.value as DiscountType)
                        : undefined,
                      pageIndex: 1,
                    }))
                  }
                  className="w-full pl-10 pr-4 py-2 rounded-lg border border-gray-200 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 outline-none appearance-none cursor-pointer"
                >
                  <option value="">Tất cả</option>
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
                  value={query.sortColumn}
                  onChange={(e) =>
                    setQuery((prev) => ({
                      ...prev,
                      sortColumn: e.target.value as UserCouponSortColumn,
                    }))
                  }
                  className="w-full pl-10 pr-4 py-2 rounded-lg border border-gray-200 dark:border-gray-600 bg-gray-50 dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 outline-none appearance-none cursor-pointer"
                >
                  <option value={USER_COUPON_SORT_COLUMN.CREATED_AT}>
                    Thời gian mua
                  </option>
                  <option value={USER_COUPON_SORT_COLUMN.DISCOUNT_VALUE}>
                    Giá trị giảm
                  </option>
                  <option value={USER_COUPON_SORT_COLUMN.MIN_ORDER_AMOUNT}>
                    Đơn tối thiểu
                  </option>
                  <option value={USER_COUPON_SORT_COLUMN.MAX_DISCOUNT_AMOUNT}>
                    Số tiền tối đa giảm
                  </option>
                </select>
              </div>

              <div className="relative">
                <button
                  onClick={() =>
                    setQuery((prev) => ({
                      ...prev,
                      sortDirection:
                        prev.sortDirection === SORT_DIRECTION.ASC
                          ? SORT_DIRECTION.DESC
                          : SORT_DIRECTION.ASC,
                    }))
                  }
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

        {loading ? (
          <div className="flex justify-center items-center py-20">
            <Loader2 className="w-8 h-8 animate-spin text-brand-600" />
          </div>
        ) : coupons.length === 0 ? (
          <div className="text-center py-20 bg-white dark:bg-gray-800 rounded-xl border border-dashed border-gray-300 dark:border-gray-700">
            <div className="w-16 h-16 bg-gray-100 dark:bg-gray-700 rounded-full flex items-center justify-center mx-auto mb-4">
              <Ticket className="text-gray-400" size={32} />
            </div>
            <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-1">
              Bạn chưa có mã giảm giá nào
            </h3>
            <p className="text-gray-500 dark:text-gray-400 mb-4">
              Hãy đổi điểm tích lũy để nhận ưu đãi hấp dẫn
            </p>
            <Link
              to={PAGES.USER.COUPONS.STORE.PATH}
              className="inline-flex items-center text-brand-600 hover:text-brand-700 font-medium"
            >
              Đổi mã ngay &rarr;
            </Link>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {coupons.map((userCoupon) => (
              <div
                key={userCoupon.id}
                className={`rounded-xl shadow-sm border overflow-hidden flex flex-col ${
                  userCoupon.used
                    ? "bg-gray-50 dark:bg-gray-800/50 border-gray-200 dark:border-gray-700 opacity-75 grayscale-[0.5]"
                    : "bg-white dark:bg-gray-800 border-gray-100 dark:border-gray-700"
                }`}
              >
                <div
                  className={`p-1 bg-gradient-to-r ${
                    userCoupon.used
                      ? "from-gray-400 to-gray-500"
                      : "from-brand-500 to-indigo-500"
                  }`}
                ></div>
                <div className="p-6 flex-grow flex flex-col">
                  <div className="flex justify-between items-start mb-4">
                    <span
                      className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                        userCoupon.used
                          ? "bg-gray-200 text-gray-600 dark:bg-gray-700 dark:text-gray-400"
                          : userCoupon.coupon.discountType ===
                              DISCOUNT_TYPE.PERCENTAGE
                            ? "bg-purple-100 text-purple-800 dark:bg-purple-900/30 dark:text-purple-300"
                            : "bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-300"
                      }`}
                    >
                      {userCoupon.used
                        ? "Đã sử dụng"
                        : userCoupon.coupon.discountType ===
                            DISCOUNT_TYPE.PERCENTAGE
                          ? "Giảm %"
                          : "Giảm tiền"}
                    </span>
                  </div>

                  <h3 className="text-xl font-medium text-gray-900 dark:text-white mb-2 line-clamp-2">
                    {userCoupon.coupon.description}
                  </h3>

                  <div className="space-y-2 mb-6 flex-grow">
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-500 dark:text-gray-400">
                        Thời gian mua
                      </span>
                      <span className="font-medium text-gray-900 dark:text-white">
                        {new Date(userCoupon.createdAt).toLocaleDateString(
                          "vi-VN",
                        ) +
                          " " +
                          new Date(userCoupon.createdAt).toLocaleTimeString(
                            "vi-VN",
                            { hour: "2-digit", minute: "2-digit" },
                          )}
                      </span>
                    </div>
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-500 dark:text-gray-400">
                        Giá trị giảm
                      </span>
                      <span className="font-semibold text-gray-900 dark:text-white">
                        {userCoupon.coupon.discountValue.toLocaleString()}
                        {userCoupon.coupon.discountType ===
                        DISCOUNT_TYPE.PERCENTAGE
                          ? "%"
                          : "đ"}
                      </span>
                    </div>
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-500 dark:text-gray-400">
                        Giảm tối đa
                      </span>
                      <span className="font-medium text-gray-900 dark:text-white">
                        {userCoupon.coupon.maxDiscountAmount
                          ? `${userCoupon.coupon.maxDiscountAmount.toLocaleString()}đ`
                          : "Không giới hạn"}
                      </span>
                    </div>
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-500 dark:text-gray-400">
                        Đơn tối thiểu:
                      </span>
                      <span className="font-medium text-gray-900 dark:text-white">
                        {userCoupon.coupon.minOrderAmount
                          ? `${userCoupon.coupon.minOrderAmount.toLocaleString()}đ`
                          : "0đ"}
                      </span>
                    </div>
                  </div>

                  <button
                    onClick={() => setSelectedCoupon(userCoupon)}
                    className="w-full mt-4 flex items-center justify-center space-x-2 py-2 rounded-lg border border-gray-200 dark:border-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700/50 font-medium transition-colors"
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
          <div className="flex justify-center mt-8">
            <nav className="flex items-center space-x-2 bg-white dark:bg-gray-800 p-2 rounded-lg shadow-sm border border-gray-100 dark:border-gray-700">
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

        <Modal
          isOpen={!!selectedCoupon}
          onClose={() => setSelectedCoupon(null)}
          title={
            selectedCoupon?.used
              ? "Thông tin mã giảm giá (Đã dùng)"
              : "Thông tin mã giảm giá"
          }
        >
          {selectedCoupon && (
            <div className="flex flex-col items-center">
              <div className="w-full mb-6 text-center">
                <span
                  className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold mb-3 ${
                    selectedCoupon.used
                      ? "bg-gray-200 text-gray-600 dark:bg-gray-700 dark:text-gray-400"
                      : selectedCoupon.coupon.discountType ===
                          DISCOUNT_TYPE.PERCENTAGE
                        ? "bg-purple-100 text-purple-800 dark:bg-purple-900/30 dark:text-purple-300"
                        : "bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-300"
                  }`}
                >
                  {selectedCoupon.used
                    ? "Đã sử dụng"
                    : selectedCoupon.coupon.discountType ===
                        DISCOUNT_TYPE.PERCENTAGE
                      ? "Giảm %"
                      : "Giảm tiền"}
                </span>
                <h4
                  className={`text-xl font-bold leading-snug ${
                    selectedCoupon.used
                      ? "text-gray-500 dark:text-gray-400 line-through"
                      : "text-gray-900 dark:text-white"
                  }`}
                >
                  {selectedCoupon.coupon.description}
                </h4>
              </div>

              <div
                className={`w-full rounded-xl p-4 border mb-6 space-y-3 ${
                  selectedCoupon.used
                    ? "bg-gray-100 dark:bg-gray-800 border-gray-200 dark:border-gray-700 opacity-80"
                    : "bg-gray-50 dark:bg-gray-800/50 border-gray-100 dark:border-gray-700/50"
                }`}
              >
                <div className="flex justify-between items-center py-1">
                  <span className="text-gray-500 dark:text-gray-400 text-sm">
                    Thời gian mua
                  </span>
                  <span className="font-semibold text-gray-900 dark:text-white">
                    {new Date(selectedCoupon.createdAt).toLocaleDateString(
                      "vi-VN",
                    ) +
                      " " +
                      new Date(selectedCoupon.createdAt).toLocaleTimeString(
                        "vi-VN",
                        { hour: "2-digit", minute: "2-digit" },
                      )}
                  </span>
                </div>
                <div className="flex justify-between items-center py-1 border-t border-gray-100 dark:border-gray-700/50 pt-2">
                  <span className="text-gray-500 dark:text-gray-400 text-sm">
                    Giá trị giảm
                  </span>
                  <span className="font-semibold text-gray-900 dark:text-white">
                    {selectedCoupon.coupon.discountValue.toLocaleString()}
                    {selectedCoupon.coupon.discountType ===
                    DISCOUNT_TYPE.PERCENTAGE
                      ? "%"
                      : "đ"}
                  </span>
                </div>
                <div className="flex justify-between items-center py-1 border-t border-gray-100 dark:border-gray-700/50 pt-2">
                  <span className="text-gray-500 dark:text-gray-400 text-sm">
                    Giảm tối đa
                  </span>
                  <span className="font-semibold text-gray-900 dark:text-white">
                    {selectedCoupon.coupon.maxDiscountAmount
                      ? `${selectedCoupon.coupon.maxDiscountAmount.toLocaleString()}đ`
                      : "Không giới hạn"}
                  </span>
                </div>
                <div className="flex justify-between items-center py-1 border-t border-gray-100 dark:border-gray-700/50 pt-2">
                  <span className="text-gray-500 dark:text-gray-400 text-sm">
                    Đơn tối thiểu
                  </span>
                  <span className="font-semibold text-gray-900 dark:text-white">
                    {selectedCoupon.coupon.minOrderAmount
                      ? `${selectedCoupon.coupon.minOrderAmount.toLocaleString()}đ`
                      : "0đ"}
                  </span>
                </div>
              </div>

              <div className="w-full">
                <button
                  onClick={() => setSelectedCoupon(null)}
                  className="w-full py-2.5 rounded-xl border border-gray-200 dark:border-gray-700 text-gray-700 dark:text-gray-300 font-medium hover:bg-gray-50 dark:hover:bg-gray-700/50 transition-colors"
                >
                  Đóng
                </button>
              </div>
            </div>
          )}
        </Modal>
      </div>
    </MainLayout>
  );
}
