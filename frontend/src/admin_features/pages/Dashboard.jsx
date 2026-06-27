import React, { useState, useEffect } from 'react';
import api from '../services/api';
import { Users, Store, Clock, Utensils, Star } from 'lucide-react';

export default function Dashboard() {
    // Khởi tạo các biến chứa dữ liệu thống kê
    const [stats, setStats] = useState({
        totalUsers: 0,
        totalEateries: 0,
        totalPendingEateries: 0,
        totalMenuItems: 0,
        totalReviews: 0
    });

    // useEffect chạy đúng 1 lần khi mở trang để gọi API
    useEffect(() => {
        const fetchStats = async () => {
            try {
                // Gọi API siêu ngắn gọn nhờ trạm thu phát đã làm ở Bước 2
                const response = await api.get('/dashboard/stats');
                setStats(response.data);
            } catch (error) {
                console.error("Lỗi lấy dữ liệu thống kê:", error);
            }
        };

        fetchStats();
    }, []);

    return (
        <div className="p-6">
            <h1 className="text-2xl font-bold text-gray-800 mb-6">Tổng quan hệ thống</h1>
            
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {/* Thẻ Users */}
                <div className="bg-white rounded-lg shadow p-6 flex items-center">
                    <div className="p-3 rounded-full bg-blue-100 text-blue-500 mr-4">
                        <Users size={28} />
                    </div>
                    <div>
                        <p className="text-gray-500 text-sm">Người dùng</p>
                        <p className="text-2xl font-bold text-gray-800">{stats.totalUsers}</p>
                    </div>
                </div>

                {/* Thẻ Quán ăn */}
                <div className="bg-white rounded-lg shadow p-6 flex items-center">
                    <div className="p-3 rounded-full bg-green-100 text-green-500 mr-4">
                        <Store size={28} />
                    </div>
                    <div>
                        <p className="text-gray-500 text-sm">Tổng quán ăn</p>
                        <p className="text-2xl font-bold text-gray-800">{stats.totalEateries}</p>
                    </div>
                </div>

                {/* Thẻ Quán chờ duyệt */}
                <div className="bg-white rounded-lg shadow p-6 flex items-center">
                    <div className="p-3 rounded-full bg-yellow-100 text-yellow-600 mr-4">
                        <Clock size={28} />
                    </div>
                    <div>
                        <p className="text-gray-500 text-sm">Chờ kiểm duyệt</p>
                        <p className="text-2xl font-bold text-gray-800">{stats.totalPendingEateries}</p>
                    </div>
                </div>
            </div>
        </div>
    );
}