import React, { useState, useEffect } from "react";
import axios from "axios";
import "./BorrowedBooksPage.css";
import API_BASE_URL from "../../config";
import { useNavigate } from 'react-router-dom';
import authorizedAxios from "../../axiosConfig";

const BorrowedBooksPage = () => {
    const [borrowedBooks, setBorrowedBooks] = useState([]);
    const [pagination, setPagination] = useState({ pageIndex: 1, pageSize: 3 });
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        fetchBorrowedBooks();
    }, [pagination.pageIndex]);

    const fetchBorrowedBooks = async () => {
        try {
            setIsLoading(true);
            const params = {
                PageIndex: pagination.pageIndex,
                PageSize: pagination.pageSize,
            };

            const response = await authorizedAxios.get(`${API_BASE_URL}/books/borrowed`, { params });
            const booksData = response.data.items;

            const booksWithImages = await Promise.all(
                booksData.map(async (book) => {
                    let imageData = null;
                    let imageType = null;

                    if (book.imageId) {
                        try {
                            const imageResponse = await axios.get(`${API_BASE_URL}/books/${book.id}/image`);
                            if (imageResponse.status === 200) {
                                imageData = imageResponse.data.imageData;
                                imageType = imageResponse.data.imageType;
                            }
                        } catch (imageError) {
                            console.error(`Ошибка при загрузке изображения для книги с id ${book.id}:`, imageError);
                        }
                    }

                    return {
                        ...book,
                        imageData,
                        imageType,
                    };
                })
            );

            setBorrowedBooks(booksWithImages);
            setPagination((prev) => ({
                ...prev,
                totalPages: response.data.totalPages,
            }));
        } catch (error) {
            console.error("Error fetching borrowed books:", error);
        } finally {
            setIsLoading(false);
        }
    };


    const handleReturnBook = async (bookId) => {
        try {
            await authorizedAxios.post(`${API_BASE_URL}/books/return`, { bookId });
            fetchBorrowedBooks();
        } catch (error) {
            console.error("Error returning book:", error);
        }
    };

    const handlePaginationChange = (direction) => {
        setPagination((prev) => ({
            ...prev,
            pageIndex: Math.max(1, Math.min(prev.pageIndex + direction, prev.totalPages)),
        }));
    };

    const handleDetailsClick = (bookId) => {
        navigate(`/books/${bookId}`);
    };

    if (isLoading) {
        return <div>Загрузка...</div>;
    }

    return (
        <div className="borrowed-books-page">
            <div className="borrowed-content-wrapper">
                <h1 className="borrowed-books-title">Взятые книги</h1>
                {borrowedBooks.length === 0 ? (
                    <div className="empty-message">Нет доступных книг.</div>
                )
                    : (
                        borrowedBooks.map((book) => (
                            <ul className="borrowed-books-list">
                                <li key={book.id} className="borrowed-book-item">
                                    <div className="borrowed-book-image">
                                        <img src={`data:${book.imageType};base64,${book.imageData}`} alt={book.title} className="borrowed-book-img" />
                                    </div>
                                    <div className="borrowed-book-details">
                                        <h3 className="borrowed-book-title">{book.title}</h3>
                                        <p><strong>Жанр:</strong> {book.genre}</p>
                                    </div>
                                    <div className="borrowed-book-actions">
                                        <button onClick={() => handleDetailsClick(book.id)} className="borrowed-btn details">Подробнее</button>
                                        <button onClick={() => handleReturnBook(book.id)} className="borrowed-btn return">Вернуть</button>
                                    </div>
                                </li>
                            </ul>
                        ))
                    )
                }

                <div className="borrowed-pagination">
                    <button
                        className="borrowed-pagination-btn"
                        onClick={() => handlePaginationChange(-1)}
                        disabled={pagination.pageIndex === 1 || pagination.totalPages === 0}
                    >
                        &lt;&lt;
                    </button>
                    {Array.from({ length: pagination.totalPages || 1 }, (_, index) => (
                        <button
                            key={index + 1}
                            className={`borrowed-pagination-number ${pagination.pageIndex === index + 1 ? "active" : ""}`}
                            onClick={() => setPagination((prev) => ({ ...prev, pageIndex: index + 1 }))}
                        >
                            {index + 1}
                        </button>
                    ))}
                    <button
                        className="borrowed-pagination-btn"
                        onClick={() => handlePaginationChange(1)}
                        disabled={pagination.pageIndex === pagination.totalPages || pagination.totalPages === 0}
                    >
                        &gt;&gt;
                    </button>
                </div>
            </div>
        </div >
    );
};

export default BorrowedBooksPage;
