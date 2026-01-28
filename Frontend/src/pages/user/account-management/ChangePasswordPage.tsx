import React from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Link, useNavigate } from "react-router-dom";
import MainLayout from "../../../layouts/MainLayout";
import { Key, ArrowLeft, Loader } from "lucide-react";
import { accountManagementApi } from "../../../api/accountManagementApi";
import { parseApiError, type ApiError } from "../../../utils/error";
import { PAGES } from "../../../config/page";

const changePasswordSchema = z
  .object({
    oldPassword: z.string().min(1, "Vui lòng nhập mật khẩu hiện tại"),
    newPassword: z.string().min(6, "Mật khẩu mới phải có ít nhất 6 ký tự"),
    confirmNewPassword: z.string(),
  })
  .refine((data) => data.newPassword === data.confirmNewPassword, {
    message: "Mật khẩu mới không khớp",
    path: ["confirmNewPassword"],
  });

type ChangePasswordForm = z.infer<typeof changePasswordSchema>;

export default function ChangePasswordPage() {
  React.useEffect(() => {
    document.title = `${PAGES.USER.ACCOUNT_MANAGEMENT.CHANGE_PASSWORD.TITLE} | HieuShop`;
  }, []);

  const navigate = useNavigate();
  const [error, setError] = React.useState<ApiError | null>(null);
  const [successMessage, setSuccessMessage] = React.useState("");

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<ChangePasswordForm>({
    resolver: zodResolver(changePasswordSchema),
  });

  const onSubmit = async (data: ChangePasswordForm) => {
    try {
      setError(null);
      setSuccessMessage("");
      await accountManagementApi.changePassword(data);
      setSuccessMessage("Đổi mật khẩu thành công!");
      setTimeout(() => {
        navigate(PAGES.USER.ACCOUNT_MANAGEMENT.INFO.PATH);
      }, 2000);
    } catch (err: any) {
      console.error(err);
      setError(
        parseApiError(
          err,
          "Đổi mật khẩu thất bại. Vui lòng kiểm tra lại mật khẩu hiện tại.",
        ),
      );
    }
  };

  return (
    <MainLayout>
      <div className="flex flex-col items-center justify-center min-h-[70vh] px-4">
        <div className="w-full max-w-md p-8 bg-white dark:bg-gray-900 rounded-2xl shadow-xl border border-gray-100 dark:border-gray-800">
          <div className="text-center mb-8">
            <div className="p-3 bg-brand-100 dark:bg-brand-900/30 rounded-full mb-4 inline-flex">
              <Key className="w-8 h-8 text-brand-600 dark:text-brand-400" />
            </div>
            <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
              Đổi Mật Khẩu
            </h1>
          </div>

          {error && (
            <div className="mb-6 p-4 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg text-red-600 dark:text-red-400 text-sm whitespace-pre-line">
              {error.message}
            </div>
          )}

          {successMessage && (
            <div className="mb-6 p-4 bg-green-50 dark:bg-green-900/20 border border-green-200 dark:border-green-800 rounded-lg text-green-600 dark:text-green-400 text-sm font-medium text-center">
              {successMessage}
            </div>
          )}

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Mật khẩu cũ
              </label>
              <input
                type="password"
                {...register("oldPassword")}
                className="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 focus:border-transparent transition-colors outline-none"
              />
              {errors.oldPassword && (
                <p className="mt-1 text-sm text-red-600 dark:text-red-400">
                  {errors.oldPassword.message}
                </p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Mật khẩu mới
              </label>
              <input
                type="password"
                {...register("newPassword")}
                className="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 focus:border-transparent transition-colors outline-none"
              />
              {errors.newPassword && (
                <p className="mt-1 text-sm text-red-600 dark:text-red-400">
                  {errors.newPassword.message}
                </p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Nhập lại mật khẩu mới
              </label>
              <input
                type="password"
                {...register("confirmNewPassword")}
                className="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 focus:border-transparent transition-colors outline-none"
              />
              {errors.confirmNewPassword && (
                <p className="mt-1 text-sm text-red-600 dark:text-red-400">
                  {errors.confirmNewPassword.message}
                </p>
              )}
            </div>

            <button
              type="submit"
              disabled={isSubmitting}
              className="w-full py-3 px-4 bg-brand-600 hover:bg-brand-700 text-white font-medium rounded-lg shadow-lg shadow-brand-500/30 transition-all transform hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
            >
              {isSubmitting ? (
                <>
                  <Loader className="w-5 h-5 mr-2 animate-spin" />
                  Đang xử lý...
                </>
              ) : (
                "Lưu thay đổi"
              )}
            </button>
          </form>

          <div className="mt-6 text-center">
            <Link
              to={PAGES.USER.ACCOUNT_MANAGEMENT.INFO.PATH}
              className="inline-flex items-center text-sm font-medium text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-white transition-colors"
            >
              <ArrowLeft className="w-4 h-4 mr-2" />
              Quay lại
            </Link>
          </div>
        </div>
      </div>
    </MainLayout>
  );
}
