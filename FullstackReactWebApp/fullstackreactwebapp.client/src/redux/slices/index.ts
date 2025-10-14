import { combineReducers } from '@reduxjs/toolkit';
import weatherSlice from './weather';
import authSlice from './auth';

export const commonReducer = combineReducers({
  weatherSlice: weatherSlice,
  authSlice: authSlice,
});

