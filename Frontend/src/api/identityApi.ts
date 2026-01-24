import axiosClient from "./axiosClient";
import type { User } from "../types/identity/User";

export interface LoginRequest {
  email?: string;
  password?: string;
  twoFactorCode?: string;
  twoFactorRecoveryCode?: string;
}

export interface RegisterRequest {
  email: string;
  password?: string;
}

export interface ConfirmEmailRequest {
  email: string;
  code: string;
  changedEmail?: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ValidateResetPasswordRequest {
  email: string;
  resetCode: string;
}

export interface ResetPasswordRequest {
  email: string;
  resetCode: string;
  newPassword?: string;
}

export interface GoogleLoginRequest {
  idToken: string;
}

export const identityApi = {
  getUserInfo: () => axiosClient.get<User>("/info"),

  cookieLogin: (data: LoginRequest) =>
    axiosClient.post("/login?useCookies=true", data),

  register: (data: RegisterRequest) => axiosClient.post("/register", data),

  cookieLogout: () => axiosClient.post("/cookie-logout"),

  confirmEmail: (data: ConfirmEmailRequest) =>
    axiosClient.post("/confirm-email", data),

  sendConfirmationEmail: () => axiosClient.post("/send-confirmation-email"),

  forgotPassword: (data: ForgotPasswordRequest) =>
    axiosClient.post("/forgot-password", data),

  validateResetPassword: (data: ValidateResetPasswordRequest) =>
    axiosClient.post("/validate-reset-password-request", data),

  resetPassword: (data: ResetPasswordRequest) =>
    axiosClient.post("/reset-password", data),

  googleLogin: (data: GoogleLoginRequest) =>
    axiosClient.post("/google-login?useCookies=true", data),
};
