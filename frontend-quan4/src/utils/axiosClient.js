import axios from 'axios';

// 1. Khởi tạo một đường ống chung trỏ thẳng tới Backend C#
const axiosClient = axios.create({
  baseURL: 'http://localhost:5045/api/v1', // Nhớ kiểm tra lại đúng port của C# trên máy bạn nhé
  headers: {
    'Content-Type': 'application/json',
  },
});

// 2. Kẻ gác cổng trước khi gửi yêu cầu đi (Tự động gắn Token)
axiosClient.interceptors.request.use((config) => {
  // Lấy Token từ kho lưu trữ của trình duyệt (sau khi user đăng nhập)
  const token = localStorage.getItem('token');
  
  if (token) {
    // Nếu có token thì tự động đính kèm vào header
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});

// 3. Kẻ gác cổng khi nhận dữ liệu về (Xử lý lỗi tự động)
axiosClient.interceptors.response.use((response) => {
  return response.data; // Chỉ lấy phần dữ liệu lõi cho code gọn
}, (error) => {
  console.error("Lỗi gọi API:", error.response?.data || error.message);
  return Promise.reject(error);
});

export default axiosClient;