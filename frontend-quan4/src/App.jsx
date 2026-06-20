import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import AdminLogin from './pages/admin/Login.jsx';
import AdminDashboard from './pages/admin/Dashboard.jsx';
import ProtectedRoute from './routes/ProtectedRoute.jsx';
import AdminLayout from './components/AdminLayout.jsx'; // 1. Import cái vỏ bọc vào
import ManageVendors from './pages/admin/Vendors.jsx';
function App() {
  return (
    <Router>
        <Routes>
            {/* 1. VÙNG TỰ DO: Ai cũng vào được */}
            <Route path="/admin/login" element={<AdminLogin />} />

            {/* Vùng Cấm: Phải có thẻ VIP */}
            <Route element={<ProtectedRoute />}>
                
              {/* 2. Khoác cái áo AdminLayout cho tất cả các trang bên trong */}
              <Route element={<AdminLayout />}>
                <Route path="/admin/dashboard" element={<AdminDashboard />} />
                    
                {/* Chỗ này để sẵn chờ team code thêm trang nhé */}
                <Route path="/admin/vendors" element={<ManageVendors />} /> {/* Thêm dòng này */}
                {/* <Route path="/admin/users" element={<ManageUsers />} /> */}
              </Route>

            </Route>

            {/* 3. Lạc đường: Nếu gõ link bậy bạ, tự động bế về trang đăng nhập */}
            <Route path="*" element={<Navigate to="/admin/login" />} />
        </Routes>
    </Router>
  );
}

export default App;