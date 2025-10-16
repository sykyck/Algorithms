import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

const instance = axios.create({
  baseURL: API_BASE_URL
});

instance.interceptors.request.use((config) => {
  console.log('Sending request:', config.url, config.headers);
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
    console.log('Added Authorization header to request', config.headers.Authorization);
  }
  return config;
});

export default instance;
