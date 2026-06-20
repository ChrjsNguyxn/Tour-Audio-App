import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

const ProtectedRoute = () => {
    // 1. Dùng đèn pin soi vào két xem có Token chưa
    const token = localStorage.getItem('admin_token');

    // 2. Nếu không có thẻ VIP, lập tức đá văng về trang Login
    if (!token) {
        return <Navigate to="/admin/login" replace />;
    }

    // 3. Nếu có thẻ, mở cổng cho đi tiếp vào các trang bên trong
    return <Outlet />;
};

export default ProtectedRoute;