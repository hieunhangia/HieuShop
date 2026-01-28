import React, { useRef, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import {
  Moon,
  Sun,
  LogOut,
  Menu,
  X,
  User as UserIcon,
  ChevronDown,
  ShoppingCart,
  MapPin,
} from "lucide-react";
import { useTheme } from "../context/ThemeContext";
import { useAuth } from "../context/AuthContext";
import { useCart } from "../context/CartContext";
import { PAGES } from "../config/page";
import { USER_ROLES } from "../types/common/enums/userRoles";

export default function Navbar() {
  const { theme, toggleTheme } = useTheme();
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [isMenuOpen, setIsMenuOpen] = React.useState(false);
  const [isUserMenuOpen, setIsUserMenuOpen] = React.useState(false);
  const userMenuRef = useRef<HTMLDivElement>(null);
  const { cartCount } = useCart();

  const handleLogout = async () => {
    await logout();
    navigate(PAGES.IDENTITY.LOGIN.PATH);
  };

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        userMenuRef.current &&
        !userMenuRef.current.contains(event.target as Node)
      ) {
        setIsUserMenuOpen(false);
      }
    }
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  return (
    <nav className="fixed top-0 w-full z-50 bg-white/80 dark:bg-gray-950/80 backdrop-blur-md border-b border-gray-200 dark:border-gray-800 transition-colors duration-300">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          {/* Logo */}
          <Link to={PAGES.HOME.PATH} className="flex items-center space-x-2">
            <img
              src="/logo.webp"
              alt="HieuShop Logo"
              className="w-8 h-8 object-contain"
            />
            <span className="text-2xl font-bold bg-gradient-to-r from-brand-600 to-indigo-600 bg-clip-text text-transparent">
              HieuShop
            </span>
          </Link>

          {/* Desktop Navigation */}
          <div className="hidden md:flex items-center space-x-8">
            <Link
              to={PAGES.HOME.PATH}
              className="text-gray-700 dark:text-gray-200 hover:text-brand-600 dark:hover:text-brand-400 font-medium transition-colors"
            >
              {PAGES.HOME.TITLE}
            </Link>
          </div>

          {/* Actions */}
          <div className="hidden md:flex items-center space-x-4">
            <button
              onClick={toggleTheme}
              className="p-2 rounded-full hover:bg-gray-100 dark:hover:bg-gray-800 text-gray-700 dark:text-gray-200 transition-colors"
              aria-label="Toggle Theme"
            >
              {theme === "light" ? <Moon size={20} /> : <Sun size={20} />}
            </button>

            {user && user.roles && user.roles.includes(USER_ROLES.CUSTOMER) && (
              <Link
                to={PAGES.USER.CARTS.PATH}
                className="p-2 rounded-full hover:bg-gray-100 dark:hover:bg-gray-800 text-gray-700 dark:text-gray-200 transition-colors relative"
                aria-label="Shopping Cart"
              >
                <ShoppingCart size={20} />
                {cartCount > 0 && (
                  <span className="absolute top-0 right-0 inline-flex items-center justify-center px-1.5 py-0.5 text-xs font-bold leading-none text-white transform translate-x-1/4 -translate-y-1/4 bg-red-600 rounded-full">
                    {cartCount}
                  </span>
                )}
              </Link>
            )}

            {user ? (
              <div className="relative" ref={userMenuRef}>
                <button
                  onClick={() => setIsUserMenuOpen(!isUserMenuOpen)}
                  className="flex items-center space-x-2 p-1 pl-2 pr-2 rounded-full border border-gray-200 dark:border-gray-800 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
                >
                  <div className="w-8 h-8 rounded-full bg-brand-100 dark:bg-brand-900/30 flex items-center justify-center text-brand-600 dark:text-brand-400">
                    <UserIcon size={18} />
                  </div>
                  <span className="text-sm font-medium text-gray-700 dark:text-gray-200 hidden lg:block max-w-[120px] truncate">
                    {user.email.split("@")[0]}
                  </span>
                  <ChevronDown
                    size={16}
                    className={`text-gray-500 transition-transform duration-200 ${isUserMenuOpen ? "rotate-180" : ""}`}
                  />
                </button>

                {isUserMenuOpen && (
                  <div className="absolute right-0 mt-2 w-60 bg-white dark:bg-gray-900 rounded-xl shadow-xl border border-gray-100 dark:border-gray-800 py-2 ring-1 ring-black ring-opacity-5 focus:outline-none transform opacity-100 scale-100 transition-all origin-top-right">
                    <div className="px-4 py-3 border-b border-gray-100 dark:border-gray-800 mb-1">
                      <p className="text-xs text-gray-500 dark:text-gray-400 mb-1">
                        Đăng nhập với
                      </p>
                      <p
                        className="text-sm font-semibold text-gray-900 dark:text-white truncate"
                        title={user.email}
                      >
                        {user.email}
                      </p>
                    </div>

                    {user.roles && user.roles.includes(USER_ROLES.CUSTOMER) && (
                      <div className="py-1">
                        <Link
                          to={PAGES.USER.ACCOUNT_MANAGEMENT.INFO.PATH}
                          className="flex items-center px-4 py-2.5 text-sm text-gray-700 dark:text-gray-200 hover:bg-gray-50 dark:hover:bg-gray-800/50 hover:text-brand-600 dark:hover:text-brand-400 transition-colors"
                          onClick={() => setIsUserMenuOpen(false)}
                        >
                          <UserIcon
                            size={16}
                            className="mr-3 text-gray-400 group-hover:text-brand-500"
                          />
                          {PAGES.USER.ACCOUNT_MANAGEMENT.INFO.TITLE}
                        </Link>
                        <Link
                          to={PAGES.USER.SHIPPING_ADDRESS.PATH}
                          className="flex items-center px-4 py-2.5 text-sm text-gray-700 dark:text-gray-200 hover:bg-gray-50 dark:hover:bg-gray-800/50 hover:text-brand-600 dark:hover:text-brand-400 transition-colors"
                          onClick={() => setIsUserMenuOpen(false)}
                        >
                          <MapPin
                            size={16}
                            className="mr-3 text-gray-400 group-hover:text-brand-500"
                          />
                          {PAGES.USER.SHIPPING_ADDRESS.TITLE}
                        </Link>
                      </div>
                    )}

                    <div className="border-t border-gray-100 dark:border-gray-800 my-1"></div>

                    <button
                      onClick={() => {
                        setIsUserMenuOpen(false);
                        handleLogout();
                      }}
                      className="w-full text-left flex items-center px-4 py-2.5 text-sm text-red-600 dark:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/10 transition-colors"
                    >
                      <LogOut size={16} className="mr-3" />
                      Đăng xuất
                    </button>
                  </div>
                )}
              </div>
            ) : (
              <div className="flex items-center space-x-2">
                <Link
                  to={PAGES.IDENTITY.LOGIN.PATH}
                  className="px-4 py-2 text-sm font-medium text-gray-700 dark:text-gray-200 hover:text-brand-600 dark:hover:text-brand-400 transition-colors"
                >
                  {PAGES.IDENTITY.LOGIN.TITLE}
                </Link>
                <Link
                  to={PAGES.IDENTITY.REGISTER.PATH}
                  className="px-4 py-2 text-sm font-medium text-white bg-brand-600 hover:bg-brand-700 rounded-lg shadow-sm transition-colors"
                >
                  {PAGES.IDENTITY.REGISTER.TITLE}
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
              {theme === "light" ? <Moon size={20} /> : <Sun size={20} />}
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
              to={PAGES.HOME.PATH}
              className="block px-3 py-2 rounded-md text-base font-medium text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
              onClick={() => setIsMenuOpen(false)}
            >
              {PAGES.HOME.TITLE}
            </Link>
            {user ? (
              <>
                <div className="border-t border-gray-100 dark:border-gray-800 my-2"></div>
                <div className="px-3 py-2 text-xs font-semibold text-gray-500 uppercase tracking-wider">
                  Tài khoản
                </div>
                {user.roles && user.roles.includes(USER_ROLES.CUSTOMER) && (
                  <Link
                    to={PAGES.USER.ACCOUNT_MANAGEMENT.INFO.PATH}
                    className="block px-3 py-2 rounded-md text-base font-medium text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
                    onClick={() => setIsMenuOpen(false)}
                  >
                    {PAGES.USER.ACCOUNT_MANAGEMENT.INFO.TITLE}
                  </Link>
                )}
                <button
                  onClick={() => {
                    handleLogout();
                    setIsMenuOpen(false);
                  }}
                  className="w-full text-left block px-3 py-2 rounded-md text-base font-medium text-red-600 dark:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors"
                >
                  Đăng Xuất
                </button>
              </>
            ) : (
              <>
                <div className="border-t border-gray-100 dark:border-gray-800 my-2"></div>
                <Link
                  to={PAGES.IDENTITY.LOGIN.PATH}
                  className="block px-3 py-2 rounded-md text-base font-medium text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors"
                  onClick={() => setIsMenuOpen(false)}
                >
                  {PAGES.IDENTITY.LOGIN.TITLE}
                </Link>
                <Link
                  to={PAGES.IDENTITY.REGISTER.PATH}
                  className="block px-3 py-2 rounded-md text-base font-medium text-brand-600 dark:text-brand-400 hover:bg-brand-50 dark:hover:bg-brand-900/20 transition-colors"
                  onClick={() => setIsMenuOpen(false)}
                >
                  {PAGES.IDENTITY.REGISTER.TITLE}
                </Link>
              </>
            )}
          </div>
        </div>
      )}
    </nav>
  );
}
