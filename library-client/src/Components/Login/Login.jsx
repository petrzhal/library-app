import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import API_BASE_URL from '../../config.js';
import './Login.css';
import { useAuth } from '../../AuthContext.js';

const Login = () => {
    const [formData, setFormData] = useState({
        Username: '',
        Password: '',
    });

    const [message, setMessage] = useState('');
    const [validationErrors, setValidationErrors] = useState([]);
    const navigate = useNavigate();
    const { setUser } = useAuth();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
        setValidationErrors([]);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const loginResponse = await axios.post(`${API_BASE_URL}/auth/login`, formData);
            console.log(loginResponse.data);

            setMessage('Login successful!');
            setValidationErrors([]);

            const { accessToken, refreshToken } = loginResponse.data;
            localStorage.setItem('accessToken', accessToken);
            localStorage.setItem('refreshToken', refreshToken);

            const userResponse = await axios.get(`${API_BASE_URL}/auth/current-user`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            });

            localStorage.setItem('user', JSON.stringify(userResponse.data));

            setUser(userResponse.data);

            navigate('/');
        } catch (error) {
            if (error.response) {
                console.log('Error response:', error.response.data);

                if (error.response.status === 401) {
                    setMessage('Incorrect username or password');
                } else {
                    setMessage(error.response.data.message || 'An error occurred.');
                }

                if (error.response.data.details) {
                    const errors = error.response.data.details.map(({ errorMessage }) => errorMessage);
                    setValidationErrors(errors);
                }
            } else if (error.request) {
                console.log('Error request:', error.request);
                setMessage('No response from server. Please try again.');
            } else {
                console.log('Error message:', error.message);
                setMessage('An error occurred. Please try again.');
            }
        }
    };

    return (
        <div className="login-container">
            <h1>Login</h1>
            <form className="login-form" onSubmit={handleSubmit}>
                <input
                    type="text"
                    name="Username"
                    placeholder="Username"
                    value={formData.Username}
                    onChange={handleChange}
                    required
                />
                <input
                    type="password"
                    name="Password"
                    placeholder="Password"
                    value={formData.Password}
                    onChange={handleChange}
                    required
                />
                <button type="submit">Login</button>
            </form>
            {message && <p className="message">{message}</p>}
            {validationErrors.length > 0 && (
                <ul className="error-list">
                    {validationErrors.map((error, index) => (
                        <li key={index} className="error">{error}</li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default Login;
