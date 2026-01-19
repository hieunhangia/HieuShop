
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ThemeProvider } from './context/ThemeContext';
import { AuthProvider } from './context/AuthContext';
import HomePage from './pages/HomePage';
import LoginPage from './pages/identity/LoginPage';
import RegisterPage from './pages/identity/RegisterPage';
import ConfirmEmailPage from './pages/identity/ConfirmEmailPage';
import ForgotPasswordPage from './pages/identity/ForgotPasswordPage';
import ResetPasswordPage from './pages/identity/ResetPasswordPage';

import { PAGES } from './config/page';


function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <Router>
          <Routes>
            <Route path={PAGES.HOME.PATH} element={<HomePage />} />
            <Route path={PAGES.IDENTITY.LOGIN.PATH} element={<LoginPage />} />
            <Route path={PAGES.IDENTITY.REGISTER.PATH} element={<RegisterPage />} />
            <Route path={PAGES.IDENTITY.FORGOT_PASSWORD.PATH} element={<ForgotPasswordPage />} />
            <Route path={PAGES.IDENTITY.CONFIRM_EMAIL.PATH} element={<ConfirmEmailPage />} />
            <Route path={PAGES.IDENTITY.RESET_PASSWORD.PATH} element={<ResetPasswordPage />} />


            <Route path="*" element={<div className="flex h-screen items-center justify-center">404 - Not Found</div>} />
          </Routes>
        </Router>
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;
