import React from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Link } from 'react-router-dom';
import MainLayout from '../layouts/MainLayout';
import { Mail, CheckCircle, ArrowLeft } from 'lucide-react';
import { identityApi } from '../api/identityApi';
import { parseApiError, type ApiError } from '../utils/error';
import { PAGES } from '../config/page';

const forgotPasswordSchema = z.object({
    email: z.email('Email không hợp lệ'),
});

type ForgotPasswordForm = z.infer<typeof forgotPasswordSchema>;

export default function ForgotPasswordPage() {
    React.useEffect(() => {
        document.title = `${PAGES.FORGOT_PASSWORD.title} | HieuShop`;
    }, []);
    const [isSubmitted, setIsSubmitted] = React.useState(false);
    const [error, setError] = React.useState<ApiError | null>(null);

    const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<ForgotPasswordForm>({
        resolver: zodResolver(forgotPasswordSchema),
    });

    const onSubmit = async (data: ForgotPasswordForm) => {
        try {
            setError(null);
            await identityApi.forgotPassword(data);
            setIsSubmitted(true);
        } catch (err: any) {
            console.error(err);
            setError(parseApiError(err, 'Có lỗi xảy ra. Vui lòng thử lại sau.'));
        }
    };

    return (
        <MainLayout>
            <div className="flex flex-col items-center justify-center min-h-[80vh] px-4">
                <div className="w-full max-w-md p-8 bg-white dark:bg-gray-900 rounded-2xl shadow-xl transition-colors duration-300 border border-gray-100 dark:border-gray-800">

                    {!isSubmitted ? (
                        <>
                            <div className="flex flex-col items-center mb-8">
                                <div className="p-3 bg-brand-100 dark:bg-brand-900/30 rounded-full mb-4">
                                    <Mail className="w-8 h-8 text-brand-600 dark:text-brand-400" />
                                </div>
                                <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Quên Mật Khẩu?</h1>
                                <p className="text-gray-500 dark:text-gray-400 mt-2 text-center">
                                    Đừng lo lắng! Nhập email của bạn và chúng tôi sẽ gửi hướng dẫn đặt lại mật khẩu.
                                </p>
                            </div>

                            {error && (
                                <div className="mb-6 p-4 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg text-red-600 dark:text-red-400 text-sm whitespace-pre-line">
                                    {error.title && <div className="font-bold mb-1 uppercase text-xs">{error.title}</div>}
                                    {error.message}
                                </div>
                            )}

                            <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                                <div>
                                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                        Email
                                    </label>
                                    <input
                                        {...register('email')}
                                        type="email"
                                        className="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 focus:border-transparent transition-colors outline-none"
                                        placeholder="name@example.com"
                                    />
                                    {errors.email && (
                                        <p className="mt-1 text-sm text-red-600 dark:text-red-400">{errors.email.message}</p>
                                    )}
                                </div>

                                <button
                                    type="submit"
                                    disabled={isSubmitting}
                                    className="w-full py-3 px-4 bg-brand-600 hover:bg-brand-700 text-white font-medium rounded-lg shadow-lg shadow-brand-500/30 transition-all transform hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed"
                                >
                                    {isSubmitting ? 'Đang gửi...' : 'Gửi yêu cầu'}
                                </button>
                            </form>

                            <div className="mt-8 text-center">
                                <Link to={PAGES.LOGIN.path} className="inline-flex items-center text-sm font-medium text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-white transition-colors">
                                    <ArrowLeft className="w-4 h-4 mr-2" />
                                    Quay lại đăng nhập
                                </Link>
                            </div>
                        </>
                    ) : (
                        <div className="flex flex-col items-center text-center">
                            <div className="p-3 bg-green-100 dark:bg-green-900/30 rounded-full mb-4">
                                <CheckCircle className="w-12 h-12 text-green-600 dark:text-green-400" />
                            </div>
                            <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">Đã gửi email!</h1>
                            <p className="text-gray-500 dark:text-gray-400 mb-8">
                                Nếu email của bạn tồn tại trong hệ thống, chúng tôi đã gửi hướng dẫn đặt lại mật khẩu đến email của bạn. Vui lòng kiểm tra hộp thư đến.
                            </p>
                            <Link to={PAGES.LOGIN.path} className="inline-flex items-center text-sm font-medium text-brand-600 hover:text-brand-500 dark:text-brand-400 transition-colors">
                                <ArrowLeft className="w-4 h-4 mr-2" />
                                Quay lại đăng nhập
                            </Link>
                        </div>
                    )}
                </div>
            </div>
        </MainLayout>
    );
}
