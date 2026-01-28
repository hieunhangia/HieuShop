import React, { createContext, useContext, useEffect, useState } from "react";
import { identityApi } from "../api/identityApi";
import type { LoginRequest, RegisterRequest } from "../api/identityApi";
import type { User } from "../types/identity/dtos/User";

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (data: LoginRequest) => Promise<void>;
  loginWithGoogle: (token: string) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  logout: () => Promise<void>;
  checkAuth: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const checkAuth = async () => {
    try {
      const response = await identityApi.getUserInfo();
      setUser(response.data);
    } catch (error) {
      setUser(null);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    checkAuth();
  }, []);

  const login = async (data: LoginRequest) => {
    await identityApi.cookieLogin(data);
    await checkAuth();
  };

  const loginWithGoogle = async (idToken: string) => {
    await identityApi.googleLogin({ idToken });
    await checkAuth();
  };

  const register = async (data: RegisterRequest) => {
    await identityApi.register(data);
  };

  const logout = async () => {
    await identityApi.cookieLogout();
    setUser(null);
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        isLoading,
        login,
        loginWithGoogle,
        register,
        logout,
        checkAuth,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error("useAuth must be used within a AuthProvider");
  }
  return context;
}
