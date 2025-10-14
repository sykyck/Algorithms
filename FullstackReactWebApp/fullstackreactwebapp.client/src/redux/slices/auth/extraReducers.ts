import type { ActionReducerMapBuilder } from "@reduxjs/toolkit";
import { login } from "./actions";
import type { AuthState } from "./auth.model";

export const buildLoginMapper = (
  builder: ActionReducerMapBuilder<AuthState>
) => {
    builder
      .addCase(login.pending, (state) => {
        state.status = "loading";
        state.error = null;
      })
      .addCase(login.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.token = action.payload.token;
        localStorage.setItem("token", action.payload.token);
      })
      .addCase(login.rejected, (state, action) => {
        state.status = "failed";
        state.error =  "Login failed";
      });
};