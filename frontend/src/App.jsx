import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';

import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import EateryManagement from './pages/EateryManagement';
import UserManagement from './pages/UserManagement';
import MenuManagement from './pages/MenuManagement'; // <-- THÊM DÒNG NÀY
import AdminLayout from './components/AdminLayout';

const ProtectedRoute = ({ children }) => {
  const token = localStorage.getItem('foodtour_admin_token');
  if (!token) return <Navigate to="/login" replace />;
  return <AdminLayout>{children}</AdminLayout>;
};

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />

        <Route path="/" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
        <Route path="/eateries" element={<ProtectedRoute><EateryManagement /></ProtectedRoute>} />
        <Route path="/users" element={<ProtectedRoute><UserManagement /></ProtectedRoute>} />
        <Route path="/menu" element={<ProtectedRoute><MenuManagement /></ProtectedRoute>} /> {/* <-- THÊM ĐƯỜNG DẪN VIP NÀY */}

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}