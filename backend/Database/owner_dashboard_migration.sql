-- ==========================================================
-- Script này CHỈ THÊM MỚI, không xóa/sửa bảng hay dữ liệu cũ.
-- Có thể chạy lại nhiều lần an toàn (idempotent).
-- ==========================================================

-- Bảng thống kê quán ăn (view / listen / favorite) — MỚI HOÀN TOÀN
CREATE TABLE IF NOT EXISTS eatery_stats (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    eatery_id INTEGER NOT NULL,
    event_type TEXT NOT NULL, -- 'view' | 'listen' | 'favorite'
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- 3 cột mới cho bảng eateries để hỗ trợ Dashboard.jsx
-- (is_open_now, narration_text, narration_language)
-- SQLite không hỗ trợ "ADD COLUMN IF NOT EXISTS" trực tiếp nên
-- Program.cs sẽ kiểm tra qua PRAGMA table_info trước khi ALTER.