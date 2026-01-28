import axiosClient from './axiosClient';
import type { ChangePasswordRequest } from '../types/user/account-management/dtos/ChangePasswordRequest';
import type { SetPasswordRequest } from '../types/user/account-management/dtos/SetPasswordRequest';

export type { ChangePasswordRequest, SetPasswordRequest };

export const accountManagementApi = {
    changePassword: (data: ChangePasswordRequest) => axiosClient.post('/change-password', data),
    setPassword: (data: SetPasswordRequest) => axiosClient.post('/set-password', data),
};
