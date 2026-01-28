import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { ThemeProvider } from "./context/ThemeContext";
import { AuthProvider } from "./context/AuthContext";
import { Toaster } from "react-hot-toast";
import HomePage from "./pages/HomePage";
import ProductsPage from "./pages/products/ProductsPage";
import ProductDetailPage from "./pages/products/ProductDetailPage";
import ProductsByBrandPage from "./pages/products/ProductsByBrandPage";
import ProductsByCategoryPage from "./pages/products/ProductsByCategoryPage";
import LoginPage from "./pages/identity/LoginPage";
import RegisterPage from "./pages/identity/RegisterPage";
import ConfirmEmailPage from "./pages/identity/ConfirmEmailPage";
import ForgotPasswordPage from "./pages/identity/ForgotPasswordPage";
import ResetPasswordPage from "./pages/identity/ResetPasswordPage";
import InfoPage from "./pages/account-management/InfoPage";
import ChangePasswordPage from "./pages/account-management/ChangePasswordPage";
import SetPasswordPage from "./pages/account-management/SetPasswordPage";

import GuestOnlyRoute from "./components/route-authorization/GuestOnlyRoute";
import CustomerOnlyRoute from "./components/route-authorization/CustomerOnlyRoute";

import { PAGES } from "./config/page";

function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <Router>
          <Routes>
            <Route path={PAGES.HOME.PATH} element={<HomePage />} />
            <Route path={PAGES.PRODUCTS.ALL.PATH} element={<ProductsPage />} />
            <Route
              path={PAGES.PRODUCTS.DETAIL.PATH}
              element={<ProductDetailPage />}
            />
            <Route
              path={PAGES.BRANDS.PRODUCTS.PATH}
              element={<ProductsByBrandPage />}
            />
            <Route
              path={PAGES.CATEGORIES.PRODUCTS.PATH}
              element={<ProductsByCategoryPage />}
            />

            <Route element={<GuestOnlyRoute />}>
              <Route path={PAGES.IDENTITY.LOGIN.PATH} element={<LoginPage />} />
              <Route
                path={PAGES.IDENTITY.REGISTER.PATH}
                element={<RegisterPage />}
              />
              <Route
                path={PAGES.IDENTITY.FORGOT_PASSWORD.PATH}
                element={<ForgotPasswordPage />}
              />
              <Route
                path={PAGES.IDENTITY.RESET_PASSWORD.PATH}
                element={<ResetPasswordPage />}
              />
            </Route>

            <Route
              path={PAGES.IDENTITY.CONFIRM_EMAIL.PATH}
              element={<ConfirmEmailPage />}
            />

            <Route element={<CustomerOnlyRoute />}>
              <Route path={PAGES.ACCOUNT.INFO.PATH} element={<InfoPage />} />
              <Route
                path={PAGES.ACCOUNT.CHANGE_PASSWORD.PATH}
                element={<ChangePasswordPage />}
              />
              <Route
                path={PAGES.ACCOUNT.SET_PASSWORD.PATH}
                element={<SetPasswordPage />}
              />
            </Route>

            <Route
              path="*"
              element={
                <div className="flex h-screen items-center justify-center">
                  404 - Not Found
                </div>
              }
            />
          </Routes>
          <Toaster position="top-right" reverseOrder={false} />
        </Router>
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;
