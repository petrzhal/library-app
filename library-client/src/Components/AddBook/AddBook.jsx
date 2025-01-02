import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import authorizedAxios from "../../axiosConfig.js";
import API_BASE_URL from "../../config";
import ValidationErrorList from "../ValidationErrorList/ValidationErrorList.jsx";
import "./AddBook.css";

const AddBook = () => {
    const [book, setBook] = useState({
        title: "",
        authorId: "",
        genre: "",
        isbn: "",
        description: "",
        imageData: "",
        imageType: "",
    });
    const [authors, setAuthors] = useState([]);
    const [selectedAuthor, setSelectedAuthor] = useState(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [isLoading, setIsLoading] = useState(true);
    const [showAddAuthorModal, setShowAddAuthorModal] = useState(false);
    const [newAuthor, setNewAuthor] = useState({
        firstName: "",
        lastName: "",
        dateOfBirth: "",
        country: "",
    });
    const [validationErrors, setValidationErrors] = useState([]);
    const [showValidationErrors, setShowValidationErrors] = useState(false);

    const navigate = useNavigate();

    useEffect(() => {
        fetchAuthors();
    }, [currentPage]);

    const fetchAuthors = async () => {
        try {
            const response = await axios.get(
                `${API_BASE_URL}/authors?PageIndex=${currentPage}&PageSize=4`
            );
            setAuthors(response.data.items || []);
            setTotalPages(response.data.totalPages);
            setIsLoading(false);
        } catch (error) {
            console.error("Ошибка при загрузке списка авторов:", error);
            alert("Ошибка при загрузке списка авторов.");
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setBook((prevBook) => ({ ...prevBook, [name]: value }));
    };

    const handleAuthorChange = (author) => {
        if (author) {
            setBook((prevBook) => ({ ...prevBook, authorId: author.id }));
            setSelectedAuthor(author);
        }
    };

    const handleImageChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = () => {
                const base64Data = reader.result.split(",")[1];
                setBook((prevBook) => ({
                    ...prevBook,
                    imageData: base64Data,
                    imageType: file.type,
                }));
            };
            reader.readAsDataURL(file);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!selectedAuthor) {
            setValidationErrors([
                ...validationErrors,
                {
                    property: "Author",
                    messages: ["Автор не выбран"],
                },
            ]);
            setShowValidationErrors(true);
            return;
        }
        try {
            const requestBody = {
                title: book.title,
                authorId: book.authorId,
                genre: book.genre,
                isbn: book.isbn,
                description: book.description,
                imageType: book.imageType,
                imageData: book.imageData,
            };
            await authorizedAxios.post(`${API_BASE_URL}/books`, requestBody);
            navigate("/books");
        } catch (error) {
            if (error.response && error.response.status === 422) {
                const errorDetails = error.response.data.details || [];
                setValidationErrors(errorDetails);
                setShowValidationErrors(true);
            } else {
                console.error("Ошибка при добавлении книги:", error);
            }
        }
    };


    const handleCancel = () => {
        navigate("/books");
    };

    const handlePageChange = (newPage) => {
        if (newPage > 0 && newPage <= totalPages) {
            setCurrentPage(newPage);
        }
    };

    const handleAddAuthorClick = () => {
        setShowAddAuthorModal(true);
    };

    const handleAuthorInputChange = (e) => {
        const { name, value } = e.target;
        setNewAuthor((prev) => ({ ...prev, [name]: value }));
    };

    const handleAddAuthorSubmit = async () => {
        try {
            await authorizedAxios.post(`${API_BASE_URL}/authors`, {
                FirstName: newAuthor.firstName,
                LastName: newAuthor.lastName,
                DateOfBirth: new Date(newAuthor.dateOfBirth).toISOString(),
                Country: newAuthor.country,
            });
            setShowAddAuthorModal(false);
            setNewAuthor({ firstName: "", lastName: "", dateOfBirth: "", country: "" });
            setValidationErrors([]);
            fetchAuthors();
        } catch (error) {
            if (error.response && error.response.status === 422) {
                const errorDetails = error.response.data.details || [];
                setValidationErrors(errorDetails);
                setShowValidationErrors(true);
            } else {
                console.error("Ошибка при добавлении автора:", error);
                alert("Ошибка при добавлении автора.");
            }
        }
    };


    if (isLoading) {
        return <div>Загрузка...</div>;
    }

    return (
        <div className="book-detail-page">
            <h1 className="book-details-title">Добавление книги</h1>
            <form className="book-details-info" onSubmit={handleSubmit}>
                <div className="book-detail">
                    <label>
                        <strong>Название:</strong>
                        <input
                            type="text"
                            name="title"
                            value={book.title}
                            onChange={handleInputChange}
                            className="input-field"
                            required
                        />
                    </label>
                    <label>
                        <strong>Жанр:</strong>
                        <input
                            type="text"
                            name="genre"
                            value={book.genre}
                            onChange={handleInputChange}
                            className="input-field"
                            required
                        />
                    </label>
                    <label>
                        <strong>ISBN:</strong>
                        <input
                            type="text"
                            name="isbn"
                            value={book.isbn}
                            onChange={handleInputChange}
                            className="input-field"
                            required
                        />
                    </label>
                    <label>
                        <strong>Описание:</strong>
                        <textarea
                            name="description"
                            value={book.description}
                            onChange={handleInputChange}
                            className="input-field"
                            required
                        />
                    </label>
                    <label>
                        <strong>Картинка:</strong>
                        <input
                            type="file"
                            accept="image/*"
                            onChange={handleImageChange}
                            className="input-field"
                        />
                        {book.imageData && (
                            <div className="image-preview">
                                <img
                                    src={`data:${book.imageType};base64,${book.imageData}`}
                                    alt="Preview"
                                    className="book-details-img"
                                />
                            </div>
                        )}
                    </label>
                </div>

                <div className="authors-section">
                    <h2>Авторы</h2>
                    <div className="authors-grid">
                        {authors.map((author) => (
                            <div
                                key={author.id}
                                className={`author-card ${book.authorId === author.id ? "selected" : ""}`}
                                onClick={() => handleAuthorChange(author)}
                            >
                                {author.firstName} {author.lastName}
                            </div>
                        ))}
                    </div>
                    <div className="pagination-add">
                        <button
                            type="button"
                            disabled={currentPage === 1}
                            onClick={() => handlePageChange(currentPage - 1)}
                        >
                            Назад
                        </button>
                        <span>
                            Страница {currentPage} из {totalPages}
                        </span>
                        <button
                            type="button"
                            disabled={currentPage === totalPages}
                            onClick={() => handlePageChange(currentPage + 1)}
                        >
                            Вперед
                        </button>
                    </div>
                    <div className="add-author-section">
                        <button type="button" className="btn add-author" onClick={handleAddAuthorClick}>
                            Добавить автора
                        </button>
                    </div>
                </div>

                <div className="admin-actions">
                    <button type="submit" className="btn edit">
                        Добавить
                    </button>
                    <button type="button" onClick={handleCancel} className="btn delete">
                        Отменить
                    </button>
                </div>
            </form>

            {showValidationErrors && (
                <ValidationErrorList
                    errors={validationErrors}
                    onClose={() => setShowValidationErrors(false)}
                />
            )}

            {showAddAuthorModal && (
                <div className="modal">
                    <div className="modal-content">
                        <h2>Добавить автора</h2>
                        <label>
                            Имя:
                            <input
                                type="text"
                                name="firstName"
                                value={newAuthor.firstName}
                                onChange={handleAuthorInputChange}
                                className="input-field"
                                required
                            />
                        </label>
                        <label>
                            Фамилия:
                            <input
                                type="text"
                                name="lastName"
                                value={newAuthor.lastName}
                                onChange={handleAuthorInputChange}
                                className="input-field"
                                required
                            />
                        </label>
                        <label>
                            Дата рождения:
                            <input
                                type="date"
                                name="dateOfBirth"
                                value={newAuthor.dateOfBirth}
                                onChange={handleAuthorInputChange}
                                className="input-field"
                                required
                            />
                        </label>
                        <label>
                            Страна:
                            <input
                                type="text"
                                name="country"
                                value={newAuthor.country}
                                onChange={handleAuthorInputChange}
                                className="input-field"
                                required
                            />
                        </label>
                        <div className="modal-actions">
                            <button className="btn add" onClick={handleAddAuthorSubmit}>
                                Сохранить
                            </button>
                            <button className="btn cancel" onClick={() => {
                                setShowAddAuthorModal(false);
                                setShowValidationErrors(false);
                            }}>
                                Отмена
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default AddBook;
