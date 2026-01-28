import axiosClient from "./axiosClient";

export interface CartItemDto {
  id: string;
  productVariant: {
    id: string;
    imageUrl: string;
    price: number;
    productOptionValuesString: string;
    product: {
      id: string;
      name: string;
      slug: string;
    };
  };
  quantity: number;
}

export interface CartDto {
  cartItems: CartItemDto[];
  warningMessage?: string;
}

export const cartApi = {
  countCartItems: async () => {
    return axiosClient.get<number>("/carts/count");
  },
  addToCart: async (productVariantId: string) => {
    return axiosClient.post("/carts", { productVariantId });
  },
  syncCart: async () => {
    return axiosClient.post<CartDto>("/carts/sync");
  },
  updateCartItemQuantity: async (cartItemId: string, quantity: number) => {
    return axiosClient.put(`/carts/${cartItemId}/quantity`, { quantity });
  },
  removeCartItem: async (cartItemId: string) => {
    return axiosClient.delete(`/carts/${cartItemId}`);
  },
};
