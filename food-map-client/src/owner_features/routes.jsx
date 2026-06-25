import { useState } from 'react'
import { Route, Navigate } from 'react-router-dom'

import Login from './pages/Login'
import Dashboard from './pages/Dashboard'

// Component bọc để quản lý state đăng nhập riêng cho Owner
function OwnerArea() {
    const [owner, setOwner] = useState(null)

    return owner
        ? <Dashboard owner={owner} onLogout={() => setOwner(null)} />
        : <Login onLogin={setOwner} />
}

// Export ra danh sách Route để App.jsx render trực tiếp trong <Routes>
const ownerRoutes = [
    <Route key="owner-area" path="/owner/*" element={<OwnerArea />} />
]

export default ownerRoutes
