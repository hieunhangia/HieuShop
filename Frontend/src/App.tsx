
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ThemeProvider } from './context/ThemeContext';
import { AuthProvider } from './context/AuthContext';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ConfirmEmailPage from './pages/ConfirmEmailPage';
import ForgotPasswordPage from './pages/ForgotPasswordPage';
import ResetPasswordPage from './pages/ResetPasswordPage';

import { PAGES } from './config/page';


function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <Router>
          <Routes>
            <Route path={PAGES.HOME.path} element={<HomePage />} />
            <Route path={PAGES.LOGIN.path} element={<LoginPage />} />
            <Route path={PAGES.REGISTER.path} element={<RegisterPage />} />
            <Route path={PAGES.FORGOT_PASSWORD.path} element={<ForgotPasswordPage />} />
            <Route path={PAGES.CONFIRM_EMAIL.path} element={<ConfirmEmailPage />} />
            <Route path={PAGES.RESET_PASSWORD.path} element={<ResetPasswordPage />} />


            <Route path="*" element={<div className="flex h-screen items-center justify-center">404 - Not Found</div>} />
          </Routes>
        </Router>
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;
