import React from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import MainLayout from '../../layouts/MainLayout';
import { LogIn } from 'lucide-react';
import { parseApiError, type ApiError } from '../../utils/error';
import { PAGES } from '../../config/page';
import GoogleLoginButton from '../../components/identity/GoogleLoginButton';

const loginSchema = z.object({
    email: z.email('Email không hợp lệ'),
    password: z.string().min(6, 'Mật khẩu phải có ít nhất 6 ký tự'),
});

type LoginForm = z.infer<typeof loginSchema>;

export default function LoginPage() {
    const { login } = useAuth();
    const navigate = useNavigate();

    React.useEffect(() => {
        document.title = `${PAGES.IDENTITY.LOGIN.TITLE} | HieuShop`;
    }, []);
    const [error, setError] = React.useState<ApiError | null>(null);

    const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<LoginForm>({
        resolver: zodResolver(loginSchema),
    });

    const onSubmit = async (data: LoginForm) => {
        try {
            setError(null);
            await login(data);
            navigate(PAGES.HOME.PATH);
        } catch (err: any) {
            console.error(err);
            setError(parseApiError(err, 'Đăng nhập thất bại. Vui lòng kiểm tra lại email và mật khẩu.'));
        }
    };

    return (
        <MainLayout>
            <div className="flex flex-col items-center justify-center min-h-[80vh] px-4">
                <div className="w-full max-w-md p-8 bg-white dark:bg-gray-900 rounded-2xl shadow-xl transition-colors duration-300 border border-gray-100 dark:border-gray-800">
                    <div className="flex flex-col items-center mb-8">
                        <div className="p-3 bg-brand-100 dark:bg-brand-900/30 rounded-full mb-4">
                            <LogIn className="w-8 h-8 text-brand-600 dark:text-brand-400" />
                        </div>
                        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Đăng Nhập</h1>
                        <p className="text-gray-500 dark:text-gray-400 mt-2 text-center">
                            Chào mừng trở lại! Vui lòng nhập thông tin của bạn.
                        </p>
                    </div>

                    {error && (
                        <div className="mb-6 p-4 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg text-red-600 dark:text-red-400 text-sm whitespace-pre-line">
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

                        <div>
                            <div className="flex justify-between items-center mb-2">
                                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                                    Mật khẩu
                                </label>
                                <Link
                                    to={PAGES.IDENTITY.FORGOT_PASSWORD.PATH}
                                    className="text-sm font-medium text-brand-600 hover:text-brand-500 dark:text-brand-400"
                                >
                                    Quên mật khẩu?
                                </Link>
                            </div>
                            <input
                                {...register('password')}
                                type="password"
                                className="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 focus:border-transparent transition-colors outline-none"
                                placeholder="••••••••"
                            />
                            {errors.password && (
                                <p className="mt-1 text-sm text-red-600 dark:text-red-400">{errors.password.message}</p>
                            )}
                        </div>

                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className="w-full py-3 px-4 bg-brand-600 hover:bg-brand-700 text-white font-medium rounded-lg shadow-lg shadow-brand-500/30 transition-all transform hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            {isSubmitting ? 'Đang xử lý...' : 'Đăng Nhập'}
                        </button>
                    </form>

                    <div className="mt-6">
                        <div className="relative">
                            <div className="absolute inset-0 flex items-center">
                                <div className="w-full border-t border-gray-300 dark:border-gray-700"></div>
                            </div>
                            <div className="relative flex justify-center text-sm">
                                <span className="px-2 bg-white dark:bg-gray-900 text-gray-500">Hoặc tiếp tục với</span>
                            </div>
                        </div>

                        <div className="mt-6">
                            <GoogleLoginButton />
                        </div>
                    </div>

                    <div className="mt-8 text-center text-sm text-gray-600 dark:text-gray-400">
                        Chưa có tài khoản?{' '}
                        <Link to={PAGES.IDENTITY.REGISTER.PATH} className="font-medium text-brand-600 hover:text-brand-500 dark:text-brand-400">
                            Đăng ký ngay
                        </Link>
                    </div>
                </div>
            </div>
        </MainLayout>
    );
}
