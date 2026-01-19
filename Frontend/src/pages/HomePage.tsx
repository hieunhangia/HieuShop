import React from 'react';
import backgroundShop from '../assets/background_shop.webp';
import MainLayout from '../layouts/MainLayout';
import { PAGES } from '../config/page';

export default function HomePage() {
    React.useEffect(() => {
        document.title = `${PAGES.HOME.TITLE} | HieuShop`;
    }, []);
    return (
        <MainLayout>
            <section className="relative isolate px-4 sm:px-6 lg:px-8 py-20 lg:py-32 overflow-hidden">
                <div className="absolute inset-0">
                    <img
                        src={backgroundShop}
                        alt="Background"
                        className="w-full h-full object-cover object-center"
                    />
                    <div className="absolute inset-0 bg-white/50 dark:bg-gray-900/50" />
                </div>

                <div className="relative z-10 w-fit mx-auto text-center bg-white/20 dark:bg-black/20 backdrop-blur-sm rounded-3xl p-6 sm:p-8 shadow-2xl ring-1 ring-gray-900/5 inset-0">
                    <h1 className="text-4xl md:text-6xl font-extrabold tracking-tight text-gray-900 dark:text-white mb-6">
                        HieuShop
                    </h1>
                    <p className="mt-4 text-xl text-gray-600 dark:text-gray-300 max-w-2xl mx-auto">
                        Khám phá những sản phẩm công nghệ đỉnh cao, chính hãng với mức giá tốt nhất thị trường. Trải nghiệm mua sắm thông minh ngay hôm nay.
                    </p>
                </div>
            </section>
        </MainLayout>
    );
}
