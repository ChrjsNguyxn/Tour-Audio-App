import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';

export default function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault(); // Ngăn trình duyệt reload lại trang
        setError('');
        setIsLoading(true);

        try {
            // Gửi request tới Backend (Đảm bảo api.js đã cấu hình đúng baseURL là http://localhost:5092/api/v1)
            const response = await api.post('/auth/login', {
                username: username,
                password: password
            });

            // Lấy thẳng token từ response.data (Backend .NET trả về { token: "..." })
            if (response.data && response.data.token) {
                // 1. Cất Token vào két sắt localStorage
                localStorage.setItem('foodtour_admin_token', response.data.token);
                
                // (Tùy chọn) Có thể lưu thêm quyền role để Frontend phân quyền hiển thị
                if (response.data.role) {
                    localStorage.setItem('foodtour_admin_role', response.data.role);
                }

                // 2. Chuyển hướng sang trang Dashboard / Quản lý quán ăn
                navigate('/');
            } else {
                setError('Lỗi: Máy chủ không cấp phát Token.');
            }
        } catch (err) {
            console.error("Lỗi đăng nhập:", err);
            
            // Bắt chính xác câu lỗi do Backend trả về (Ví dụ: "Sai tên đăng nhập hoặc mật khẩu!")
            if (err.response && err.response.data && err.response.data.message) {
                setError(err.response.data.message);
            } else {
                setError('Không thể kết nối đến máy chủ. Vui lòng kiểm tra lại Backend!');
            }
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-md w-96">
                <h2 className="text-2xl font-bold mb-6 text-center text-gray-800">Đăng Nhập Admin</h2>
                
                <form onSubmit={handleLogin}>
                    <div className="mb-4">
                        <label className="block text-gray-700 text-sm font-bold mb-2">Tên đăng nhập</label>
                        <input 
                            type="text" 
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-2 focus:ring-blue-500"
                            required
                            autoFocus
                        />
                    </div>
                    <div className="mb-6">
                        <label className="block text-gray-700 text-sm font-bold mb-2">Mật khẩu</label>
                        <input 
                            type="password" 
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-2 focus:ring-blue-500"
                            required
                        />
                    </div>
                    
                    <button 
                        type="submit" 
                        disabled={isLoading}
                        className={`w-full font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline transition duration-150 ${
                            isLoading ? 'bg-blue-300 cursor-not-allowed text-white' : 'bg-blue-500 hover:bg-blue-700 text-white'
                        }`}
                    >
                        {isLoading ? 'Đang xác thực...' : 'Đăng Nhập'}
                    </button>
                    
                    {error && (
                        <div className="mt-4 bg-red-50 border-l-4 border-red-500 p-3 rounded">
                            <p className="text-red-700 text-sm font-medium text-center">
                                {error}
                            </p>
                        </div>
                    )}
                </form>
            </div>
        </div>
    );
}