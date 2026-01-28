import axiosClient from "./axiosClient";
import type { User } from "../types/identity/dtos/User";
import type { LoginRequest } from "../types/identity/dtos/LoginRequest";
import type { RegisterRequest } from "../types/identity/dtos/RegisterRequest";
import type { ConfirmEmailRequest } from "../types/identity/dtos/ConfirmEmailRequest";
import type { ForgotPasswordRequest } from "../types/identity/dtos/ForgotPasswordRequest";
import type { ValidateResetPasswordRequest } from "../types/identity/dtos/ValidateResetPasswordRequest";
import type { ResetPasswordRequest } from "../types/identity/dtos/ResetPasswordRequest";
import type { GoogleLoginRequest } from "../types/identity/dtos/GoogleLoginRequest";

export type {
  LoginRequest,
  RegisterRequest,
  ConfirmEmailRequest,
  ForgotPasswordRequest,
  ValidateResetPasswordRequest,
  ResetPasswordRequest,
  GoogleLoginRequest,
};

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
