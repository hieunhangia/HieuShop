import axiosClient from './axiosClient';
import type { ChangePasswordRequest } from '../types/account-management/dtos/ChangePasswordRequest';
import type { SetPasswordRequest } from '../types/account-management/dtos/SetPasswordRequest';

export type { ChangePasswordRequest, SetPasswordRequest };

export const accountManagementApi = {
    changePassword: (data: ChangePasswordRequest) => axiosClient.post('/change-password', data),
    setPassword: (data: SetPasswordRequest) => axiosClient.post('/set-password', data),
};
