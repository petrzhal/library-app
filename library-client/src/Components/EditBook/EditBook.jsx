import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";
import authorizedAxios from "../../axiosConfig.js";
import API_BASE_URL from "../../config";
import "../BookDetails/BookDetails.css";
import "./EditBook.css";
import ValidationErrorList from "../ValidationErrorList/ValidationErrorList";

const EditBook = () => {
    const { bookId } = useParams();
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
    const [validationErrors, setValidationErrors] = useState([]);
    const [showValidationErrors, setShowValidationErrors] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        fetchBookDetails();
        fetchAuthors();
    }, [bookId]);

    useEffect(() => {
        fetchAuthors();
    }, [currentPage]);

    const fetchBookDetails = async () => {
        try {
            const response = await axios.get(`${API_BASE_URL}/books/${bookId}`);
            const bookData = response.data;

            let imageData = null;
            let imageType = null;

            if (bookData.imageId) {
                try {
                    const imageResponse = await axios.get(`${API_BASE_URL}/books/${bookId}/image`);
                    if (imageResponse.status === 200) {
                        const imageInfo = imageResponse.data;
                        imageData = imageInfo.imageData;
                        imageType = imageInfo.imageType;
                    }
                } catch (imageError) {
                    console.error(`Ошибка при загрузке изображения для книги с id ${bookId}:`, imageError);
                    alert(`Ошибка при загрузке изображения для книги.`);
                }
            }

            setBook({
                id: bookData.id,
                title: bookData.title,
                authorId: bookData.author.id,
                genre: bookData.genre,
                isbn: bookData.isbn,
                description: bookData.description,
                imageId: bookData.imageId,
                imageData: imageData,
                imageType: imageType,
            });
            setSelectedAuthor(bookData.author);
            setIsLoading(false);
        } catch (error) {
            console.error("Ошибка при загрузке данных книги:", error);
            alert("Ошибка при загрузке данных книги.");
        }
    };


    const fetchAuthors = async () => {
        try {
            const response = await axios.get(
                `${API_BASE_URL}/authors?PageIndex=${currentPage}&PageSize=4`
            );
            setAuthors(response.data.items || []);
            setTotalPages(response.data.totalPages);
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
        setValidationErrors([]);
        setShowValidationErrors(false);
        try {
            const requestBody = {
                Id: bookId,
                Isbn: book.isbn,
                Title: book.title,
                Description: book.description,
                Genre: book.genre,
                AuthorId: book.authorId,
                ImageId: book.imageId,
                ImageType: book.imageType,
                ImageData: book.imageData,
            };
            console.log(requestBody);
            await authorizedAxios.put(`${API_BASE_URL}/books`, requestBody);
            navigate("/books");
        } catch (error) {
            if (error.response && error.response.status === 422) {
                const errorDetails = error.response.data.details || [];
                setValidationErrors(errorDetails);
                setShowValidationErrors(true);
            } else {
                console.error("Ошибка при обновлении книги:", error);
            }
        }
    };

    const handleCancel = () => {
        navigate(`/books/${bookId}`);
    };

    const handlePageChange = (newPage) => {
        if (newPage > 0 && newPage <= totalPages) {
            setCurrentPage(newPage);
        }
    };

    if (isLoading) {
        return <div>Загрузка...</div>;
    }

    return (
        <div className="book-detail-page">
            <h1 className="book-details-title">Редактирование книги</h1>
            <form className="book-details-info" onSubmit={handleSubmit}>
                {showValidationErrors && (
                    <ValidationErrorList
                        errors={validationErrors}
                        onClose={() => setShowValidationErrors(false)}
                    />
                )}

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
                                className={`author-card ${book.authorId === author.id ? "selected" : ""
                                    }`}
                                onClick={() => handleAuthorChange(author)}
                            >
                                {author.firstName} {author.lastName}
                            </div>
                        ))}
                    </div>
                    <div className="pagination-edit">
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
                </div>

                <div className="admin-actions">
                    <button type="submit" className="btn edit">
                        Сохранить
                    </button>
                    <button type="button" onClick={handleCancel} className="btn delete">
                        Отменить
                    </button>
                </div>
            </form>
        </div>
    );
};

export default EditBook;
