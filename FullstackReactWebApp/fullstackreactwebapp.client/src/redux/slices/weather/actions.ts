// src/features/weather/weatherSlice.ts

import { createAsyncThunk } from "@reduxjs/toolkit";
import type { WeatherForecast } from "./weather.model";

// Define async thunk to fetch weather data
export const fetchWeatherForecasts = createAsyncThunk<
  WeatherForecast[], // Return type
  void,              // Argument type
  { rejectValue: string }
>(
  "weather/fetchWeatherForecasts",
  async (_, { rejectWithValue }) => {
    try {
      const response = await fetch(`/weatherforecast`);
      if (!response.ok) {
        throw new Error(`HTTP error ${response.status}`);
      }
      const data = (await response.json()) as WeatherForecast[];
      return data;
    } catch (error: any) {
      return rejectWithValue(error.message ?? "Failed to fetch weather data");
    }
  }
);