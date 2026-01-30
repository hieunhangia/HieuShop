import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Loader2, Ticket, Info } from "lucide-react";
import MainLayout from "../../../layouts/MainLayout";
import { PAGES } from "../../../config/page";
import couponApi from "../../../api/couponApi";
import type { UserCoupon } from "../../../types/user/coupons/dtos/UserCoupon.ts";
import { DISCOUNT_TYPE } from "../../../types/user/coupons/enums/DiscountType";
import Modal from "../../../components/Modal";
import type { Coupon } from "../../../types/user/coupons/dtos/Coupon.ts";

export default function MyCoupons() {
  const [coupons, setCoupons] = useState<UserCoupon[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedCoupon, setSelectedCoupon] = useState<Coupon | null>(null);

  useEffect(() => {
    document.title = `${PAGES.USER.COUPONS.MY.TITLE} | HieuShop`;
    const fetchCoupons = async () => {
      try {
        const data = await couponApi.getUserCoupons();
        setCoupons(data);
      } catch (error) {
        console.error("Failed to fetch user coupons:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchCoupons();
  }, []);

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
          <div className="mt-4 md:mt-0 flex gap-2">
            <Link
              to={PAGES.USER.COUPONS.STORE.PATH}
              className="px-4 py-2 bg-brand-600 hover:bg-brand-700 text-white rounded-lg transition-colors font-medium flex items-center gap-2"
            >
              <Ticket size={18} />
              Đến cửa hàng
            </Link>
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
                        Giá trị:
                      </span>
                      <span className="font-semibold text-gray-900 dark:text-white">
                        {userCoupon.coupon.discountValue.toLocaleString()}
                        {userCoupon.coupon.discountType ===
                        DISCOUNT_TYPE.PERCENTAGE
                          ? "%"
                          : "đ"}
                      </span>
                    </div>
                    {userCoupon.coupon.maxDiscountAmount && (
                      <div className="flex justify-between text-sm">
                        <span className="text-gray-500 dark:text-gray-400">
                          Tối đa:
                        </span>
                        <span className="font-medium text-gray-900 dark:text-white">
                          {userCoupon.coupon.maxDiscountAmount.toLocaleString()}
                          đ
                        </span>
                      </div>
                    )}
                    {userCoupon.coupon.minOrderAmount && (
                      <div className="flex justify-between text-sm">
                        <span className="text-gray-500 dark:text-gray-400">
                          Đơn tối thiểu:
                        </span>
                        <span className="font-medium text-gray-900 dark:text-white">
                          {userCoupon.coupon.minOrderAmount.toLocaleString()}đ
                        </span>
                      </div>
                    )}
                  </div>

                  <button
                    onClick={() => setSelectedCoupon(userCoupon.coupon)}
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
                {selectedCoupon.maxDiscountAmount && (
                  <div className="flex justify-between items-center py-1 border-t border-gray-100 dark:border-gray-700/50 pt-2">
                    <span className="text-gray-500 dark:text-gray-400 text-sm">
                      Giảm tối đa
                    </span>
                    <span className="font-semibold text-gray-900 dark:text-white">
                      {selectedCoupon.maxDiscountAmount.toLocaleString()}đ
                    </span>
                  </div>
                )}
                {selectedCoupon.minOrderAmount && (
                  <div className="flex justify-between items-center py-1 border-t border-gray-100 dark:border-gray-700/50 pt-2">
                    <span className="text-gray-500 dark:text-gray-400 text-sm">
                      Đơn tối thiểu
                    </span>
                    <span className="font-semibold text-gray-900 dark:text-white">
                      {selectedCoupon.minOrderAmount.toLocaleString()}đ
                    </span>
                  </div>
                )}
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
