import { createAsyncThunk } from "@reduxjs/toolkit";
import axios from 'axios';

export const login = createAsyncThunk(
  'auth/login',
  async (credentials: { username: string; password: string }) => {
    const response = await axios.post('/api/auth/login', credentials);
    localStorage.setItem('token', response.data.token);
    return response.data;
  }
);