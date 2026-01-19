import axiosClient from './axiosClient';

export const accountManagementApi = {
    changePassword: (data: any) => axiosClient.post('/change-password', data),
    setPassword: (data: any) => axiosClient.post('/set-password', data),
};
