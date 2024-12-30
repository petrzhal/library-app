import React from "react";
import "./ValidationErrorList.css";

const ValidationErrorList = ({ errors }) => {
    if (!errors || errors.length === 0) {
        return null;
    }

    return (
        <div className="validation-errors-modal">
            <h3>Ошибки валидации:</h3>
            {errors.map((error, index) => (
                <div key={index} className="error-block">
                    <strong className="error-property">{error.property}</strong>
                    <ul className="error-messages">
                        {error.messages.map((message, msgIndex) => (
                            <li key={msgIndex} className="error-message">
                                {message}
                            </li>
                        ))}
                    </ul>
                </div>
            ))}
        </div>
    );
};

export default ValidationErrorList;
