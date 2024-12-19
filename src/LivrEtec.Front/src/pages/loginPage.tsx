import '../App.css'

import { useState } from "react";
import { useAuth } from "../hooks/useAuth";
import { useApi } from '../clientApi';
import axios from 'axios';
export const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const { login, logout, user } = useAuth()!;
  const api = useApi();
  const handleLogin = async (e) => {
    e.preventDefault();
    

    axios.post(import.meta.env.VITE_API_URL, {
      hashSenha: password,
      idUsuario: parseInt(username)
    })
    .then(async function({ data }) {
      await login({
        user_login: username,
        jwtToken: data
      });
    })
    .catch(function (error) {
      console.log(error);
    });
    

  };
  return (
    <div>
      {user?.user_login}
      <form onSubmit={handleLogin}>
        <div>
          <label htmlFor="username">Username:</label>
          <input
            id="username"
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
          <input
            id="password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <button type="submit">Login</button>
      </form>
      <button onClick={logout}>logout</button>
    </div>
  );
};