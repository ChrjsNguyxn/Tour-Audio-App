import React, { useState } from 'react';
import { Form, Input, Button, Card, Typography, message } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom'; 
import axiosClient from '../../utils/axiosClient'; // Đảm bảo đường dẫn này trỏ đúng tới file axiosClient của bạn

const { Title } = Typography;

const AdminLogin = () => {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false); // Thêm hiệu ứng xoay xoay lúc chờ API

    // Hàm xử lý gọi API
    const onFinish = async (values) => {
        setLoading(true);
        try {
            // Bắn request POST mang theo Username và Password
            const response = await axiosClient.post('/admin/auth/login', {
                username: values.username,
                password: values.password
            });

            // Xóa chữ .data đi vì axiosClient đã bóc sẵn rồi
localStorage.setItem('admin_token', response.token);
            message.success('Đăng nhập thành công! Đang vào hệ thống...');

            // 2. Mở cổng, đá văng user sang trang Dashboard
            navigate('/admin/dashboard');

        } catch (error) {
            // Hứng cái lỗi 401 (Sai pass) từ C# ném sang và hiện thông báo đỏ
            const errorMsg = error.response?.data || 'Đăng nhập thất bại, vui lòng thử lại!';
            message.error(errorMsg);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-900">
            <Card className="w-full max-w-md shadow-2xl border-0 rounded-2xl">
                <div className="text-center mb-8">
                    <Title level={2} className="m-0 text-gray-800">Trạm Kiểm Soát</Title>
                    <p className="text-gray-500 mt-2">Đăng nhập tài khoản Admin hệ thống</p>
                </div>

                <Form
                    name="admin_login"
                    onFinish={onFinish}
                    layout="vertical"
                    size="large"
                >
                    <Form.Item
                        name="username"
                        rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập!' }]}
                    >
                        <Input prefix={<UserOutlined className="text-gray-400" />} placeholder="Tên đăng nhập" />
                    </Form.Item>

                    <Form.Item
                        name="password"
                        rules={[{ required: true, message: 'Vui lòng nhập mật khẩu!' }]}
                    >
                        <Input.Password prefix={<LockOutlined className="text-gray-400" />} placeholder="Mật khẩu" />
                    </Form.Item>

                    <Form.Item className="mt-6">
                        <Button 
                            type="primary" 
                            htmlType="submit" 
                            loading={loading} // Bật hiệu ứng chờ khi đang gọi API
                            className="w-full h-12 bg-black hover:bg-gray-800 text-white font-semibold rounded-lg"
                        >
                            ĐĂNG NHẬP
                        </Button>
                    </Form.Item>
                </Form>
            </Card>
        </div>
    );
};

export default AdminLogin;