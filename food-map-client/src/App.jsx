import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'

import ownerRoutes from './owner_features/routes'
// import touristRoutes from './user_features/routes'
// import adminRoutes from './admin_features/routes'

//import Home from './user_features/pages/Home'

function App() {
    return (
        <BrowserRouter>
            <Routes>
                {/* ===== User / Tourist ===== */}
                {/* <Route path="/tourist" element={<Home />} /> */}
                {/* {touristRoutes} */}

                {/* ===== Owner / Vendor ===== */}
                {ownerRoutes}

                {/* ===== Admin ===== */}
                {/* {adminRoutes} */}

                {/* ===== Trang chủ mặc định ===== */}
                <Route path="/" element={<Navigate to="/owner" />} />

                {/* ===== 404 ===== */}
                <Route path="*" element={<div style={{ padding: 40 }}>404 — Không tìm thấy trang</div>} />
            </Routes>
        </BrowserRouter>
    )
}

export default App
