import type { ActionReducerMapBuilder, PayloadAction } from "@reduxjs/toolkit";
import type { WeatherForecast, WeatherSlice } from "./weather.model";
import { fetchWeatherForecasts } from "./actions";

export const buildForecastsListMapper = (
  builder: ActionReducerMapBuilder<WeatherSlice>
) => {
    builder
        .addCase(fetchWeatherForecasts.pending, (state) => {
        state.loading = true;
        state.error = null;
        })
        .addCase(fetchWeatherForecasts.fulfilled, (state, action: PayloadAction<WeatherForecast[]>) => {
        state.forecasts = action.payload;
        state.loading = false;
        })
        .addCase(fetchWeatherForecasts.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload ?? "Error fetching weather data";
        });
};