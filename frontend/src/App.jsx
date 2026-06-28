import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useState } from 'react';

// =====ADMIN=====
import Login from './admin_features/pages/Login';
import Dashboard from './admin_features/pages/Dashboard';
import EateryManagement from './admin_features/pages/EateryManagement';
import UserManagement from './admin_features/pages/UserManagement';
import MenuManagement from './admin_features/pages/MenuManagement'; // <-- THÊM DÒNG NÀY
import AdminLayout from './components/AdminLayout';

// =====OWNER=====
import OwnerDashboard from './owner_features/page/DashBoard';
import OwnerLogin from './owner_features/page/Login';

// =====TOURIST=====
import Home from './user_features/pages/Home';

function OwnerArea() {
    const [owner, setOwner] = useState(() => {
        const saved = localStorage.getItem("owner_info")
        return saved ? JSON.parse(saved) : null
    })

    const handleLogin = (ownerData) => {
        setOwner(ownerData)
        localStorage.setItem("owner_info", JSON.stringify(ownerData))
    }

    const handleLogout = () => {
        setOwner(null)
        localStorage.removeItem("owner_info")
        localStorage.removeItem("owner_token")
    }

    if (!owner) return <OwnerLogin onLogin={handleLogin} />
    return <OwnerDashboard owner={owner} onLogout={handleLogout} />
}

const ProtectedRoute = ({ children }) => {
  const token = localStorage.getItem('foodtour_admin_token');
  if (!token) return <Navigate to="/login" replace />;
  return <AdminLayout>{children}</AdminLayout>;
};

// export default function App() {
//   return (
//     <BrowserRouter>
//       <Routes>
//         {/*ADMIN ROUTE*/}
//         <Route path="/login" element={<Login />} />

//         <Route path="/" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
//         <Route path="/eateries" element={<ProtectedRoute><EateryManagement /></ProtectedRoute>} />
//         <Route path="/users" element={<ProtectedRoute><UserManagement /></ProtectedRoute>} />
//         <Route path="/menu" element={<ProtectedRoute><MenuManagement /></ProtectedRoute>} /> {/* <-- THÊM ĐƯỜNG DẪN VIP NÀY */}

//         <Route path="*" element={<Navigate to="/" replace />} />

//         {/*TOURIST ROUTE*/}
//         <Route path="/tourist" element={<Home/>} />
//       </Routes>
//     </BrowserRouter>
//   );
// }

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/tourist" element={<Home />} />


                <Route path="/login" element={<Login />} />
                <Route path="/" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
                <Route path="/eateries" element={<ProtectedRoute><EateryManagement /></ProtectedRoute>} />
                <Route path="/users" element={<ProtectedRoute><UserManagement /></ProtectedRoute>} />
                <Route path="/menu" element={<ProtectedRoute><MenuManagement /></ProtectedRoute>} /> {/* <-- THÊM ĐƯỜNG DẪN VIP NÀY */}
                
                <Route path="/owner" element={<OwnerArea />} />
                <Route path="/owner/login" element={<OwnerArea />} />
                <Route path="/" element={<Navigate to="/tourist" replace />} />
            </Routes>
        </BrowserRouter>
    )
}

//export default App