import { createAsyncThunk } from "@reduxjs/toolkit";
import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const login = createAsyncThunk(
  'auth/login',
  async (credentials: { username: string; password: string }) => {
    const response = await axios.post(`${API_BASE_URL}/api/Auth/login`, credentials);
    localStorage.setItem('token', response.data.token);
    return response.data;
  }
);