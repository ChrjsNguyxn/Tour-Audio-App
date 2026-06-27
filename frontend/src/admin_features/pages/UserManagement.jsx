import React, { useState, useEffect } from 'react';
import api from '../services/api';
import { Plus, Edit, Trash2, X, Shield, User, Mail, AlertCircle } from 'lucide-react';

export default function UserManagement() {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [showModal, setShowModal] = useState(false);
    const [modalMode, setModalMode] = useState('add');
    const [formData, setFormData] = useState({ 
        id: '', username: '', password: '', email: '', fullName: '', role: 'user' 
    });

    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [userToDelete, setUserToDelete] = useState(null);

    useEffect(() => {
        fetchUsers();
    }, []);

    const fetchUsers = async () => {
        try {
            setLoading(true);
            // ĐÃ SỬA ĐƯỜNG DẪN FIX 405:
            const response = await api.get('/auth/users'); 
            const userData = response.data.data || response.data;
            setUsers(Array.isArray(userData) ? userData : []);
            setError(null);
        } catch (err) {
            console.error("Lỗi tải danh sách người dùng:", err);
            setError("Không thể tải dữ liệu người dùng. Vui lòng kiểm tra lại Backend!");
        } finally {
            setLoading(false);
        }
    };

    const renderRoleBadge = (role) => {
        if (!role) return <span className="px-3 py-1 inline-flex text-xs font-semibold rounded-full bg-gray-100 text-gray-800 border border-gray-200">Chưa xác định</span>;

        switch (role.toLowerCase()) {
            case 'admin': return <span className="px-3 py-1 inline-flex text-xs font-semibold rounded-full bg-red-100 text-red-800 border border-red-200"><Shield size={12} className="mr-1 mt-0.5" /> Quản trị viên</span>;
            case 'owner': return <span className="px-3 py-1 inline-flex text-xs font-semibold rounded-full bg-blue-100 text-blue-800 border border-blue-200">Chủ quán</span>;
            case 'user': case 'tourist': return <span className="px-3 py-1 inline-flex text-xs font-semibold rounded-full bg-green-100 text-green-800 border border-green-200">Người dùng</span>;
            default: return <span className="px-3 py-1 inline-flex text-xs font-semibold rounded-full bg-gray-100 text-gray-800 border border-gray-200">{role}</span>;
        }
    };

    const openAddModal = () => {
        setModalMode('add');
        setFormData({ id: '', username: '', password: '', email: '', fullName: '', role: 'user' });
        setShowModal(true);
    };

    const openEditModal = (user) => {
        setModalMode('edit');
        setFormData({ ...user, password: '' });
        setShowModal(true);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (modalMode === 'add') await api.post('/auth/register', formData); 
            else await api.put(`/user/${formData.id}`, formData);
            setShowModal(false);
            fetchUsers();
        } catch (err) {
            alert(err.response?.data?.message || "Có lỗi xảy ra khi lưu thông tin người dùng!");
        }
    };

    const openDeleteConfirm = (user) => {
        setUserToDelete(user);
        setShowDeleteModal(true);
    };

    const confirmDelete = async () => {
        try {
            await api.delete(`/user/${userToDelete.id}`);
            setShowDeleteModal(false);
            fetchUsers();
        } catch (err) {
            alert("Không thể xóa người dùng này!");
        }
    };

    return (
        <div className="p-6 h-full flex flex-col">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-800">Quản lý Người dùng</h1>
                <button onClick={openAddModal} className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center shadow-sm transition">
                    <Plus size={18} className="mr-2" /> Thêm Người dùng
                </button>
            </div>
            {error && <div className="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 mb-6 rounded">{error}</div>}
            <div className="bg-white rounded-lg shadow overflow-hidden flex-1">
                <div className="overflow-x-auto">
                    <table className="min-w-full divide-y divide-gray-200">
                        <thead className="bg-gray-50">
                            <tr>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Người dùng</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Liên hệ</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Quyền / Vai trò</th>
                                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Thao tác</th>
                            </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                            {loading ? (
                                <tr><td colSpan="4" className="px-6 py-10 text-center">Đang tải dữ liệu...</td></tr>
                            ) : users.length === 0 ? (
                                <tr><td colSpan="4" className="px-6 py-10 text-center text-gray-500">Chưa có người dùng nào.</td></tr>
                            ) : (
                                users.map((user) => (
                                    <tr key={user.id} className="hover:bg-gray-50">
                                        <td className="px-6 py-4 whitespace-nowrap">
                                            <div className="flex items-center">
                                                <div className="flex-shrink-0 h-10 w-10 bg-gray-200 rounded-full flex items-center justify-center text-gray-500"><User size={20} /></div>
                                                <div className="ml-4">
                                                    <div className="text-sm font-bold text-gray-900">{user.fullName || user.username}</div>
                                                    <div className="text-xs text-gray-500">@{user.username}</div>
                                                </div>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap">
                                            <div className="flex items-center text-sm text-gray-600"><Mail size={14} className="mr-2 text-gray-400" />{user.email || 'Chưa cập nhật'}</div>
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap">{renderRoleBadge(user.role)}</td>
                                        <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium space-x-2">
                                            <button onClick={() => openEditModal(user)} className="text-gray-600 hover:text-blue-600 bg-gray-100 p-2 rounded transition" title="Sửa"><Edit size={16} /></button>
                                            <button onClick={() => openDeleteConfirm(user)} className="text-red-600 hover:text-red-900 bg-red-50 p-2 rounded transition" title="Xóa"><Trash2 size={16} /></button>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>

            {/* MODAL THÊM / SỬA */}
            {showModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50 p-4">
                    <div className="bg-white rounded-lg p-6 w-full max-w-md shadow-xl">
                        <div className="flex justify-between items-center mb-5 border-b pb-3">
                            <h2 className="text-xl font-bold text-gray-800">{modalMode === 'add' ? 'Thêm Người dùng mới' : 'Chỉnh sửa Người dùng'}</h2>
                            <button onClick={() => setShowModal(false)} className="text-gray-500 hover:text-red-500 transition"><X size={24} /></button>
                        </div>
                        <form onSubmit={handleSubmit} className="space-y-4">
                            <div><label className="block text-sm font-medium text-gray-700 mb-1">Tên hiển thị</label><input type="text" required value={formData.fullName} onChange={(e) => setFormData({...formData, fullName: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none"/></div>
                            <div><label className="block text-sm font-medium text-gray-700 mb-1">Tên đăng nhập *</label><input type="text" required disabled={modalMode === 'edit'} value={formData.username} onChange={(e) => setFormData({...formData, username: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100 disabled:text-gray-500"/></div>
                            <div><label className="block text-sm font-medium text-gray-700 mb-1">Email</label><input type="email" value={formData.email} onChange={(e) => setFormData({...formData, email: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none"/></div>
                            <div><label className="block text-sm font-medium text-gray-700 mb-1">{modalMode === 'add' ? 'Mật khẩu *' : 'Mật khẩu mới (Để trống nếu không đổi)'}</label><input type="password" required={modalMode === 'add'} value={formData.password} onChange={(e) => setFormData({...formData, password: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none"/></div>
                            <div><label className="block text-sm font-medium text-gray-700 mb-1">Phân quyền</label><select value={formData.role} onChange={(e) => setFormData({...formData, role: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none"><option value="user">Người dùng (Tourist)</option><option value="owner">Chủ quán (Owner)</option><option value="admin">Quản trị viên (Admin)</option></select></div>
                            <div className="flex justify-end space-x-3 pt-4 mt-2"><button type="button" onClick={() => setShowModal(false)} className="px-4 py-2 border rounded hover:bg-gray-50 font-medium">Hủy</button><button type="submit" className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 font-medium">Lưu thông tin</button></div>
                        </form>
                    </div>
                </div>
            )}

            {/* MODAL XÓA */}
            {showDeleteModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50 p-4">
                    <div className="bg-white rounded-lg p-6 w-[400px] shadow-xl text-center">
                        <AlertCircle size={48} className="mx-auto text-red-500 mb-4" />
                        <h2 className="text-xl font-bold mb-2">Xóa tài khoản?</h2>
                        <p className="text-gray-600 mb-6 text-sm">Bạn chắc chắn muốn xóa vĩnh viễn <strong>{userToDelete?.username}</strong>?</p>
                        <div className="flex justify-center space-x-3">
                            <button onClick={() => setShowDeleteModal(false)} className="px-5 py-2 border rounded hover:bg-gray-50 font-medium">Hủy</button>
                            <button onClick={confirmDelete} className="px-5 py-2 bg-red-600 text-white rounded hover:bg-red-700 font-medium">Xóa vĩnh viễn</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}