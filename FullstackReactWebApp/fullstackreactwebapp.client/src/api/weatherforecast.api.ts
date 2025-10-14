import api from './axios';

export const getWeatherForecasts = async () => {
  const res = await api.get('/weatherforecast');
  return res;
};