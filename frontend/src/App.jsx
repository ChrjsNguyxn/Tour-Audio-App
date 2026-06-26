import { useState } from "react"
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom"
import Home from "./user_features/pages/Home"
import Login from "./owner_features/page/Login.jsx"
import Dashboard from "./owner_features/page/Dashboard.jsx"
import AdminDashboard from "./admin_features/page/AdminDashboard"
import AdminQrScanner from "./admin_features/components/AdminQrScanner.jsx"
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

    if (!owner) return <Login onLogin={handleLogin} />
    return <Dashboard owner={owner} onLogout={handleLogout} />
}

// function App() {
//     return (
//         <BrowserRouter>
//             <Routes>
//                 <Route path="/tourist" element={<Home />} />

//                 {/* ---- Mới thêm cho Owner ---- */}
//                 <Route path="/owner" element={<OwnerArea />} />
//                 <Route path="/owner/login" element={<OwnerArea />} />

//                 {/* Trang gốc "/" tạm điều hướng về /tourist để không bị trắng trang */}
//                 <Route path="/" element={<Navigate to="/tourist" replace />} />
//             </Routes>
//         </BrowserRouter>
//     )
// }
function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/tourist" element={<Home />} />
                <Route path="/admin/scan" element={<AdminQrScanner />} />
                <Route path="/owner" element={<OwnerArea />} />
                <Route path="/owner/login" element={<OwnerArea />} />
                <Route path="/admin" element={<AdminDashboard />} /> { }
                <Route path="/" element={<Navigate to="/tourist" replace />} />
            </Routes>
        </BrowserRouter>
    )
}

export default App
