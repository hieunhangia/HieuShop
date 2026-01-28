import type { CartItem } from "./CartItem.ts";

export interface Cart {
  cartItems: CartItem[];
  warningMessage?: string;
}
