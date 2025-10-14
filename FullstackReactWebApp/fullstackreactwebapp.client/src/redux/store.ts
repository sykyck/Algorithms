import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { commonReducer } from './slices';

const combinedReducer = combineReducers({
  common: commonReducer,
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
  devTools: true,
  middleware: getDefaultMiddleware =>
    getDefaultMiddleware({
      serializableCheck: false
    }).concat()
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;