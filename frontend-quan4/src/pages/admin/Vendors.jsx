import React, { useState, useEffect } from 'react';
import { Table, Button, Space, Typography, message, Modal, Form, Input, Row, Col, InputNumber, Tabs, Popconfirm, Tag, Select } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, CheckCircleOutlined, CloseCircleOutlined } from '@ant-design/icons';
import axiosClient from '../../utils/axiosClient';

const { Title } = Typography;

const ManageVendors = () => {
    // ================= STATE QUẢN LÝ DỮ LIỆU =================
    const [approvedVendors, setApprovedVendors] = useState([]);
    const [pendingVendors, setPendingVendors] = useState([]);
    const [categories, setCategories] = useState([]); // Kho chứa Danh mục cho Menu xổ xuống
    
    const [loading, setLoading] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [form] = Form.useForm();

    // ================= CÁC HÀM GỌI API (LẤY DỮ LIỆU) =================
    const fetchApprovedVendors = async () => {
        try {
            const response = await axiosClient.get('/vendors');
            setApprovedVendors(response); 
        } catch (error) {
            console.error(error);
        }
    };

    const fetchPendingVendors = async () => {
        try {
            const response = await axiosClient.get('/vendors/unapproved');
            setPendingVendors(response); 
        } catch (error) {
            console.error(error);
        }
    };

    const fetchCategories = async () => {
        try {
            // Lấy danh sách Categories từ Backend C#
            const response = await axiosClient.get('/categories');
            setCategories(response); 
        } catch (error) {
            console.error("Chưa lấy được danh mục (Có thể C# chưa có API này):", error);
        }
    };

    const fetchAllData = async () => {
        setLoading(true);
        // Chạy song song 3 luồng lấy dữ liệu cho nhanh
        await Promise.all([fetchApprovedVendors(), fetchPendingVendors(), fetchCategories()]);
        setLoading(false);
    };

    useEffect(() => {
        fetchAllData();
    }, []);

    // ================= CÁC HÀM XỬ LÝ HÀNH ĐỘNG =================
    // 1. Thêm quán mới
    const handleAddVendor = async (values) => {
        try {
            await axiosClient.post('/vendors', values);
            message.success('Thêm quán thành công! Quán đã được đưa vào danh sách Chờ duyệt.');
            setIsModalOpen(false);
            form.resetFields();
            fetchPendingVendors(); // Cập nhật lại tab Chờ duyệt
        } catch (error) {
            message.error('Lỗi khi thêm quán ăn mới! Hãy kiểm tra lại dữ liệu.');
        }
    };

    // 2. Phê duyệt quán
    const handleApprove = async (id) => {
        try {
            await axiosClient.put(`/vendors/${id}/approve`);
            message.success('Đã duyệt quán ăn lên hệ thống!');
            fetchAllData(); 
        } catch (error) {
            message.error('Lỗi khi duyệt quán!');
        }
    };

    // 3. Xóa / Từ chối quán
    const handleDelete = async (id) => {
        try {
            await axiosClient.delete(`/vendors/${id}`);
            message.success('Đã xóa thành công!');
            fetchAllData();
        } catch (error) {
            message.error('Lỗi khi xóa! Có thể quán này đang chứa dữ liệu liên quan.');
        }
    };

    // ================= KHAI BÁO CỘT CHO BẢNG =================
    // Cột cho Tab 1: Đang hoạt động
    const approvedColumns = [
        { title: 'Tên quán', dataIndex: 'name', key: 'name', className: 'font-semibold' },
        { title: 'Mức giá', dataIndex: 'priceRange', key: 'priceRange' },
        { title: 'Giờ mở', dataIndex: 'openTime', key: 'openTime' },
        { title: 'Giờ đóng', dataIndex: 'closeTime', key: 'closeTime' },
        { title: 'Trạng thái', key: 'status', render: () => <Tag color="green">Đang hoạt động</Tag> },
        {
            title: 'Hành động',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <Button type="primary" ghost icon={<EditOutlined />} size="small" />
                    <Popconfirm
                        title="Xóa quán ăn này?"
                        description={`Bạn có chắc chắn muốn xóa vĩnh viễn "${record.name}" không?`}
                        onConfirm={() => handleDelete(record.id)}
                        okText="Xóa luôn"
                        cancelText="Hủy"
                        okButtonProps={{ danger: true }}
                    >
                        <Button danger icon={<DeleteOutlined />} size="small" />
                    </Popconfirm>
                </Space>
            ),
        },
    ];

    // Cột cho Tab 2: Chờ duyệt
    const pendingColumns = [
        { title: 'Tên quán', dataIndex: 'name', key: 'name', className: 'font-semibold text-gray-500' },
        { title: 'Người tạo', dataIndex: 'ownerName', key: 'ownerName' },
        { title: 'Trạng thái', key: 'status', render: () => <Tag color="orange">Chờ duyệt</Tag> },
        {
            title: 'Hành động',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <Popconfirm
                        title="Duyệt quán ăn này?"
                        description={`Cho phép "${record.name}" hiển thị trên ứng dụng?`}
                        onConfirm={() => handleApprove(record.id)}
                        okText="Duyệt ngay"
                        cancelText="Hủy"
                        okButtonProps={{ className: "bg-green-500" }}
                    >
                        <Button type="primary" className="bg-green-500 hover:bg-green-600" icon={<CheckCircleOutlined />} size="small">
                            Duyệt
                        </Button>
                    </Popconfirm>

                    <Popconfirm
                        title="Từ chối quán ăn này?"
                        description={`Bạn có chắc muốn từ chối và xóa bỏ yêu cầu "${record.name}"?`}
                        onConfirm={() => handleDelete(record.id)}
                        okText="Từ chối"
                        cancelText="Hủy"
                        okButtonProps={{ danger: true }}
                    >
                        <Button danger icon={<CloseCircleOutlined />} size="small">
                            Từ chối
                        </Button>
                    </Popconfirm>
                </Space>
            ),
        },
    ];

    const tabItems = [
        {
            key: '1',
            label: 'Quán đang hoạt động',
            children: <Table columns={approvedColumns} dataSource={approvedVendors} rowKey="id" loading={loading} pagination={{ pageSize: 8 }} />
        },
        {
            key: '2',
            label: `Chờ duyệt (${pendingVendors.length})`,
            children: <Table columns={pendingColumns} dataSource={pendingVendors} rowKey="id" loading={loading} pagination={{ pageSize: 8 }} />
        }
    ];

    return (
        <div>
            {/* Header của trang */}
            <div className="flex justify-between items-center mb-6">
                <Title level={3} className="m-0 text-gray-800">Quản lý Quán ăn</Title>
                <Button type="primary" icon={<PlusOutlined />} className="bg-black hover:bg-gray-800 h-10 px-4 rounded-lg" onClick={() => setIsModalOpen(true)}>
                    Thêm Quán ăn
                </Button>
            </div>
            
            {/* Thanh Tabs phân chia 2 bảng */}
            <Tabs defaultActiveKey="1" items={tabItems} className="bg-white p-4 rounded-lg shadow-sm border border-gray-100" />

            {/* Cửa sổ Modal Thêm quán ăn */}
            <Modal
                title={<div className="text-xl mb-4">Thêm Quán Ăn Mới</div>}
                open={isModalOpen}
                onCancel={() => { setIsModalOpen(false); form.resetFields(); }}
                onOk={() => form.submit()}
                okText="Lưu dữ liệu"
                cancelText="Hủy"
                okButtonProps={{ className: "bg-black" }}
                width={700}
            >
                <Form form={form} layout="vertical" onFinish={handleAddVendor}>
                    {/* Hàng 1: Tên quán và Địa chỉ */}
                    <Row gutter={16}>
                        <Col span={12}>
                            <Form.Item name="name" label="Tên quán" rules={[{ required: true, message: 'Bắt buộc nhập' }]}>
                                <Input placeholder="Nhập tên quán..." />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            {/* --- Ô NHẬP ĐỊA CHỈ MỚI THÊM VÀO ĐÂY --- */}
                            <Form.Item name="address" label="Địa chỉ cụ thể" rules={[{ required: true, message: 'Bắt buộc nhập địa chỉ' }]}>
                                <Input placeholder="Ví dụ: 123 Đường Tôn Đản, Quận 4..." />
                            </Form.Item>
                        </Col>
                    </Row>
                    
                    {/* Hàng 2: Danh mục và Mức giá */}
                    <Row gutter={16}>
                        <Col span={12}>
                            <Form.Item name="categoryId" label="Danh mục quán" rules={[{ required: true, message: 'Vui lòng chọn danh mục' }]}>
                                <Select placeholder="-- Chọn danh mục --" className="w-full">
                                    {categories.map((cat) => (
                                        <Select.Option key={cat.id} value={cat.id}>
                                            {cat.name}
                                        </Select.Option>
                                    ))}
                                </Select>
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item name="priceRange" label="Mức giá" rules={[{ required: true, message: 'Bắt buộc nhập' }]}>
                                <Input placeholder="VD: 50.000đ - 100.000đ" />
                            </Form.Item>
                        </Col>
                    </Row>
                    
                    <Form.Item name="description" label="Mô tả">
                        <Input.TextArea rows={3} placeholder="Mô tả điểm nổi bật của quán..." />
                    </Form.Item>
                    
                    {/* Hàng 3: Tọa độ bản đồ */}
                    <Row gutter={16}>
                        <Col span={12}>
                            <Form.Item name="latitude" label="Vĩ độ (Latitude)" rules={[{ required: true, message: 'Bắt buộc nhập' }]}>
                                <InputNumber className="w-full" placeholder="VD: 10.76" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item name="longitude" label="Kinh độ (Longitude)" rules={[{ required: true, message: 'Bắt buộc nhập' }]}>
                                <InputNumber className="w-full" placeholder="VD: 106.7" />
                            </Form.Item>
                        </Col>
                    </Row>
                    
                    {/* Hàng 4: Thời gian */}
                    <Row gutter={16}>
                        <Col span={12}>
                            <Form.Item name="openTime" label="Giờ mở cửa" rules={[{ required: true, message: 'Bắt buộc nhập' }]}>
                                <Input placeholder="VD: 08:00" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item name="closeTime" label="Giờ đóng cửa" rules={[{ required: true, message: 'Bắt buộc nhập' }]}>
                                <Input placeholder="VD: 22:00" />
                            </Form.Item>
                        </Col>
                    </Row>
                </Form>
            </Modal>
        </div>
    );
};

export default ManageVendors;