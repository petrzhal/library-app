import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import API_BASE_URL from '../../config.js';
import './Register.css';
import { useAuth } from '../../AuthContext.js';
import ValidationErrorList from '../ValidationErrorList/ValidationErrorList.jsx';

const Register = () => {
    const [formData, setFormData] = useState({
        Username: '',
        Email: '',
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
            const response = await axios.post(`${API_BASE_URL}/auth/register`, formData);
            console.log(response.data);

            setMessage('Registration successful!');

            const { accessToken, refreshToken } = response.data;
            localStorage.setItem('accessToken', accessToken);
            localStorage.setItem('refreshToken', refreshToken);

            const userResponse = await axios.get(`${API_BASE_URL}/auth/current-user`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            });

            console.log('Current user:', userResponse.data);

            localStorage.setItem('user', JSON.stringify(userResponse.data));
            setUser(userResponse.data);

            setValidationErrors([]);
            navigate('/');
        } catch (error) {
            if (error.response) {
                console.log('Error response:', error.response.data);
                setMessage(error.response.data.message);

                if (error.response.status === 422) {
                    const errorDetails = error.response.data.details || [];
                    setValidationErrors(errorDetails);
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
        <div className="register-container">
            <h1>Register</h1>
            <form className="register-form" onSubmit={handleSubmit}>
                <input
                    type="text"
                    name="Username"
                    placeholder="Username"
                    value={formData.Username}
                    onChange={handleChange}
                    required
                />
                <input
                    type="email"
                    name="Email"
                    placeholder="Email"
                    value={formData.Email}
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
                <button type="submit">Register</button>
            </form>
            {message && <p className="message">{message}</p>}
            <ValidationErrorList errors={validationErrors} />
        </div>
    );
};

export default Register;
