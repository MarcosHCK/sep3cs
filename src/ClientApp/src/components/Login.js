import React, { useState } from 'react';
import axios from 'axios';
import  navigate, { useNavigate }  from 'react-router-dom';
import './Login.css';

function Login() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();
    const handleSubmit = async e => {
        e.preventDefault();

        try {
            // Hacer una solicitud POST a tu servidor para autenticar al usuario
            const response = await axios.post('https://localhost:7264/api/account/login', {
                username,
                password
            });

            // Guardar el token de autenticación en el almacenamiento local
            localStorage.setItem('token', response.data.token);

            // Redirigir al usuario a la página principal
            navigate('/');
        } catch (error) {
            // Manejar el error
            console.error('Error al iniciar sesión:', error);
        }
    };

    return (
        <div className="login-container">
            <h2>Login</h2>
            <form onSubmit={handleSubmit}>
                <input 
                    type="text" 
                    placeholder="Username" 
                    value={username} 
                    onChange={e => setUsername(e.target.value)} 
                />
                <input 
                    type="password" 
                    placeholder="Password" 
                    value={password} 
                    onChange={e => setPassword(e.target.value)} 
                />
                <button type="submit">Login</button>
            </form>
        </div>
    );
}

export default Login;
