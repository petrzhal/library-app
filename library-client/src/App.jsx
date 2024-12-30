import React from 'react';
import { Routes, Route, BrowserRouter } from 'react-router-dom';
import Layout from './Components/Layout/Layout.jsx';
import Register from './Components/Register/Register.jsx';
import Login from './Components/Login/Login.jsx';
import BooksPage from './Components/Books/BooksPage.jsx';
import BookDetails from './Components/BookDetails/BookDetails.jsx';
import EditBook from './Components/EditBook/EditBook.jsx';
import AddBook from './Components/AddBook/AddBook.jsx';
import AdminRoute from './Components/AdminRoute/AdminRoute.jsx';
import Forbidden from './Components/Forbidden/Forbidden.jsx';
import { AuthProvider } from './AuthContext.js';
import BorrowedBooksPage from './Components/BorrowedBooksPage/BorrowedBooksPage.jsx';

const App = () => {
    return (
        <BrowserRouter>
            <AuthProvider>
                <Layout>
                    <Routes>
                        <Route
                            path="/edit-book/:bookId"
                            element={
                                <AdminRoute>
                                    <EditBook />
                                </AdminRoute>
                            }
                        />
                        <Route
                            path="/add-book"
                            element={
                                <AdminRoute>
                                    <AddBook />
                                </AdminRoute>
                            }
                        />
                        <Route path="/login" element={<Login />} />
                        <Route path="/register" element={<Register />} />
                        <Route path="/" element={<BooksPage />} />
                        <Route path="/books" element={<BooksPage />} />
                        <Route path="/books/:bookId" element={<BookDetails />} />
                        <Route path='/borrowed-books' element={<BorrowedBooksPage />} />
                        <Route path="/forbidden" element={<Forbidden />} />
                    </Routes>
                </Layout>
            </AuthProvider>
        </BrowserRouter>
    );
};

export default App;
