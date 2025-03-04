import axios from 'axios';
import { toast } from 'react-toastify';

const instance = axios.create({
  baseURL: 'http://localhost:8080',
});

instance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

instance.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    if (error.response.status !== 401) {
      error.response.data.Errors.forEach((error: {ErrorMessage: string}) => {
        toast.error(error.ErrorMessage);
      })
    }
    const originalRequest = error.config;

    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const refreshToken = localStorage.getItem('refreshToken');
        const response = await axios.post(
          'http://localhost:8080/api/auth/refresh',
          {
            accessToken: localStorage.getItem('accessToken'),
            refreshToken: refreshToken,
          }
        );

        localStorage.setItem('accessToken', response.data.accessToken);

        axios.defaults.headers.common['Authorization'] =
          'Bearer ' + response.data.accessToken;

        return instance(originalRequest);
      } catch (err) {
        console.error('Error refreshing token:', err);

        window.location.href = '/login';
      }
    }

    return Promise.reject(error);
  }
);

export default instance;
