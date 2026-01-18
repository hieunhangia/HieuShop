import axios from 'axios';

// Create an Axios instance with defaults
const axiosClient = axios.create({
    baseURL: 'https://localhost:7777',
    withCredentials: true,
});

// Add a response interceptor to handle 401 errors globally if needed
axiosClient.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.status === 401) {
            // Handle unauthorized access (e.g., redirect to login)
            // window.location.href = '/login'; // Optional: Use with caution to avoid loops
        }
        return Promise.reject(error);
    }
);

export default axiosClient;
