import React, { useState, useEffect } from "react";
import axios from "axios";
import authorizedAxios from "../../axiosConfig.js";
import { useParams, useNavigate } from "react-router-dom";
import API_BASE_URL from "../../config";
import "./BookDetails.css";

const BookDetails = () => {
    const { bookId } = useParams();
    const [book, setBook] = useState({
        id: null,
        title: "",
        author: {},
        genre: "",
        isbn: "",
        description: "",
        imageData: "",
        borrowedAt: null,
        returnBy: null,
        imageType: "",
    });
    const [isLoading, setIsLoading] = useState(true);
    const [isAdmin, setIsAdmin] = useState(false);
    const [showModal, setShowModal] = useState(false);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [returnDate, setReturnDate] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        const user = JSON.parse(localStorage.getItem("user"));
        if (user?.role === "Admin") {
            setIsAdmin(true);
        }
        fetchBookDetail();
    }, [bookId]);

    const fetchBookDetail = async () => {
        setIsLoading(true);
        try {
            const response = await axios.get(`${API_BASE_URL}/books/${bookId}`);
            const bookData = response.data;

            let imageData = null;
            let imageType = null;

            const imageResponse = await axios.get(`${API_BASE_URL}/books/${bookId}/image`);
            if (imageResponse.status === 200) {
                imageData = imageResponse.data.imageData;
                imageType = imageResponse.data.imageType;
            }
            console.log(bookData);
            setBook({
                id: bookData.id,
                title: bookData.title,
                author: bookData.author,
                genre: bookData.genre,
                isbn: bookData.isbn,
                description: bookData.description,
                borrowedAt: bookData.borrowedAt,
                returnBy: bookData.returnBy,
                imageId: bookData.imageId,
                imageData: imageData,
                imageType: imageType,
            });
        } catch (error) {
            console.error("Error fetching book details:", error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleTakeBook = async () => {
        try {
            const requestBody = { BookId: parseInt(bookId, 10) };
            await authorizedAxios.post(`${API_BASE_URL}/books/borrow`, requestBody);
            const updatedBook = await fetchBookDetail();
            setShowModal(true);
            setReturnDate(new Date(updatedBook.returnBy).toLocaleString());
        } catch (error) {
            console.error("Error taking book:", error);
        }
    };

    const handleCloseModal = () => setShowModal(false);

    const handleEditBook = () => navigate(`/edit-book/${bookId}`);

    const openDeleteModal = () => setShowDeleteModal(true);

    const closeDeleteModal = () => setShowDeleteModal(false);

    const confirmDeleteBook = async () => {
        try {
            await authorizedAxios.delete(`${API_BASE_URL}/books`, {
                data: { BookId: bookId },
            });
            navigate("/books");
        } catch (error) {
            console.error("Error deleting book:", error);
            alert("Ошибка при удалении книги.");
        } finally {
            closeDeleteModal();
        }
    };

    const handleBack = () => navigate(-1);

    if (isLoading) {
        return <div>Загрузка...</div>;
    }

    if (!book) {
        return <div>Книга не найдена.</div>;
    }

    return (
        <div className="book-detail-page">
            <h1 className="book-details-title">{book.title}</h1>
            <div className="book-details-info">
                <img
                    src={`data:${book.imageType};base64,${book.imageData}`}
                    alt={book.title}
                    className="book-details-img"
                />
                <div className="book-detail">
                    <p><strong>ISBN:</strong> {book.isbn}</p>
                    <p><strong>Жанр:</strong> {book.genre}</p>
                    <p><strong>Описание:</strong> {book.description}</p>
                    <p><strong>Автор:</strong> {book.author.firstName} {book.author.lastName}</p>
                    <p><strong>Дата взятия:</strong> {book.borrowedAt ? new Date(book.borrowedAt).toLocaleString() : "Не взята"}</p>
                    <p><strong>Дата возврата:</strong> {book.returnBy ? new Date(book.returnBy).toLocaleString() : "Не указана"}</p>
                    <p><strong>Статус:</strong> {book.borrowedAt ? "Занята" : "Доступна"}</p>

                    {book.borrowedAt === null && book.returnBy === null && (
                        <button onClick={handleTakeBook} className="btn take-book">
                            Взять книгу
                        </button>
                    )}

                    {isAdmin && (
                        <div className="admin-actions">
                            <button onClick={handleEditBook} className="btn edit">Редактировать</button>
                            <button onClick={openDeleteModal} className="btn delete">Удалить</button>
                        </div>
                    )}
                </div>
            </div>

            <button onClick={handleBack} className="btn back-button">Назад</button>

            {showModal && (
                <div className="borrow-modal-overlay">
                    <div className="borrow-modal">
                        <h2>Книга успешно взята!</h2>
                        <p>Пожалуйста, верните книгу через 30 дней.</p>
                        <p>Вы можете вернуть книгу в разделе "Мои книги".</p>
                        <button onClick={handleCloseModal} className="btn close-modal">Закрыть</button>
                    </div>
                </div>
            )}

            {showDeleteModal && (
                <div className="delete-modal-overlay">
                    <div className="delete-modal">
                        <h2>Подтверждение удаления</h2>
                        <p>Вы уверены, что хотите удалить эту книгу?</p>
                        <div className="modal-actions">
                            <button onClick={confirmDeleteBook} className="btn confirm-delete">Удалить</button>
                            <button onClick={closeDeleteModal} className="btn cancel-delete">Отмена</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default BookDetails;
