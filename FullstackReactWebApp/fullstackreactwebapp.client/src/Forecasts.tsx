// src/components/Forecasts.tsx
import React, { useEffect } from 'react';
import { fetchWeatherForecasts } from './redux/slices/weather/actions';
import { useAppDispatch, useAppSelector } from './hooks/redux';

const Forecasts: React.FC = () => {
    const dispatch = useAppDispatch();
    const forecasts = useAppSelector((state) => state.common.weatherSlice.forecasts);

    useEffect(() => {
        dispatch(fetchWeatherForecasts());
    }, []);

    if (!forecasts) {
        return <p><em>Loading... Please refresh once the backend has started.</em></p>;
    }

    return (
        <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(f => (
                    <tr key={f.date}>
                        <td>{f.date}</td>
                        <td>{f.temperatureC}</td>
                        <td>{f.temperatureF}</td>
                        <td>{f.summary}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export default Forecasts;
