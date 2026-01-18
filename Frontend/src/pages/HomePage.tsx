import { Star, Zap, Shield, Truck } from 'lucide-react';
import React from 'react';
import { Link } from 'react-router-dom';
import backgroundShop from '../assets/background_shop.webp';
import MainLayout from '../layouts/MainLayout';
import { PAGES } from '../config/page';

const MOCK_PRODUCTS = [
    {
        id: 1,
        name: "MacBook Pro M3 Max",
        price: "49.990.000₫",
        rating: 5,
        image: "https://images.unsplash.com/photo-1517336714731-489689fd1ca4?auto=format&fit=crop&q=80&w=1000",
        category: "Laptop"
    },
    {
        id: 2,
        name: "Sony WH-1000XM5",
        price: "8.490.000₫",
        rating: 4.8,
        image: "https://images.unsplash.com/photo-1618366712010-f4ae9c647dcb?auto=format&fit=crop&q=80&w=1000",
        category: "Audio"
    },
    {
        id: 3,
        name: "iPhone 15 Pro Max",
        price: "34.990.000₫",
        rating: 4.9,
        image: "https://images.unsplash.com/photo-1696446701796-da61225697cc?auto=format&fit=crop&q=80&w=1000",
        category: "Smartphone"
    },
    {
        id: 4,
        name: "iPad Pro M4",
        price: "28.990.000₫",
        rating: 4.7,
        image: "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?auto=format&fit=crop&q=80&w=1000",
        category: "Tablet"
    }
];

export default function HomePage() {
    React.useEffect(() => {
        document.title = `${PAGES.HOME.title} | HieuShop`;
    }, []);
    return (
        <MainLayout>
            {/* Hero Section */}
            <section className="relative isolate px-4 sm:px-6 lg:px-8 py-20 lg:py-32 overflow-hidden">
                <div className="absolute inset-0">
                    <img
                        src={backgroundShop}
                        alt="Background"
                        className="w-full h-full object-cover object-center"
                    />
                    <div className="absolute inset-0 bg-white/50 dark:bg-gray-900/50" />
                </div>

                <div className="relative z-10 max-w-7xl mx-auto text-center">
                    <h1 className="text-4xl md:text-6xl font-extrabold tracking-tight text-gray-900 dark:text-white mb-6">
                        Công Nghệ Dẫn Đầu <br />
                        <span className="text-transparent bg-clip-text bg-gradient-to-r from-brand-600 to-indigo-600">
                            Kiến Tạo Tương Lai
                        </span>
                    </h1>
                    <p className="mt-4 text-xl text-gray-600 dark:text-gray-300 max-w-2xl mx-auto">
                        Khám phá những sản phẩm công nghệ đỉnh cao, chính hãng với mức giá tốt nhất thị trường. Trải nghiệm mua sắm thông minh ngay hôm nay.
                    </p>
                    <div className="mt-10 flex justify-center gap-x-6">
                        <Link
                            to={PAGES.PRODUCTS.path}
                            className="rounded-full bg-brand-600 px-8 py-3.5 text-base font-semibold text-white shadow-sm hover:bg-brand-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-brand-600 transition-all transform hover:-translate-y-1"
                        >
                            Mua Sắm Ngay
                        </Link>
                        <Link
                            to={PAGES.ABOUT.path}
                            className="rounded-full px-8 py-3.5 text-base font-semibold text-gray-900 dark:text-white ring-1 ring-inset ring-gray-300 dark:ring-gray-700 hover:bg-gray-50 dark:hover:bg-gray-800 transition-all"
                        >
                            Tìm Hiểu Thêm
                        </Link>
                    </div>
                </div>
            </section>

            {/* Features */}
            <section className="py-16 bg-white dark:bg-gray-900/50">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                        <div className="flex flex-col items-center text-center p-6 bg-gray-50 dark:bg-gray-800/50 rounded-2xl">
                            <div className="p-3 bg-blue-100 dark:bg-blue-900/30 rounded-full mb-4">
                                <Zap className="w-6 h-6 text-blue-600 dark:text-blue-400" />
                            </div>
                            <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">Giao Hàng Hỏa Tốc</h3>
                            <p className="text-gray-500 dark:text-gray-400">Nhận hàng trong 2h tại nội thành Hà Nội & TP.HCM.</p>
                        </div>
                        <div className="flex flex-col items-center text-center p-6 bg-gray-50 dark:bg-gray-800/50 rounded-2xl">
                            <div className="p-3 bg-green-100 dark:bg-green-900/30 rounded-full mb-4">
                                <Shield className="w-6 h-6 text-green-600 dark:text-green-400" />
                            </div>
                            <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">Bảo Hành Chính Hãng</h3>
                            <p className="text-gray-500 dark:text-gray-400">Cam kết 100% sản phẩm chính hãng, bảo hành điện tử tiện lợi.</p>
                        </div>
                        <div className="flex flex-col items-center text-center p-6 bg-gray-50 dark:bg-gray-800/50 rounded-2xl">
                            <div className="p-3 bg-purple-100 dark:bg-purple-900/30 rounded-full mb-4">
                                <Truck className="w-6 h-6 text-purple-600 dark:text-purple-400" />
                            </div>
                            <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">Đổi Trả Dễ Dàng</h3>
                            <p className="text-gray-500 dark:text-gray-400">Hỗ trợ đổi mới trong 30 ngày nếu có lỗi từ nhà sản xuất.</p>
                        </div>
                    </div>
                </div>
            </section>

            {/* Featured Products */}
            <section className="py-16 px-4 sm:px-6 lg:px-8">
                <div className="max-w-7xl mx-auto">
                    <div className="flex justify-between items-end mb-8">
                        <div>
                            <h2 className="text-3xl font-bold text-gray-900 dark:text-white">Sản Phẩm Nổi Bật</h2>
                            <p className="mt-2 text-gray-500 dark:text-gray-400">Những lựa chọn được yêu thích nhất tuần qua</p>
                        </div>
                        <Link to={PAGES.PRODUCTS.path} className="hidden md:block text-brand-600 hover:text-brand-500 font-medium hover:underline">
                            Xem tất cả &rarr;
                        </Link>
                    </div>

                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
                        {MOCK_PRODUCTS.map((product) => (
                            <div key={product.id} className="group relative bg-white dark:bg-gray-900 rounded-2xl overflow-hidden border border-gray-100 dark:border-gray-800 shadow-sm hover:shadow-xl transition-all duration-300">
                                <div className="aspect-square w-full overflow-hidden bg-gray-200 group-hover:opacity-90 transition-opacity">
                                    <img
                                        src={product.image}
                                        alt={product.name}
                                        className="h-full w-full object-cover object-center transform group-hover:scale-105 transition-transform duration-500"
                                    />
                                    {/* Overlay Action */}
                                    <div className="absolute inset-0 flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity duration-300">
                                        <button className="bg-white/90 dark:bg-gray-900/90 backdrop-blur text-gray-900 dark:text-white px-6 py-3 rounded-full font-medium shadow-lg hover:bg-brand-600 hover:text-white dark:hover:bg-brand-600 transition-all transform translate-y-4 group-hover:translate-y-0">
                                            Thêm vào giỏ
                                        </button>
                                    </div>
                                </div>
                                <div className="p-4">
                                    <div className="text-xs text-gray-500 dark:text-gray-400 mb-1">{product.category}</div>
                                    <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2 line-clamp-1">
                                        <Link to={`/products/${product.id}`}>
                                            <span aria-hidden="true" className="absolute inset-0" />
                                            {product.name}
                                        </Link>
                                    </h3>
                                    <div className="flex justify-between items-center">
                                        <p className="text-lg font-bold text-brand-600">{product.price}</p>
                                        <div className="flex items-center text-yellow-400 text-sm">
                                            <Star className="w-4 h-4 fill-current" />
                                            <span className="ml-1 text-gray-500 dark:text-gray-400">{product.rating}</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>

                    <div className="mt-8 text-center md:hidden">
                        <Link to={PAGES.PRODUCTS.path} className="text-brand-600 hover:text-brand-500 font-medium hover:underline">
                            Xem tất cả &rarr;
                        </Link>
                    </div>
                </div>
            </section>
        </MainLayout>
    );
}
