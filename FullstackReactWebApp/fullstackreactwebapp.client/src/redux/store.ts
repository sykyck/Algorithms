import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { supplierReducer } from '../Supplier/Slices';
import { commonReducer } from './Slices';

const combinedReducer = combineReducers({
  common: commonReducer,
  supplier: supplierReducer
});

const rootReducer = (
  state: ReturnType<typeof combinedReducer> | undefined,
  action: any
) => {
  if (action.type === 'LOGOUT') {
    state = undefined; // Clear the state
  }
  return combinedReducer(state, action);
};

const store = configureStore({
  reducer: rootReducer,
  devTools: config.mode === 'dev',
  middleware: getDefaultMiddleware =>
    getDefaultMiddleware({
      serializableCheck: false
    }).concat()
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;