import React from 'react';
import { useAuth } from '../../context/AuthContext';
import MainLayout from '../../layouts/MainLayout';
import { PAGES } from '../../config/page';
import { Link } from 'react-router-dom';
import { User, Shield, Key } from 'lucide-react';

export default function InfoPage() {
    const { user } = useAuth();

    React.useEffect(() => {
        document.title = `${PAGES.ACCOUNT.INFO.TITLE} | HieuShop`;
    }, []);

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
                                <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2 flex items-center">
                                    {user.email}
                                    {user.isEmailConfirmed && (
                                        <span className="ml-2 inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-400">
                                            <Shield className="w-3 h-3 mr-1" />
                                            Đã xác thực
                                        </span>
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
