import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Moon, Sun, LogOut, Menu, X } from 'lucide-react';
import { useTheme } from '../context/ThemeContext';
import { useAuth } from '../context/AuthContext';
import { PAGES } from '../config/page';


export default function Navbar() {
    const { theme, toggleTheme } = useTheme();
    const { user, logout } = useAuth();
    const navigate = useNavigate();
    const [isMenuOpen, setIsMenuOpen] = React.useState(false);

    const handleLogout = async () => {
        await logout();
        navigate(PAGES.LOGIN.path);
    };

    return (
        <nav className="fixed top-0 w-full z-50 bg-white/80 dark:bg-gray-950/80 backdrop-blur-md border-b border-gray-200 dark:border-gray-800 transition-colors duration-300">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex justify-between items-center h-16">
                    {/* Logo */}
                    <Link to={PAGES.HOME.path} className="flex items-center space-x-2">
                        <span className="text-2xl font-bold bg-gradient-to-r from-brand-600 to-indigo-600 bg-clip-text text-transparent">
                            HieuShop
                        </span>
                    </Link>

                    {/* Desktop Navigation */}
                    <div className="hidden md:flex items-center space-x-8">
                        <Link to={PAGES.HOME.path} className="text-gray-700 dark:text-gray-200 hover:text-brand-600 dark:hover:text-brand-400 font-medium transition-colors">
                            {PAGES.HOME.title}
                        </Link>
                        <Link to={PAGES.PRODUCTS.path} className="text-gray-700 dark:text-gray-200 hover:text-brand-600 dark:hover:text-brand-400 font-medium transition-colors">
                            {PAGES.PRODUCTS.title}
                        </Link>
                        <Link to={PAGES.ABOUT.path} className="text-gray-700 dark:text-gray-200 hover:text-brand-600 dark:hover:text-brand-400 font-medium transition-colors">
                            {PAGES.ABOUT.title}
                        </Link>
                    </div>

                    {/* Actions */}
                    <div className="hidden md:flex items-center space-x-4">
                        <button
                            onClick={toggleTheme}
                            className="p-2 rounded-full hover:bg-gray-100 dark:hover:bg-gray-800 text-gray-700 dark:text-gray-200 transition-colors"
                            aria-label="Toggle Theme"
                        >
                            {theme === 'light' ? <Moon size={20} /> : <Sun size={20} />}
                        </button>

                        {user ? (
                            <div className="flex items-center space-x-4">
                                <span className="text-sm font-medium text-gray-700 dark:text-gray-200">
                                    {user.email}
                                </span>
                                <button
                                    onClick={handleLogout}
                                    className="p-2 rounded-full hover:bg-red-50 dark:hover:bg-red-900/20 text-red-600 dark:text-red-400 transition-colors"
                                    title="Đăng xuất"
                                >
                                    <LogOut size={20} />
                                </button>
                            </div>
                        ) : (
                            <div className="flex items-center space-x-2">
                                <Link
                                    to={PAGES.LOGIN.path}
                                    className="px-4 py-2 text-sm font-medium text-gray-700 dark:text-gray-200 hover:text-brand-600 dark:hover:text-brand-400 transition-colors"
                                >
                                    {PAGES.LOGIN.title}
                                </Link>
                                <Link
                                    to={PAGES.REGISTER.path}
                                    className="px-4 py-2 text-sm font-medium text-white bg-brand-600 hover:bg-brand-700 rounded-lg shadow-sm transition-colors"
                                >
                                    {PAGES.REGISTER.title}
                                </Link>
                            </div>
                        )}
                    </div>

                    {/* Mobile menu button */}
                    <div className="md:hidden flex items-center space-x-2">
                        <button
                            onClick={toggleTheme}
                            className="p-2 rounded-full hover:bg-gray-100 dark:hover:bg-gray-800 text-gray-700 dark:text-gray-200 transition-colors"
                        >
                            {theme === 'light' ? <Moon size={20} /> : <Sun size={20} />}
                        </button>
                        <button
                            onClick={() => setIsMenuOpen(!isMenuOpen)}
                            className="p-2 rounded-md text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
                        >
                            {isMenuOpen ? <X size={24} /> : <Menu size={24} />}
                        </button>
                    </div>
                </div>
            </div>

            {/* Mobile Menu */}
            {isMenuOpen && (
                <div className="md:hidden bg-white dark:bg-gray-950 border-t border-gray-200 dark:border-gray-800">
                    <div className="px-4 pt-2 pb-4 space-y-1">
                        <Link
                            to={PAGES.HOME.path}
                            className="block px-3 py-2 rounded-md text-base font-medium text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
                            onClick={() => setIsMenuOpen(false)}
                        >
                            {PAGES.HOME.title}
                        </Link>
                        <Link
                            to={PAGES.PRODUCTS.path}
                            className="block px-3 py-2 rounded-md text-base font-medium text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
                            onClick={() => setIsMenuOpen(false)}
                        >
                            {PAGES.PRODUCTS.title}
                        </Link>
                        {user ? (
                            <button
                                onClick={() => {
                                    handleLogout();
                                    setIsMenuOpen(false);
                                }}
                                className="w-full text-left block px-3 py-2 rounded-md text-base font-medium text-red-600 dark:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors"
                            >
                                Đăng Xuất ({user.email})
                            </button>
                        ) : (
                            <>
                                <Link
                                    to={PAGES.LOGIN.path}
                                    className="block px-3 py-2 rounded-md text-base font-medium text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
                                    onClick={() => setIsMenuOpen(false)}
                                >
                                    {PAGES.LOGIN.title}
                                </Link>
                                <Link
                                    to={PAGES.REGISTER.path}
                                    className="block px-3 py-2 rounded-md text-base font-medium text-brand-600 dark:text-brand-400 hover:bg-brand-50 dark:hover:bg-brand-900/20 transition-colors"
                                    onClick={() => setIsMenuOpen(false)}
                                >
                                    {PAGES.REGISTER.title}
                                </Link>
                            </>
                        )}
                    </div>
                </div>
            )}
        </nav>
    );
}
