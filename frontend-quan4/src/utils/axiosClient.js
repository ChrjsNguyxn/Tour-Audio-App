import axios from 'axios';

// 1. Tạo một instance của Axios với đường dẫn gốc tới Backend C#
const axiosClient = axios.create({
    baseURL: 'http://localhost:5045/api/v1', // Đảm bảo khớp cổng 5045 của C#
    headers: {
        'Content-Type': 'application/json',
    },
});

// 2. INTERCEPTOR REQUEST: "Trạm kiểm tra trước khi xuất phát"
// Trạm này sẽ tự động móc thẻ Token từ két sắt (nếu có) và gắn vào Header
axiosClient.interceptors.request.use(
    (config) => {
        // Lấy token của admin từ localStorage
        const token = localStorage.getItem('admin_token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// 3. INTERCEPTOR RESPONSE: "Trạm kiểm tra hàng trả về"
// Giúp bóc tách dữ liệu cho gọn, hoặc xử lý tự động đá văng ra Login nếu Token hết hạn
axiosClient.interceptors.response.use(
    (response) => {
        // Chỉ lấy data của response, bỏ qua các thông tin rườm rà khác của Axios
        return response.data;
    },
    (error) => {
        // Nếu Backend trả về 401 (Hết hạn token hoặc không hợp lệ)
        if (error.response && error.response.status === 401) {
            console.error("Token hết hạn hoặc không hợp lệ, vui lòng đăng nhập lại!");
            // localStorage.removeItem('admin_token');
            // window.location.href = '/admin/login'; 
        }
        return Promise.reject(error);
    }
);

export default axiosClient;