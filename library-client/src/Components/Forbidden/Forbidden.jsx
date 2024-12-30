import React from 'react';
import { useNavigate } from 'react-router-dom';
import './Forbidden.css';

const Forbidden = () => {
    const navigate = useNavigate();

    const goBack = () => {
        navigate(-1);
    };

    return (
        <div className="forbidden-page">
            <h1 className="forbidden-title">403 - Доступ запрещен</h1>
            <p className="forbidden-text">У вас нет прав для доступа к этой странице.</p>
            <button className="forbidden-btn" onClick={goBack}>
                Вернуться назад
            </button>
        </div>
    );
};

export default Forbidden;
