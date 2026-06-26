import React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';

import { LayoutDashboard, Store, Users, LogOut, Utensils, Music } from 'lucide-react';
export default function AdminLayout({ children }) {
    const location = useLocation();
    const navigate = useNavigate();

    // Hàm xử lý Đăng xuất
    const handleLogout = () => {
        localStorage.removeItem('foodtour_admin_token');
        navigate('/login');
    };

    // Hàm kiểm tra xem menu nào đang được chọn để bôi đậm màu lên
    const isActive = (path) => location.pathname === path ? "bg-gray-800 text-blue-400" : "hover:bg-gray-800 text-white";

    return (
        <div className="flex h-screen bg-gray-100">
            {/* THÀNH PHẦN 1: SIDEBAR (Thanh menu bên trái) */}
            <div className="w-64 bg-gray-900 flex flex-col transition-all duration-300">
                <div className="p-6 text-2xl font-bold border-b border-gray-800 flex items-center text-white">
                    <Utensils className="mr-3 text-blue-400" /> FoodTour
                </div>
                
                <nav className="flex-1 px-4 py-6 space-y-2">
                    <Link to="/" className={`flex items-center px-4 py-3 rounded transition ${isActive('/')}`}>
                        <LayoutDashboard className="mr-3" size={20} /> Tổng quan
                    </Link>
                    <Link to="/eateries" className={`flex items-center px-4 py-3 rounded transition ${isActive('/eateries')}`}>
                        <Store className="mr-3" size={20} /> Quản lý Quán ăn
                    </Link>
                    <Link to="/users" className={`flex items-center px-4 py-3 rounded transition ${isActive('/users')}`}>
                        <Users className="mr-3" size={20} /> Người dùng
                    </Link>
                    <Link to="/menu" className={`flex items-center px-4 py-3 rounded transition ${isActive('/menu')}`}>
    <Music className="mr-3" size={20} /> Món ăn & Audio
</Link>
                </nav>

                <div className="p-4 border-t border-gray-800">
                    <button 
                        onClick={handleLogout}
                        className="flex items-center justify-center w-full bg-red-600 hover:bg-red-700 text-white py-2 rounded transition"
                    >
                        <LogOut className="mr-2" size={20} /> Đăng xuất
                    </button>
                </div>
            </div>

            {/* THÀNH PHẦN 2: KHU VỰC NỘI DUNG CHÍNH (Bên phải) */}
            <div className="flex-1 flex flex-col overflow-hidden">
                {/* Header Topbar */}
                <header className="bg-white shadow-sm h-16 flex items-center justify-between px-6 z-10">
                    <h1 className="text-xl font-semibold text-gray-800">Hệ thống Quản trị</h1>
                    <div className="flex items-center">
                        <span className="text-gray-600 mr-3">Xin chào, <strong>Admin</strong></span>
                        <div className="w-9 h-9 bg-blue-500 rounded-full flex items-center justify-center text-white font-bold shadow-md">
                            A
                        </div>
                    </div>
                </header>

                {/* Phần ruột biến đổi linh hoạt (Chính là thẻ {children}) */}
                <main className="flex-1 overflow-x-hidden overflow-y-auto bg-gray-50">
                    {children}
                </main>
            </div>
        </div>
    );
}