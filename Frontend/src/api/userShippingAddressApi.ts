import axiosClient from "./axiosClient";
import type { UserShippingAddressSummary } from "../types/shipping-addresses/dtos/UserShippingAddressSummary";
import type { UserShippingAddressDetail } from "../types/shipping-addresses/dtos/UserShippingAddressDetail";
import type { AddUserShippingAddressRequest } from "../types/shipping-addresses/dtos/AddUserShippingAddressRequest";

const userShippingAddressApi = {
  getUserShippingAddresses: async (): Promise<UserShippingAddressSummary[]> => {
    const response = await axiosClient.get("/users/shipping-addresses");
    return response.data;
  },
  getUserShippingAddressById: async (
    id: string,
  ): Promise<UserShippingAddressDetail> => {
    const response = await axiosClient.get(`/users/shipping-addresses/${id}`);
    return response.data;
  },
  addUserShippingAddress: async (
    data: AddUserShippingAddressRequest,
  ): Promise<void> => {
    await axiosClient.post("/users/shipping-addresses", data);
  },
  updateUserShippingAddress: async (
    id: string,
    data: AddUserShippingAddressRequest,
  ): Promise<void> => {
    await axiosClient.put(`/users/shipping-addresses/${id}`, data);
  },
  removeUserShippingAddress: async (id: string): Promise<void> => {
    await axiosClient.delete(`/users/shipping-addresses/${id}`);
  },
};

export default userShippingAddressApi;
