import React, { useState } from 'react';
import { Layout, Menu, Button, theme } from 'antd';
import {
    DashboardOutlined,
    ShopOutlined,
    UserOutlined,
    LogoutOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined
} from '@ant-design/icons';
import { useNavigate, Outlet, useLocation } from 'react-router-dom';

const { Header, Sider, Content } = Layout;

const AdminLayout = () => {
    const [collapsed, setCollapsed] = useState(false); // Trạng thái đóng/mở menu
    const navigate = useNavigate();
    const location = useLocation(); // Lấy đường dẫn hiện tại để bôi đen menu
    const { token: { colorBgContainer, borderRadiusLG } } = theme.useToken();

    // Xử lý đăng xuất
    const handleLogout = () => {
        localStorage.removeItem('admin_token');
        navigate('/admin/login');
    };

    // Danh sách các mục trên Menu
    const menuItems = [
        { key: '/admin/dashboard', icon: <DashboardOutlined />, label: 'Tổng quan' },
        { key: '/admin/vendors', icon: <ShopOutlined />, label: 'Quản lý Quán ăn' },
        { key: '/admin/users', icon: <UserOutlined />, label: 'Tài khoản' },
        { key: 'logout', icon: <LogoutOutlined />, label: 'Đăng xuất', danger: true, onClick: handleLogout },
    ];

    // Xử lý khi bấm vào Menu
    const onMenuClick = (e) => {
        if (e.key !== 'logout') {
            navigate(e.key);
        }
    };

    return (
        <Layout className="min-h-screen">
            {/* THANH MENU BÊN TRÁI */}
            <Sider trigger={null} collapsible collapsed={collapsed} theme="dark" width={250}>
                <div className="h-16 flex items-center justify-center text-white font-bold text-xl tracking-widest bg-gray-900 shadow-md">
                    {collapsed ? 'Q4' : 'TRẠM KIỂM SOÁT'}
                </div>
                <Menu
                    theme="dark"
                    mode="inline"
                    selectedKeys={[location.pathname]}
                    items={menuItems}
                    onClick={onMenuClick}
                    className="mt-4"
                />
            </Sider>

            {/* KHU VỰC BÊN PHẢI */}
            <Layout>
                {/* Thanh Header trên cùng */}
                <Header style={{ padding: 0, background: colorBgContainer }} className="flex justify-between items-center pr-6 shadow-sm">
                    <Button
                        type="text"
                        icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
                        onClick={() => setCollapsed(!collapsed)}
                        className="w-16 h-16 text-lg hover:bg-gray-100"
                    />
                    <div className="font-semibold text-gray-700 bg-gray-100 px-4 py-1 rounded-full border border-gray-200">
                        Xin chào, Admin!
                    </div>
                </Header>

                {/* Phần ruột chứa nội dung các trang */}
                <Content style={{ margin: '24px 16px', padding: 24, background: colorBgContainer, borderRadius: borderRadiusLG }} className="shadow-sm overflow-auto">
                    {/* Thẻ Outlet này chính là "cái hố" để nhét ruột các trang khác vào */}
                    <Outlet />
                </Content>
            </Layout>
        </Layout>
    );
};

export default AdminLayout;