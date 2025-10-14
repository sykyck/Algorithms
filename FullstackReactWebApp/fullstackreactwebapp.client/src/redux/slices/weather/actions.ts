// src/features/weather/weatherSlice.ts

import { createAsyncThunk } from "@reduxjs/toolkit";
import type { WeatherForecast } from "./weather.model";
import { getWeatherForecasts } from "../../../api/weatherforecast.api";
import type { AxiosError } from "axios";

// Define async thunk to fetch weather data
export const fetchWeatherForecasts = createAsyncThunk<
  WeatherForecast[], // ✅ Return type
  void,              // ✅ No argument
  { rejectValue: string }
>(
  "weather/fetchWeatherForecasts",
  async (_, { rejectWithValue }) => {
    try {
      // ✅ Call API (this should return axios response)
      const response = await getWeatherForecasts();

      // ✅ Axios auto-throws for non-2xx, so this line is usually not needed
      return response.data as WeatherForecast[];
    } catch (err) {
      // ✅ Handle Axios errors properly
      const error = err as AxiosError;

      if (error.response) {
        // Server responded with non-2xx
        return rejectWithValue(
          `Server Error: ${error.response.status} ${error.response.statusText}`
        );
      } else if (error.request) {
        // Request was made but no response
        return rejectWithValue("No response from server");
      } else {
        // Something else went wrong
        return rejectWithValue(error.message ?? "Failed to fetch weather data");
      }
    }
  }
);