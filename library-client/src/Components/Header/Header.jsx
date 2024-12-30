import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Header.css';
import { useAuth } from '../../AuthContext.js';
import authorizedAxios from '../../axiosConfig.js';
import API_BASE_URL from '../../config.js';

const Header = () => {
    const { user, setUser } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        const storedUser = localStorage.getItem('user');
        if (storedUser) {
            setUser(JSON.parse(storedUser));
        }
    }, [setUser]);

    const handleLogout = () => {
        const requestData = {
            refreshToken: localStorage.getItem('refreshToken')
        }
        authorizedAxios.post(`${API_BASE_URL}/auth/logout`, requestData)
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('user');
        setUser(null);
        navigate('/login');
    };

    useEffect(() => {
        const handleStorageChange = () => {
            const updatedUser = localStorage.getItem('user');
            setUser(updatedUser ? JSON.parse(updatedUser) : null);
        };

        window.addEventListener('storage', handleStorageChange);

        return () => {
            window.removeEventListener('storage', handleStorageChange);
        };
    }, [setUser]);

    return (
        <header className="header">
            <div className="logo">Библиотека</div>
            <nav className="nav">
                <a href="/books">Книги</a>
                <a href="/borrowed-books">Мои книги</a>
                {user ? (
                    <div className="auth-section">
                        <span className="username">Привет, {user.username}!</span>
                        <button className="logout-btn" onClick={handleLogout}>
                            Выйти
                        </button>
                    </div>
                ) : (
                    <>
                        <a href="/login">Войти</a>
                        <a href="/register">Зарегестрироваться</a>
                    </>
                )}
            </nav>
        </header>
    );
};

export default Header;
