import React, { useState } from "react";
import { useAuth } from "../../context/AuthContext";
import MainLayout from "../../layouts/MainLayout";
import { PAGES } from "../../config/page";
import { Link } from "react-router-dom";
import { User, Shield, Key, AlertTriangle, Send } from "lucide-react";
import { identityApi } from "../../api/identityApi";
import toast from "react-hot-toast";

export default function InfoPage() {
  const { user } = useAuth();
  const [isSending, setIsSending] = useState(false);

  React.useEffect(() => {
    document.title = `${PAGES.ACCOUNT.INFO.TITLE} | HieuShop`;
  }, []);

  const handleSendVerificationEmail = async () => {
    if (isSending) return;
    setIsSending(true);
    try {
      await identityApi.sendConfirmationEmail();
      toast.success(
        "Email xác thực đã được gửi! Vui lòng kiểm tra hộp thư của bạn.",
      );
    } catch (error) {
      console.error("Failed to send verification email:", error);
      toast.error("Gửi email xác thực thất bại. Vui lòng thử lại sau.");
    } finally {
      setIsSending(false);
    }
  };

  if (!user) {
    return (
      <MainLayout>
        <div className="flex justify-center items-center min-h-[60vh]">
          <div className="animate-pulse text-gray-500">Loading...</div>
        </div>
      </MainLayout>
    );
  }

  return (
    <MainLayout>
      <div className="max-w-4xl mx-auto px-4 py-8 sm:px-6 lg:px-8">
        <div className="bg-white dark:bg-gray-900 shadow rounded-lg overflow-hidden border border-gray-100 dark:border-gray-800">
          <div className="px-4 py-5 sm:px-6 bg-gray-50 dark:bg-gray-800/50 border-b border-gray-200 dark:border-gray-800">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-white flex items-center">
              <User className="w-5 h-5 mr-2 text-brand-600" />
              Thông tin tài khoản
            </h3>
            <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-gray-400">
              Chi tiết thông tin cá nhân và bảo mật.
            </p>
          </div>
          <div className="border-t border-gray-200 dark:border-gray-800 px-4 py-5 sm:p-0">
            <dl className="sm:divide-y sm:divide-gray-200 dark:sm:divide-gray-800">
              <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                <dt className="text-sm font-medium text-gray-500 dark:text-gray-400">
                  Email
                </dt>
                <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2 space-y-2">
                  <div className="flex items-center">
                    {user.email}
                    {user.emailConfirmed ? (
                      <span className="ml-2 inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-400">
                        <Shield className="w-3 h-3 mr-1" />
                        Đã xác thực
                      </span>
                    ) : (
                      <span className="ml-2 inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-yellow-100 text-yellow-800 dark:bg-yellow-900/30 dark:text-yellow-400">
                        <AlertTriangle className="w-3 h-3 mr-1" />
                        Chưa xác thực
                      </span>
                    )}
                  </div>
                  {!user.emailConfirmed && (
                    <button
                      onClick={handleSendVerificationEmail}
                      disabled={isSending}
                      className="inline-flex items-center px-3 py-1.5 border border-transparent text-xs font-medium rounded-md shadow-sm text-white bg-brand-600 hover:bg-brand-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-brand-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                    >
                      {isSending ? (
                        <span className="flex items-center">
                          <svg
                            className="animate-spin -ml-1 mr-2 h-3 w-3 text-white"
                            fill="none"
                            viewBox="0 0 24 24"
                          >
                            <circle
                              className="opacity-25"
                              cx="12"
                              cy="12"
                              r="10"
                              stroke="currentColor"
                              strokeWidth="4"
                            ></circle>
                            <path
                              className="opacity-75"
                              fill="currentColor"
                              d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                            ></path>
                          </svg>
                          Đang gửi...
                        </span>
                      ) : (
                        <span className="flex items-center">
                          <Send className="w-3 h-3 mr-1.5" />
                          Xác thực email ngay để sử dụng toàn bộ tính năng
                        </span>
                      )}
                    </button>
                  )}
                </dd>
              </div>
              <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                <dt className="text-sm font-medium text-gray-500 dark:text-gray-400">
                  Bảo mật
                </dt>
                <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2">
                  {user.hasPassword ? (
                    <Link
                      to={PAGES.ACCOUNT.CHANGE_PASSWORD.PATH}
                      className="inline-flex items-center text-brand-600 hover:text-brand-500 font-medium"
                    >
                      <Key className="w-4 h-4 mr-1" />
                      Đổi mật khẩu
                    </Link>
                  ) : (
                    <Link
                      to={PAGES.ACCOUNT.SET_PASSWORD.PATH}
                      className="inline-flex items-center text-brand-600 hover:text-brand-500 font-medium"
                    >
                      <Key className="w-4 h-4 mr-1" />
                      Thiết lập mật khẩu
                    </Link>
                  )}
                </dd>
              </div>
            </dl>
          </div>
        </div>
      </div>
    </MainLayout>
  );
}
