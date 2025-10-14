// src/features/auth/Login.tsx
import React, { useState } from "react";
import { login } from "./redux/slices/auth/actions";
import { useAppDispatch, useAppSelector } from "./hooks/redux";

const Login: React.FC = () => {
  const dispatch = useAppDispatch();
  const { status, error, token } = useAppSelector((state) => state.common.authSlice);

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    dispatch(login({ username, password }));
  };

  return (
    <div style={{ maxWidth: 400, margin: "auto", padding: "2rem" }}>
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Username:</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div style={{ marginTop: 10 }}>
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button
          type="submit"
          style={{ marginTop: 20 }}
          disabled={status === "loading"}
        >
          {status === "loading" ? "Logging in..." : "Login"}
        </button>
      </form>

      {error && <p style={{ color: "red" }}>{error}</p>}
      {token && <p style={{ color: "green" }}>Logged in successfully!</p>}
    </div>
  );
};

export default Login;
