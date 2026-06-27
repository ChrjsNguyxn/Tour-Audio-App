import axios from 'axios';

// Tạo một trạm phát mặc định nối thẳng tới Backend của bạn
const api = axios.create({
    baseURL: 'http://localhost:5092/api/v1',
});

// BỘ LỌC CHIỀU ĐI (Tự động móc Token từ LocalStorage gắn vào mọi thẻ gửi đi)
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('foodtour_admin_token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, (error) => {
    return Promise.reject(error);
});

// BỘ LỌC CHIỀU VỀ (Tự động đá văng ra màn hình đăng nhập nếu Token hết hạn)
api.interceptors.response.use((response) => {
    return response;
}, (error) => {
    if (error.response && error.response.status === 401) {
        localStorage.removeItem('foodtour_admin_token');
        window.location.href = '/login'; // Ép văng ra trang đăng nhập
    }
    return Promise.reject(error);
});

export default api;