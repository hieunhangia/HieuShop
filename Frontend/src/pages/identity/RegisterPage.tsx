import React from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import MainLayout from '../../layouts/MainLayout';
import { UserPlus } from 'lucide-react';
import { identityApi } from '../../api/identityApi';
import { parseApiError, type ApiError } from '../../utils/error';
import { PAGES } from '../../config/page';

const registerSchema = z.object({
    email: z.email('Email không hợp lệ'),
    password: z.string().min(6, 'Mật khẩu phải có ít nhất 6 ký tự'),
    confirmPassword: z.string()
}).refine((data) => data.password === data.confirmPassword, {
    message: "Mật khẩu nhập lại không khớp",
    path: ["confirmPassword"],
});

type RegisterForm = z.infer<typeof registerSchema>;

export default function RegisterPage() {
    React.useEffect(() => {
        document.title = `${PAGES.IDENTITY.REGISTER.TITLE} | HieuShop`;
    }, []);
    const { register: registerUser, login } = useAuth();
    const [error, setError] = React.useState<ApiError | null>(null);
    const [isRegistrationSuccess, setIsRegistrationSuccess] = React.useState(false);
    const [registeredEmail, setRegisteredEmail] = React.useState('');
    const [isSendingEmail, setIsSendingEmail] = React.useState(false);
    const [emailSent, setEmailSent] = React.useState(false);

    const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<RegisterForm>({
        resolver: zodResolver(registerSchema),
    });

    const onSubmit = async (data: RegisterForm) => {
        try {
            setError(null);
            await registerUser({ email: data.email, password: data.password });

            await login({ email: data.email, password: data.password });

            setRegisteredEmail(data.email);
            setIsRegistrationSuccess(true);
        } catch (err: any) {
            console.error(err);
            setError(parseApiError(err, 'Đăng ký thất bại. Vui lòng thử lại.'));
        }
    };

    const handleSendConfirmationEmail = async () => {
        try {
            setIsSendingEmail(true);
            await identityApi.sendConfirmationEmail();
            setEmailSent(true);
        } catch (err) {
            console.error(err);
            // Optionally handle error
        } finally {
            setIsSendingEmail(false);
        }
    };

    if (isRegistrationSuccess) {
        return (
            <MainLayout>
                <div className="flex flex-col items-center justify-center min-h-[80vh] px-4">
                    <div className="w-full max-w-md p-8 bg-white dark:bg-gray-900 rounded-2xl shadow-xl border border-gray-100 dark:border-gray-800 text-center">
                        <div className="p-3 bg-green-100 dark:bg-green-900/30 rounded-full mb-4 inline-flex">
                            <UserPlus className="w-8 h-8 text-green-600 dark:text-green-400" />
                        </div>
                        <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">Đăng Ký Thành Công!</h1>
                        <p className="text-gray-500 dark:text-gray-400 mb-6">
                            Tài khoản của bạn đã được tạo. Vui lòng xác thực email <span className="font-semibold text-gray-900 dark:text-white">{registeredEmail}</span> để hoàn tất.
                        </p>

                        {!emailSent ? (
                            <button
                                onClick={handleSendConfirmationEmail}
                                disabled={isSendingEmail}
                                className="w-full py-3 px-4 bg-brand-600 hover:bg-brand-700 text-white font-medium rounded-lg shadow-lg shadow-brand-500/30 transition-all transform hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed"
                            >
                                {isSendingEmail ? 'Đang gửi...' : 'Gửi Email Xác Thực'}
                            </button>
                        ) : (
                            <div className="p-4 bg-green-50 dark:bg-green-900/20 text-green-700 dark:text-green-300 rounded-lg">
                                Email xác thực đã được gửi! Vui lòng kiểm tra hộp thư đến của bạn.
                            </div>
                        )}

                        <div className="mt-6 text-sm">
                            <Link to={PAGES.HOME.PATH} className="text-brand-600 hover:underline">
                                Về trang chủ
                            </Link>
                        </div>
                    </div>
                </div>
            </MainLayout>
        );
    }

    return (
        <MainLayout>
            <div className="flex flex-col items-center justify-center min-h-[80vh] px-4">
                <div className="w-full max-w-md p-8 bg-white dark:bg-gray-900 rounded-2xl shadow-xl transition-colors duration-300 border border-gray-100 dark:border-gray-800">
                    <div className="flex flex-col items-center mb-8">
                        <div className="p-3 bg-brand-100 dark:bg-brand-900/30 rounded-full mb-4">
                            <UserPlus className="w-8 h-8 text-brand-600 dark:text-brand-400" />
                        </div>
                        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Đăng Ký</h1>
                        <p className="text-gray-500 dark:text-gray-400 mt-2 text-center">
                            Tạo tài khoản mới để trải nghiệm mua sắm tốt nhất.
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
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Mật khẩu
                            </label>
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

                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                                Nhập lại mật khẩu
                            </label>
                            <input
                                {...register('confirmPassword')}
                                type="password"
                                className="w-full px-4 py-3 rounded-lg border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-brand-500 focus:border-transparent transition-colors outline-none"
                                placeholder="••••••••"
                            />
                            {errors.confirmPassword && (
                                <p className="mt-1 text-sm text-red-600 dark:text-red-400">{errors.confirmPassword.message}</p>
                            )}
                        </div>

                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className="w-full py-3 px-4 bg-brand-600 hover:bg-brand-700 text-white font-medium rounded-lg shadow-lg shadow-brand-500/30 transition-all transform hover:-translate-y-0.5 active:translate-y-0 disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            {isSubmitting ? 'Đang xử lý...' : 'Đăng Ký'}
                        </button>
                    </form>

                    <div className="mt-8 text-center text-sm text-gray-600 dark:text-gray-400">
                        Đã có tài khoản?{' '}
                        <Link to={PAGES.IDENTITY.LOGIN.PATH} className="font-medium text-brand-600 hover:text-brand-500 dark:text-brand-400">
                            Đăng nhập ngay
                        </Link>
                    </div>
                </div>
            </div>
        </MainLayout>
    );
}

