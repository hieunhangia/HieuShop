import axiosClient from "./axiosClient";
import type { Cart } from "../types/carts/dtos/Cart";

export const cartApi = {
  countCartItems: async () => {
    return axiosClient.get<number>("/carts/count");
  },
  addToCart: async (productVariantId: string) => {
    return axiosClient.post("/carts", { productVariantId });
  },
  syncCart: async () => {
    return axiosClient.post<Cart>("/carts/sync");
  },
  updateCartItemQuantity: async (cartItemId: string, quantity: number) => {
    return axiosClient.put(`/carts/${cartItemId}/quantity`, { quantity });
  },
  removeCartItem: async (cartItemId: string) => {
    return axiosClient.delete(`/carts/${cartItemId}`);
  },
};
