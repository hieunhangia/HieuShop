import axiosClient from './axiosClient';

export const identityApi = {
    getUserInfo: () => axiosClient.get('/info'),

    cookieLogin: (data: any) => axiosClient.post('/login?useCookies=true', data),

    register: (data: any) => axiosClient.post('/register', data),

    cookieLogout: () => axiosClient.post('/cookie-logout'),

    confirmEmail: (data: { email: string; code: string }) => axiosClient.post('/confirm-email', data),

    sendConfirmationEmail: () => axiosClient.post('/send-confirmation-email'),

    forgotPassword: (data: { email: string }) => axiosClient.post('/forgot-password', data),

    validateResetPassword: (data: { email: string; resetCode: string }) => axiosClient.post('/validate-reset-password-request', data),

    resetPassword: (data: any) => axiosClient.post('/reset-password', data),

    googleLogin: (data: { idToken: string }) => axiosClient.post('/google-login?useCookies=true', data),
};
