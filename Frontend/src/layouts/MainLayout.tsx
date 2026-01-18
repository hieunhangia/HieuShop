import React from 'react';
import Navbar from '../components/Navbar';

interface MainLayoutProps {
    children: React.ReactNode;
}

export default function MainLayout({ children }: MainLayoutProps) {
    return (
        <div className="min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors duration-300 flex flex-col">
            <Navbar />
            <main className="pt-16 flex-grow">
                {children}
            </main>
            <footer className="bg-white dark:bg-gray-950 border-t border-gray-200 dark:border-gray-800 py-8 mt-12 transition-colors duration-300">
                <div className="max-w-7xl mx-auto px-4 text-center text-gray-500 dark:text-gray-400">
                    <p>© {new Date().getFullYear()} HieuShop. All rights reserved.</p>
                </div>
            </footer>
        </div>
    );
}
