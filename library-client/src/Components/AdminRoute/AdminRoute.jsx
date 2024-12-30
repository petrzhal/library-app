import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../../AuthContext.js';

const AdminRoute = ({ children }) => {
    const { user, loading } = useAuth();

    if (loading) {
        return <div>Loading...</div>;
    }

    if (!user || user.role !== 'Admin') {
        return <Navigate to="/forbidden" />;
    }

    return children;
};

export default AdminRoute;
