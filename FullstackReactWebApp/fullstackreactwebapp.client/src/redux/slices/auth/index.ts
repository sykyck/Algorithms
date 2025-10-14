import { createSlice } from "@reduxjs/toolkit";
import type { AuthState } from "./auth.model";
import { buildLoginMapper } from "./extraReducers";

const initialState: AuthState = {
  token: localStorage.getItem("token"),
  status: "idle",
  error: null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    logout: (state) => {
      localStorage.removeItem('token');
      state.token = null;
    },
  },
  extraReducers: (builder) => {
     buildLoginMapper(builder);
  },
});

export const { logout } = authSlice.actions;
export default authSlice.reducer;