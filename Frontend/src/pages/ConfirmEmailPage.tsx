import React, { useEffect, useState, useRef } from 'react';
import { useSearchParams, Link } from 'react-router-dom';
import { CheckCircle, XCircle, Loader, ArrowLeft, Send } from 'lucide-react';
import MainLayout from '../layouts/MainLayout';
import { identityApi } from '../api/identityApi';
import { useAuth } from '../context/AuthContext';
import { parseApiError, type ApiError } from '../utils/error';
import { PAGES } from '../config/page';

export default function ConfirmEmailPage() {
    const [searchParams] = useSearchParams();
    const { isAuthenticated } = useAuth();

    React.useEffect(() => {
        document.title = `${PAGES.CONFIRM_EMAIL.title} | HieuShop`;
    }, []);


    const [status, setStatus] = useState<'loading' | 'success' | 'error'>('loading');
    const [message, setMessage] = useState<ApiError | string>('Đang xác thực email...');
    const [isResending, setIsResending] = useState(false);

    const [resendStatus, setResendStatus] = useState<{ type: 'success' | 'error', message: ApiError | string } | null>(null);

    const isRun = useRef(false);

    useEffect(() => {
        if (isRun.current) return;
        isRun.current = true;

        const confirmEmail = async () => {
            const email = searchParams.get('email');
            const code = searchParams.get('code');

            if (!email || !code) {
                setStatus('error');
                setMessage('Liên kết xác thực không hợp lệ.');
                return;
            }

            try {
                await identityApi.confirmEmail({ email, code });
                setStatus('success');
                setMessage('Email của bạn đã được xác thực thành công!');
            } catch (error: any) {
                console.error(error);
                setStatus('error');
                setMessage(parseApiError(error, 'Xác thực email thất bại. Vui lòng thử lại.'));
            }
        };

        confirmEmail();
    }, [searchParams]);

    const handleResendEmail = async () => {
        try {
            setIsResending(true);
            setResendStatus(null);
            await identityApi.sendConfirmationEmail();
            setResendStatus({ type: 'success', message: 'Email xác thực mới đã được gửi! Vui lòng kiểm tra hộp thư đến.' });
        } catch (error) {
            console.error(error);
            setResendStatus({ type: 'error', message: parseApiError(error, 'Gửi email thất bại. Vui lòng thử lại sau.') });
        } finally {
            setIsResending(false);
        }
    };

    return (
        <MainLayout>
            <div className="flex flex-col items-center justify-center min-h-[60vh] px-4">
                <div className="w-full max-w-md p-8 bg-white dark:bg-gray-900 rounded-2xl shadow-xl border border-gray-100 dark:border-gray-800 text-center">
                    {status === 'loading' && (
                        <div className="flex flex-col items-center">
                            <Loader className="w-12 h-12 text-brand-600 animate-spin mb-4" />
                            <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">Đang xử lý</h1>
                            <div className="text-gray-500 dark:text-gray-400 whitespace-pre-line">
                                {typeof message === 'string' ? message : (
                                    <>
                                        {message.title && <div className="font-bold mb-1 uppercase text-xs text-brand-600">{message.title}</div>}
                                        {message.message}
                                    </>
                                )}
                            </div>
                        </div>
                    )}

                    {status === 'success' && (
                        <div className="flex flex-col items-center">
                            <div className="p-3 bg-green-100 dark:bg-green-900/30 rounded-full mb-4">
                                <CheckCircle className="w-12 h-12 text-green-600 dark:text-green-400" />
                            </div>
                            <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">Thành Công!</h1>
                            <div className="text-gray-500 dark:text-gray-400 mb-8 whitespace-pre-line">
                                {typeof message === 'string' ? message : (
                                    <>
                                        {message.title && <div className="font-bold mb-1 uppercase text-xs text-green-600">{message.title}</div>}
                                        {message.message}
                                    </>
                                )}
                            </div>

                            {isAuthenticated ? (
                                <Link
                                    to={PAGES.HOME.path}
                                    className="inline-flex items-center justify-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-brand-600 hover:bg-brand-700 transition"
                                >
                                    Về trang chủ
                                </Link>
                            ) : (
                                <Link
                                    to={PAGES.LOGIN.path}
                                    className="inline-flex items-center justify-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-brand-600 hover:bg-brand-700 transition"
                                >
                                    Đăng nhập ngay
                                </Link>
                            )}
                        </div>
                    )}

                    {status === 'error' && (
                        <div className="flex flex-col items-center">
                            <div className="p-3 bg-red-100 dark:bg-red-900/30 rounded-full mb-4">
                                <XCircle className="w-12 h-12 text-red-600 dark:text-red-400" />
                            </div>
                            <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">Đã có lỗi xảy ra</h1>
                            <div className="text-gray-500 dark:text-gray-400 mb-8 whitespace-pre-line">
                                {typeof message === 'string' ? message : (
                                    <>
                                        {message.title && <div className="font-bold mb-1 uppercase text-xs text-red-600 dark:text-red-400">{message.title}</div>}
                                        {message.message}
                                    </>
                                )}
                            </div>

                            {isAuthenticated ? (
                                <div className="flex flex-col items-center w-full">
                                    <button
                                        onClick={handleResendEmail}
                                        disabled={isResending}
                                        className="inline-flex items-center justify-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-brand-600 hover:bg-brand-700 transition disabled:opacity-50"
                                    >
                                        {isResending ? (
                                            <>
                                                <Loader className="w-4 h-4 mr-2 animate-spin" />
                                                Đang gửi...
                                            </>
                                        ) : (
                                            <>
                                                <Send className="w-4 h-4 mr-2" />
                                                Gửi lại email xác thực
                                            </>
                                        )}
                                    </button>

                                    {resendStatus && (
                                        <div className={`mt-4 p-3 rounded-lg text-sm w-full whitespace-pre-line ${resendStatus.type === 'success'
                                            ? 'bg-green-50 dark:bg-green-900/20 text-green-700 dark:text-green-300'
                                            : 'bg-red-50 dark:bg-red-900/20 text-red-700 dark:text-red-300'
                                            }`}>
                                            {typeof resendStatus.message === 'string' ? resendStatus.message : (
                                                <>
                                                    {resendStatus.message.title && <div className="font-bold mb-1 uppercase text-xs">{resendStatus.message.title}</div>}
                                                    {resendStatus.message.message}
                                                </>
                                            )}
                                        </div>
                                    )}
                                </div>
                            ) : (
                                <Link to={PAGES.LOGIN.path} className="inline-flex items-center text-sm font-medium text-brand-600 hover:text-brand-500 dark:text-brand-400 transition-colors">
                                    <ArrowLeft className="w-4 h-4 mr-2" />
                                    Quay lại đăng nhập
                                </Link>
                            )}
                        </div>
                    )}
                </div>
            </div>
        </MainLayout>
    );
}
