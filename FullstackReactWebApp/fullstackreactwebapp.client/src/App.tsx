import './App.css';
import { Navigate, Route, BrowserRouter, Routes } from 'react-router-dom';
import { useAppSelector } from './hooks/redux';
import Login from './Login';
import Forecasts from './Forecasts';

function App() {
    const {  token } = useAppSelector((state) => state.common.authSlice);

    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={token ? <Navigate to="/forecasts" /> : <Login />} />
                <Route path="/login" element={<Login />} />
                <Route path="/forecasts" element={token ? <Forecasts /> : <Navigate to="/login" />} />
                <Route path="*" element={<Navigate to="/" />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;