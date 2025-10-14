import { combineReducers } from '@reduxjs/toolkit';
import weatherSlice from './weather';

export const commonReducer = combineReducers({
  weatherSlice: weatherSlice
});

