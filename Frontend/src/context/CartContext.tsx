import React, { createContext, useContext, useEffect, useState } from "react";
import { cartApi } from "../api/cartApi";
import { useAuth } from "./AuthContext";
import { USER_ROLES } from "../types/common/enums/userRoles";

interface CartContextType {
  cartCount: number;
  refreshCartCount: () => Promise<void>;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export function CartProvider({ children }: { children: React.ReactNode }) {
  const [cartCount, setCartCount] = useState<number>(0);
  const { user } = useAuth();

  const refreshCartCount = async () => {
    if (user && user.roles && user.roles.includes(USER_ROLES.CUSTOMER)) {
      try {
        const response = await cartApi.countCartItems();
        setCartCount(response.data);
      } catch (error) {
        console.error("Failed to fetch cart count:", error);
      }
    } else {
      setCartCount(0);
    }
  };

  useEffect(() => {
    refreshCartCount();
  }, [user]);

  return (
    <CartContext.Provider value={{ cartCount, refreshCartCount }}>
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  const context = useContext(CartContext);
  if (context === undefined) {
    throw new Error("useCart must be used within a CartProvider");
  }
  return context;
}
