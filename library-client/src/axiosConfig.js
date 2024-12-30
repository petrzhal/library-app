import axios from 'axios';
import API_BASE_URL from './config';

const authorizedAxios = axios.create({
    baseURL: API_BASE_URL,
    headers: { 'Content-Type': 'application/json' },
});

authorizedAxios.interceptors.request.use(
    (config) => {
        const accessToken = localStorage.getItem("accessToken");
        if (accessToken) {
            config.headers['Authorization'] = `Bearer ${accessToken}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

authorizedAxios.interceptors.response.use(
    (response) => {
        return response;
    },
    async (error) => {
        const originalRequest = error.config;

        if (error.response && error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            try {
                const refreshToken = localStorage.getItem("refreshToken");

                if (!refreshToken) {
                    console.error("No refresh token available.");
                    return Promise.reject(new Error("No refresh token available."));
                }

                const response = await axios.post(`${API_BASE_URL}/auth/refresh`, { refreshToken });
                localStorage.setItem('accessToken', response.data.accessToken);
                localStorage.setItem('refreshToken', response.data.refreshToken);

                originalRequest.headers['Authorization'] = `Bearer ${response.data.accessToken}`;

                return authorizedAxios(originalRequest);
            } catch (refreshError) {
                console.error("Refresh token failed:", refreshError);

                try {
                    const requestData = {
                        refreshToken: localStorage.getItem('refreshToken')
                    };
                    await axios.post(`${API_BASE_URL}/auth/logout`, requestData);
                } catch (logoutError) {
                    console.error("Logout failed:", logoutError);
                }

                return Promise.reject(refreshError);
            }
        }
        return Promise.reject(error);
    }
);

export default authorizedAxios;
