import { GoogleLogin } from '@react-oauth/google';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { PAGES } from '../../config/page';
import React from 'react';
import { parseApiError } from '../../utils/error';

export default function GoogleLoginButton() {
    const navigate = useNavigate();
    const { loginWithGoogle, isLoading } = useAuth();
    const [isGoogleLoading, setIsGoogleLoading] = React.useState(false);
    const [errorMessage, setErrorMessage] = React.useState<string | null>(null);

    const handleSuccess = async (credentialResponse: any) => {
        try {
            setErrorMessage(null);
            console.log("Google Credential Response:", credentialResponse);
            if (credentialResponse.credential) {
                setIsGoogleLoading(true);
                await loginWithGoogle(credentialResponse.credential);
                navigate(PAGES.HOME.PATH);
            }
        } catch (error: any) {
            console.error("Google login failed", error);
            // Parse backend error
            const apiError = parseApiError(error, 'Đăng nhập Google thất bại. Vui lòng thử lại.');
            setErrorMessage(apiError.message);

        } finally {
            setIsGoogleLoading(false);
        }
    };

    const handleError = () => {
        console.error('Google Login connection failed');
        setErrorMessage('Không thể kết nối với Google. Vui lòng kiểm tra kết nối mạng.');
    };

    return (
        <div className="w-full flex flex-col gap-3">
            {errorMessage && (
                <div className="p-3 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg text-red-600 dark:text-red-400 text-sm whitespace-pre-line text-center">
                    {errorMessage}
                </div>
            )}

            {(isLoading || isGoogleLoading) ? (
                <div className="w-full h-[40px] flex items-center justify-center border border-gray-300 dark:border-gray-600 rounded-lg bg-gray-50 dark:bg-gray-800">
                    <span className="text-sm text-gray-500">Đang xử lý...</span>
                </div>
            ) : (
                <div className="w-full">
                    <GoogleLogin
                        onSuccess={handleSuccess}
                        onError={handleError}
                        width="385"
                        theme="filled_blue"
                        shape="pill"
                        text="signin_with"
                    />
                </div>
            )}
        </div>
    );
}
