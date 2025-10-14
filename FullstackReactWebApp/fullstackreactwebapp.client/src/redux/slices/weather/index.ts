import { createSlice } from "@reduxjs/toolkit";
import type { WeatherSlice } from "./weather.model";
import { buildForecastsListMapper } from "./extraReducers";

const initialState: WeatherSlice = {
  forecasts: [],
  loading: false,
  error: null,
};

// Create slice
const weatherSlice = createSlice({
  name: "weather",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    buildForecastsListMapper(builder);
  },
});

export default weatherSlice.reducer;