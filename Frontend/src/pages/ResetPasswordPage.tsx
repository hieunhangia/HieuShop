import React, { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useSearchParams, Link } from 'react-router-dom';
import { Lock, CheckCircle, ArrowLeft, Loader } from 'lucide-react';
import MainLayout from '../layouts/MainLayout';
import { identityApi } from '../api/identityApi';
import { parseApiError, type ApiError } from '../utils/error';
import { PAGES } from '../config/page';

const resetPasswordSchema = z.object({
    password: z.string().min(6, 'Mật khẩu phải có ít nhất 6 ký tự'),
    confirmPassword: z.string()
}).refine((data) => data.password === data.confirmPassword, {
    message: "Mật khẩu nhập lại không khớp",
    path: ["confirmPassword"],
});

type ResetPasswordForm = z.infer<typeof resetPasswordSchema>;

export default function ResetPasswordPage() {
    const [searchParams] = useSearchParams();

    React.useEffect(() => {
        document.title = `${PAGES.RESET_PASSWORD.title} | HieuShop`;
    }, []);

    const [status, setStatus] = useState<'idle' | 'success' | 'error'>('idle');
    const [errorMessage, setErrorMessage] = useState<ApiError | string>('');

    const [isValidating, setIsValidating] = useState(true);

    // Extract email and code from URL
    const email = searchParams.get('email');
    const code = searchParams.get('code');

    const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<ResetPasswordForm>({
        resolver: zodResolver(resetPasswordSchema),
    });

    useEffect(() => {
        const validateRequest = async () => {
            if (!email || !code) {
                setStatus('error');
                setErrorMessage('Liên kết đặt lại mật khẩu không hợp lệ hoặc bị thiếu thông tin.');
                setIsValidating(false);
                return;
            }

            try {
                await identityApi.validateResetPassword({ email, resetCode: code });
                setIsValidating(false);
            } catch (error: any) {
                console.error(error);
                setStatus('error');
                setErrorMessage(parseApiError(error, 'Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.'));
                setIsValidating(false);
            }
        };

        validateRequest();
    }, [email, code]);

    const onSubmit = async (data: ResetPasswordForm) => {
        if (!email || !code) return;

        try {
            await identityApi.resetPassword({
                email,
                resetCode: code,
                newPassword: data.password
            });
            setStatus('success');
        } catch (error: any) {
            console.error(error);
            setStatus('error');
            setErrorMessage(parseApiError(error, 'Đặt lại mật khẩu thất bại. Vui lòng thử lại hoặc yêu cầu liên kết mới.'));
        }
    };

    if (isValidating) {
        return (
            <MainLayout>
                <div className="flex flex-col items-center justify-center min-h-[60vh] px-4">
                    <Loader className="w-12 h-12 text-brand-600 animate-spin mb-4" />
                    <h1 className="text-xl font-medium text-gray-900 dark:text-white">Đang kiểm tra liên kết...</h1>
                </div>
            </MainLayout>
        );
    }

    if (status === 'success') {
        return (
            <MainLayout>
                <div className="flex flex-col items-center justify-center min-h-[60vh] px-4">
                    <div className="w-full max-w-md p-8 bg-white dark:bg-gray-900 rounded-2xl shadow-xl border border-gray-100 dark:border-gray-800 text-center">
                        <div className="p-3 bg-green-100 dark:bg-green-900/30 rounded-full mb-4 inline-flex">
                            <CheckCircle className="w-12 h-12 text-green-600 dark:text-green-400" />
                        </div>
                        <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">Thành Công!</h1>
                        <p className="text-gray-500 dark:text-gray-400 mb-8">
                            Mật khẩu của bạn đã được đặt lại thành công. Bạn có thể đăng nhập ngay bây giờ.
                        </p>
                        <Link
                            to={PAGES.LOGIN.path}
                            className="inline-flex items-center justify-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-brand-600 hover:bg-brand-700 transition w-full"
                        >
                            Đăng nhập ngay
                        </Link>
                    </div>
                </div>
            </MainLayout>
        );
    }

    const isInvalidLinkError = (err: string | ApiError) => {
        if (typeof err === 'string') return err.includes('không hợp lệ');
        return err.message.includes('không hợp lệ') || (err.title && err.title.includes('không hợp lệ'));
    };

    const isSubmissionError = (err: string | ApiError) => {
        if (typeof err === 'string') return err.includes('thất bại');
        return err.message.includes('thất bại') || (err.title && err.title.includes('thất bại'));
    };

    if (status === 'error' && isInvalidLinkError(errorMessage)) {
        return (
            <MainLayout>
                <div className="flex flex-col items-center justify-center min-h-[60vh] px-4">
                    <div className="w-full max-w-md p-8 bg-white dark:bg-gray-900 rounded-2xl shadow-xl border border-gray-100 dark:border-gray-800 text-center">
                        <div className="p-3 bg-red-100 dark:bg-red-900/30 rounded-full mb-4 inline-flex">
                            <CheckCircle className="w-12 h-12 text-red-600 dark:text-red-400 rotate-45 transform" />
                        </div>
                        <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">Liên Kết Không Hợp Lệ</h1>
                        <div className="text-gray-500 dark:text-gray-400 mb-8 whitespace-pre-line">
                            {typeof errorMessage === 'string' ? errorMessage : (
                                <>
                                    {errorMessage.message}
                                </>
                            )}
                        </div>
                        <Link
                            to={PAGES.FORGOT_PASSWORD.path}
                            className="inline-flex items-center justify-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-brand-600 hover:bg-brand-700 transition w-full"
                        >
                            Yêu cầu liên kết mới
                        </Link>
                    </div>
                </div>
            </MainLayout>
        );
    }

    return (
        <MainLayout>
            <div className="flex flex-col items-center justify-center min-h-[70vh] px-4">
                <div className="w-full max-w-md p-8 bg-white dark:bg-gray-900 rounded-2xl shadow-xl border border-gray-100 dark:border-gray-800 transition-colors duration-300">
                    <div className="text-center mb-8">
                        <div className="p-3 bg-brand-100 dark:bg-brand-900/30 rounded-full mb-4 inline-flex">
                            <Lock className="w-8 h-8 text-brand-600 dark:text-brand-400" />
                        </div>
                        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Đặt Lại Mật Khẩu</h1>
                        <p className="text-gray-500 dark:text-gray-400 mt-2">
                            Vui lòng nhập mật khẩu mới cho tài khoản của bạn.
                        </p>
                    </div>

                    {status === 'error' && (
                        <div className="mb-6 p-4 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg text-red-600 dark:text-red-400 text-sm whitespace-pre-line">
                            {typeof errorMessage === 'string' ? errorMessage : (
                                <>
                                    {errorMessage.title && <div className="font-bold mb-1 uppercase text-xs">{errorMessage.title}</div>}
                                    {errorMessage.message}
                                </>
                            )}
                        </div>
                    )}

                    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Mật khẩu mới
                            </label>
                            <input
                                type="password"
                                {...register('password')}
                                className="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 focus:border-transparent transition-colors outline-none"
                                placeholder="••••••••"
                                disabled={status === 'error' && !isSubmissionError(errorMessage)} // Disable only if fundamental link error
                            />
                            {errors.password && (
                                <p className="mt-1 text-sm text-red-600 dark:text-red-400">{errors.password.message}</p>
                            )}
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Nhập lại mật khẩu mới
                            </label>
                            <input
                                type="password"
                                {...register('confirmPassword')}
                                className="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 focus:border-transparent transition-colors outline-none"
                                placeholder="••••••••"
                                disabled={status === 'error' && !isSubmissionError(errorMessage)}
                            />
                            {errors.confirmPassword && (
                                <p className="mt-1 text-sm text-red-600 dark:text-red-400">{errors.confirmPassword.message}</p>
                            )}
                        </div>

                        <button
                            type="submit"
                            disabled={isSubmitting || (status === 'error' && !isSubmissionError(errorMessage))}
                            className="w-full py-3 px-4 bg-brand-600 hover:bg-brand-700 text-white font-medium rounded-lg shadow-lg shadow-brand-500/30 transition-all transform hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center"
                        >
                            {isSubmitting ? (
                                <>
                                    <Loader className="w-5 h-5 mr-2 animate-spin" />
                                    Đang xử lý...
                                </>
                            ) : (
                                'Đặt lại mật khẩu'
                            )}
                        </button>
                    </form>

                    <div className="mt-6 text-center">
                        <Link to={PAGES.LOGIN.path} className="inline-flex items-center text-sm font-medium text-brand-600 hover:text-brand-500 dark:text-brand-400 transition-colors">
                            <ArrowLeft className="w-4 h-4 mr-2" />
                            Quay lại đăng nhập
                        </Link>
                    </div>
                </div>
            </div>
        </MainLayout>
    );
}
