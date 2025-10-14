// src/models/WeatherForecast.ts

export interface WeatherForecast {
  date: string; // ISO date string from backend
  temperatureC: number;
  temperatureF: number;
  summary?: string;
}

export interface WeatherSlice {
    forecasts: WeatherForecast[];
    loading: boolean;
    error: string | null;
}

