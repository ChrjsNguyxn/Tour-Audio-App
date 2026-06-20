import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { Card, Spin, Empty } from 'antd'; // Import đồ nghề UI
import axiosClient from './utils/axiosClient'; // Import đường ống API

function App() {
  // --- STATE LƯU TRỮ DỮ LIỆU ---
  const [vendors, setVendors] = useState([]); // Chứa danh sách quán ăn
  const [loading, setLoading] = useState(true); // Trạng thái đang xoay vòng chờ dữ liệu

  // --- HÀM GỌI API (Chạy 1 lần duy nhất khi mở trang) ---
  useEffect(() => {
    const fetchVendors = async () => {
      try {
        // Chọc sang C# lấy dữ liệu
        const data = await axiosClient.get('/vendors');
        setVendors(data);
      } catch (error) {
        console.error("Lỗi khi tải dữ liệu quán ăn:", error);
      } finally {
        setLoading(false); // Dù lỗi hay thành công cũng tắt vòng xoay
      }
    };

    fetchVendors();
  }, []);

  return (
    <Router>
      <Routes>
        {/* --- TRANG CHỦ HIỂN THỊ DANH SÁCH QUÁN ĂN --- */}
        <Route path="/" element={
          <div className="min-h-screen bg-gray-50 p-10">
            <h1 className="text-4xl font-extrabold text-orange-500 mb-8 text-center drop-shadow-sm">
              Đặc sản Ẩm thực Quận 4
            </h1>

            {/* Nếu đang tải thì hiện vòng xoay, nếu không có data thì báo trống */}
            {loading ? (
              <div className="flex justify-center mt-20"><Spin size="large" /></div>
            ) : vendors.length === 0 ? (
              <Empty description="Hiện chưa có quán ăn nào được duyệt" />
            ) : (
              // Vòng lặp in danh sách quán ăn ra giao diện
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {vendors.map((vendor) => (
                  <Card 
                    key={vendor.Id} 
                    title={<span className="text-xl font-bold text-gray-800">{vendor.Name}</span>}
                    bordered={false}
                    className="shadow-md hover:shadow-xl transition-shadow duration-300 rounded-2xl"
                  >
                    <p className="text-gray-600 mb-2 font-medium">Danh mục: <span className="text-blue-500">{vendor.CategoryName}</span></p>
                    <p className="text-red-500 font-bold mb-2">Giá: {vendor.PriceRange}</p>
                    <p className="text-gray-500 line-clamp-2">{vendor.Description || "Đang cập nhật mô tả..."}</p>
                  </Card>
                ))}
              </div>
            )}
          </div>
        } />

        {/* Các trang dự phòng */}
        <Route path="/admin" element={<h2 className="text-center mt-10 text-2xl">Trang Admin</h2>} />
      </Routes>
    </Router>
  );
}

export default App;