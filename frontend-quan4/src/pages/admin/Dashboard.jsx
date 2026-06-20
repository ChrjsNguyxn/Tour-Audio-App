import React from 'react';
import { Typography, Card, Statistic, Row, Col } from 'antd';
import { ShopOutlined, UserOutlined } from '@ant-design/icons';

const { Title } = Typography;

const AdminDashboard = () => {
    return (
        <div>
            <Title level={3} className="mb-6">Tổng quan hệ thống</Title>
            <Row gutter={16}>
                <Col span={8}>
                    <Card bordered={false} className="shadow-sm bg-blue-50">
                        <Statistic title="Tổng số Quán ăn" value={12} prefix={<ShopOutlined />} />
                    </Card>
                </Col>
                <Col span={8}>
                    <Card bordered={false} className="shadow-sm bg-green-50">
                        <Statistic title="Người dùng đăng ký" value={150} prefix={<UserOutlined />} />
                    </Card>
                </Col>
            </Row>
        </div>
    );
};

export default AdminDashboard;