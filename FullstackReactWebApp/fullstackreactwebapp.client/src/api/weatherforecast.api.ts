import api from './axios';

export const getWeatherForecasts = async () => {
  const res = await api.get('/WeatherForecast');
  return res;
};