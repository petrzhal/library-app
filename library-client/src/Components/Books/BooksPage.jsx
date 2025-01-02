import React, { useState, useEffect } from "react";
import axios from "axios";
import "./BooksPage.css";
import API_BASE_URL from "../../config";
import { useNavigate } from 'react-router-dom';

const BooksPage = () => {
    const [books, setBooks] = useState([]);
    const [authors, setAuthors] = useState([]);
    const [filters, setFilters] = useState({
        title: "",
        genre: "",
        authorId: null,
    });
    const [tempFilters, setTempFilters] = useState({ ...filters });
    const [pagination, setPagination] = useState({ pageIndex: 1, pageSize: 3 });
    const [authorPagination, setAuthorPagination] = useState({ pageIndex: 1, pageSize: 5 });
    const [isAdmin, setIsAdmin] = useState(false);
    const [imagesLoaded, setImagesLoaded] = useState(false); // New state for image loading
    const navigate = useNavigate();

    useEffect(() => {
        const user = JSON.parse(localStorage.getItem("user"));
        if (user?.role === "Admin") {
            setIsAdmin(true);
        }
        fetchBooks();
    }, [filters, pagination.pageIndex]);

    useEffect(() => {
        fetchAuthors();
    }, [authorPagination.pageIndex]);

    const fetchBooks = async () => {
        try {
            const response = await axios.get(`${API_BASE_URL}/books`, {
                params: {
                    PageIndex: pagination.pageIndex,
                    PageSize: pagination.pageSize,
                    Genre: filters.genre || "",
                    AuthorId: filters.authorId || null,
                    Title: filters.title || "",
                },
            });

            const booksData = response.data.items;

            const booksWithImages = await Promise.all(
                booksData.map(async (book) => {
                    let imageData = null;
                    let imageType = null;

                    if (book.imageId) {
                        try {
                            const imageResponse = await axios.get(`${API_BASE_URL}/books/${book.id}/image`);
                            if (imageResponse.status === 200) {
                                const imageInfo = imageResponse.data;
                                imageData = imageInfo.imageData;
                                imageType = imageInfo.imageType;
                            }
                        } catch (imageError) {
                            if (imageError.response?.status === 404) {
                                console.warn(`Изображение для книги с id ${book.id} не найдено, будет использоваться стандартное изображение.`);
                            } else {
                                console.error(`Ошибка при загрузке изображения для книги с id ${book.id}:`, imageError);
                            }
                            imageData = "none";
                            imageType = "image/png";
                        }
                    }

                    return { ...book, ImageData: imageData, ImageType: imageType };
                })
            );

            setBooks(booksWithImages);
            setPagination((prev) => ({
                ...prev,
                totalPages: response.data.totalPages,
            }));

            const allImagesLoaded = booksWithImages.every(book => book.ImageData !== null);
            setImagesLoaded(allImagesLoaded);

        } catch (error) {
            console.error("Error fetching books:", error);
        }
    };

    const fetchAuthors = async () => {
        try {
            const response = await axios.get(`${API_BASE_URL}/authors`, {
                params: {
                    PageIndex: authorPagination.pageIndex,
                    PageSize: authorPagination.pageSize,
                },
            });
            setAuthors(response.data.items);
            setAuthorPagination((prev) => ({
                ...prev,
                totalPages: response.data.totalPages,
            }));
        } catch (error) {
            console.error("Error fetching authors:", error);
        }
    };

    const handleTempFilterChange = (e) => {
        const { name, value } = e.target;
        setTempFilters((prev) => ({ ...prev, [name]: value }));
    };

    const handleApplyFilters = () => {
        setFilters(tempFilters);
        setPagination((prev) => ({ ...prev, pageIndex: 1 }));
    };

    const handleResetFilters = () => {
        const defaultFilters = {
            title: "",
            genre: "",
            authorId: null,
        };
        setTempFilters(defaultFilters);
        setFilters(defaultFilters);
        setPagination((prev) => ({ ...prev, pageIndex: 1 }));
    };

    const handlePaginationChange = (direction) => {
        setPagination((prev) => ({
            ...prev,
            pageIndex: Math.max(1, Math.min(prev.pageIndex + direction, prev.totalPages)),
        }));
    };

    const handleAuthorPaginationChange = (direction) => {
        setAuthorPagination((prev) => ({
            ...prev,
            pageIndex: Math.max(1, Math.min(prev.pageIndex + direction, prev.totalPages)),
        }));
    };

    const handleAuthorFilter = (authorId) => {
        setTempFilters((prev) => ({
            ...prev,
            authorId: authorId === tempFilters.authorId ? null : authorId,
        }));
    };

    const handleDetailsClick = (bookId) => {
        navigate(`/books/${bookId}`);
    };

    const handleAddBookClick = () => {
        navigate('/add-book');
    };

    if (!imagesLoaded) {
        return <div>Загрузка книг и изображений...</div>;
    }

    return (
        <div className="books-page">
            <div className="content-wrapper">
                <div className="main-content">
                    <h1 className="books-title">Книги</h1>
                    {isAdmin && (
                        <button onClick={handleAddBookClick} className="add-book-button">
                            Добавить книгу
                        </button>
                    )}
                    <div className="filters-container">
                        <input
                            type="text"
                            name="title"
                            placeholder="Поиск по названию"
                            value={tempFilters.title}
                            onChange={handleTempFilterChange}
                            className="input-field"
                        />
                        <input
                            type="text"
                            name="genre"
                            placeholder="Фильтр по жанру"
                            value={tempFilters.genre}
                            onChange={handleTempFilterChange}
                            className="input-field"
                        />
                        <div className="filters-buttons">
                            <button onClick={handleApplyFilters} className="apply-filters-button">
                                Применить
                            </button>
                            <button onClick={handleResetFilters} className="reset-filters-button">
                                Сбросить
                            </button>
                        </div>
                    </div>

                    {books.length === 0 ? (
                        <div className="empty-message">Нет в наличии.</div>
                    ) : (
                        books.map((book) => (
                            <ul className="books-list" key={book.id}>
                                <li className="book-item">
                                    <div className="book-image">
                                        <img src={`data:${book.ImageType};base64,${book.ImageData}`} alt={book.title} className="book-img" />
                                    </div>
                                    <div className="book-details">
                                        <h3 className="book-title">{book.title}</h3>
                                        <p><strong>Жанр:</strong> {book.genre}</p>
                                        <p><strong>Автор:</strong> {book.author.firstName} {book.author.lastName}</p>
                                    </div>
                                    <div className="book-actions">
                                        <button onClick={() => handleDetailsClick(book.id)} className="btn details">Подробнее</button>
                                    </div>
                                </li>
                            </ul>
                        ))
                    )}
                    <div className="pagination">
                        <button
                            className="pagination-btn"
                            onClick={() => handlePaginationChange(-1)}
                            disabled={pagination.pageIndex === 1 || pagination.totalPages === 0}
                        >
                            &lt;&lt;
                        </button>
                        {Array.from({ length: pagination.totalPages || 1 }, (_, index) => (
                            <button
                                key={index + 1}
                                className={`pagination-number ${pagination.pageIndex === index + 1 ? "active" : ""}`}
                                onClick={() => setPagination((prev) => ({ ...prev, pageIndex: index + 1 }))}
                            >
                                {index + 1}
                            </button>
                        ))}
                        <button
                            className="pagination-btn"
                            onClick={() => handlePaginationChange(1)}
                            disabled={pagination.pageIndex === pagination.totalPages || pagination.totalPages === 0}
                        >
                            &gt;&gt;
                        </button>
                    </div>
                </div>
                <div className="author-picker">
                    <h2 className="author-title">Авторы</h2>
                    <ul className="author-list">
                        {authors.length === 0 ? (
                            <li>Нет доступных авторов.</li>
                        ) : (
                            authors.map((author) => (
                                <li key={author.id} className="author-item">
                                    <button
                                        className={`author-button ${tempFilters.authorId === author.id ? "selected" : ""}`}
                                        onClick={() => handleAuthorFilter(author.id)}
                                    >
                                        {author.firstName} {author.lastName}
                                    </button>
                                </li>
                            ))
                        )}
                    </ul>
                    <div className="author-pagination">
                        <button
                            className="pagination-btn"
                            onClick={() => handleAuthorPaginationChange(-1)}
                            disabled={authorPagination.pageIndex === 1 || authorPagination.totalPages === 0}
                        >
                            Назад
                        </button>
                        <button
                            className="pagination-btn"
                            onClick={() => handleAuthorPaginationChange(1)}
                            disabled={authorPagination.pageIndex === authorPagination.totalPages || authorPagination.totalPages === 0}
                        >
                            Вперед
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default BooksPage;
