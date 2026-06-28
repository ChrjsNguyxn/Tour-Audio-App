import React, { useState, useEffect } from 'react';
import api from '../services/api';
import { CheckCircle, Clock, Edit, Trash2, Plus, X, Eye, MapPin, Store, Lock, Unlock, Ban, AlertCircle } from 'lucide-react';

export default function EateryManagement() {
    const [eateries, setEateries] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [filter, setFilter] = useState('all'); // 'all', 'pending', 'approved', 'locked', 'deleted'

    // --- STATE MODAL CƠ BẢN (THÊM / SỬA / XEM) ---
    const [showModal, setShowModal] = useState(false);
    const [modalMode, setModalMode] = useState('add'); 
    const [formData, setFormData] = useState({ 
        id: '', name: '', address: '', description: '', 
        categoryId: 1, priceRange: '', latitude: '', longitude: '', 
        audioFilePath: '', imagePath: '', openTime: '08:00', closeTime: '22:00', status: 'pending' 
    });

    // --- STATE MODAL CHO DANH MỤC(CATEGORY) ---
    const [categories, setCategories] = useState([]);

    // --- STATE MODAL KHÓA QUÁN ---
    const [showLockModal, setShowLockModal] = useState(false);
    const [eateryToLock, setEateryToLock] = useState(null);
    const [lockReason, setLockReason] = useState('');

    // --- STATE MODAL XÓA QUÁN ---
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [eateryToDelete, setEateryToDelete] = useState(null);
    const [deleteReason, setDeleteReason] = useState('');

    useEffect(() => {
        fetchEateries();
        fetchCategories(); // LOAD CATEGORY
    }, []);

    const fetchEateries = async () => {
        try {
            setLoading(true);
            const response = await api.get('/eatery/admin-all');
            
            // Xử lý mapping tạm thời nếu Backend chưa cập nhật field 'status'
            const mappedData = response.data.map(e => ({
                ...e,
                status: e.status || (e.isDeleted ? 'deleted' : e.isLocked ? 'locked' : e.isApproved ? 'approved' : 'pending'),
                actionReason: e.actionReason || e.reason || ''
            }));
            
            setEateries(mappedData);
            setError(null);
        } catch (err) {
            console.error("Lỗi tải danh sách:", err);
            setError("Không thể tải dữ liệu. Vui lòng thử lại!");
        } finally {
            setLoading(false);
        }
    };

    // Lấy dữ liệu category để cập nhật lên dropbox
    const fetchCategories = async () => {
        try {
            const response = await api.get("/category");

            console.log("Categories response:", response.data);
            
            setCategories(response.data);
        } catch (err) {
            console.error("Cannot load categories", err);
        }
    };

    // ================= XỬ LÝ FORM THÊM / SỬA / XEM =================
    const openAddModal = () => {
        setModalMode('add');
        setFormData({ id: '', name: '', address: '', description: '', categoryId: 1, priceRange: '', latitude: '', longitude: '', audioFilePath: '', imagePath: '', openTime: '08:00', closeTime: '22:00', status: 'pending' });
        setShowModal(true);
    };

    const openEditModal = (eatery) => {
        setModalMode('edit');
        setFormData({ ...eatery });
        setShowModal(true);
    };

    const openViewModal = (eatery) => {
        setModalMode('view');
        setFormData({ ...eatery });
        setShowModal(true);
    };

    const handleSubmitForm = async (e) => {
        e.preventDefault();
        if (modalMode === 'view') { setShowModal(false); return; }
        try {
            const payload = {
                ...formData,
                categoryId: parseInt(formData.categoryId) || 1,
                latitude: parseFloat(formData.latitude) || 0,
                longitude: parseFloat(formData.longitude) || 0
            };

            // CÁI NÀY ĐỂ LOG KIỂM TRA PAYLOAD
            console.log("===== Payload =====");
            console.log(payload);
            console.table(payload);
            
            if (modalMode === 'add') await api.post('/eatery', payload);
            else if (modalMode === 'edit'){
                 await api.put(`/eatery/${formData.id}`, payload);

                 console.log("Sending payload:", payload);
                console.log("ID:", formData.id);
            }

            setShowModal(false);
            fetchEateries();
        } catch (err) {
            //alert("Có lỗi xảy ra khi lưu dữ liệu!");
            console.error(err);

            console.log("Response:", err.response);

            alert(
                err.response?.data?.message ||
                JSON.stringify(err.response?.data) ||
                err.message
            );
        }
    };

    // ================= XỬ LÝ TRẠNG THÁI (DUYỆT / KHÓA / XÓA) =================
    
    // 1. Duyệt / Mở khóa (Chuyển thành Approved)
    const handleApproveOrUnlock = async (id) => {
        setEateries(eateries.map(e => e.id === id ? { ...e, status: 'approved', actionReason: '' } : e));
        try {
            await api.put(`/eatery/${id}/change-status`, { status: 'approved', reason: '' });
        } catch (err) {
            fetchEateries(); // Hoàn tác nếu lỗi
        }
    };

    // 2. Khóa quán (Cần lý do)
    const openLockConfirm = (eatery) => {
        setEateryToLock(eatery);
        setLockReason('');
        setShowLockModal(true);
    };

    const confirmLock = async (e) => {
        e.preventDefault();
        setEateries(eateries.map(e => e.id === eateryToLock.id ? { ...e, status: 'locked', actionReason: lockReason } : e));
        setShowLockModal(false);
        try {
            await api.put(`/eatery/${eateryToLock.id}/change-status`, { status: 'locked', reason: lockReason });
        } catch (err) {
            fetchEateries();
        }
    };

    // 3. Xóa mềm quán (Cần lý do vi phạm)
    const openDeleteConfirm = (eatery) => {
        setEateryToDelete(eatery);
        setDeleteReason('');
        setShowDeleteModal(true);
    };

    const confirmDelete = async (e) => {
        e.preventDefault();
        setEateries(eateries.map(e => e.id === eateryToDelete.id ? { ...e, status: 'deleted', actionReason: deleteReason } : e));
        setShowDeleteModal(false);
        try {
            // Thay vì dùng DELETE HTTP Method, ta dùng PUT để Soft Delete kèm lý do
            await api.put(`/eatery/${eateryToDelete.id}/change-status`, { status: 'deleted', reason: deleteReason });
        } catch (err) {
            fetchEateries();
        }
    };

    // ================= BỘ LỌC TÌM KIẾM & TAB =================
    const filteredEateries = eateries.filter(eatery => {
        if (filter === 'all') return true;
        return eatery.status === filter;
    });

    const counts = {
        pending: eateries.filter(e => e.status === 'pending').length,
        locked: eateries.filter(e => e.status === 'locked').length,
    };

    return (
        <div className="p-6 relative h-full flex flex-col">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-800">Quản lý Quán ăn</h1>
                <button onClick={openAddModal} className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center shadow-sm transition">
                    <Plus size={18} className="mr-2" /> Thêm Quán mới
                </button>
            </div>

            {/* BỘ LỌC TABS CHUYÊN NGHIỆP */}
            <div className="flex flex-wrap gap-2 mb-6 border-b pb-4">
                <button onClick={() => setFilter('all')} className={`px-4 py-2 rounded-lg font-medium transition ${filter === 'all' ? 'bg-gray-800 text-white shadow' : 'bg-gray-100 text-gray-600 hover:bg-gray-200'}`}>
                    Tất cả
                </button>
                <button onClick={() => setFilter('pending')} className={`px-4 py-2 rounded-lg font-medium transition flex items-center ${filter === 'pending' ? 'bg-yellow-500 text-white shadow' : 'bg-yellow-50 text-yellow-700 hover:bg-yellow-100'}`}>
                    Chờ duyệt {counts.pending > 0 && <span className="ml-2 bg-red-500 text-white text-xs px-2 py-0.5 rounded-full">{counts.pending}</span>}
                </button>
                <button onClick={() => setFilter('approved')} className={`px-4 py-2 rounded-lg font-medium transition ${filter === 'approved' ? 'bg-green-600 text-white shadow' : 'bg-green-50 text-green-700 hover:bg-green-100'}`}>
                    Đã duyệt
                </button>
                <button onClick={() => setFilter('locked')} className={`px-4 py-2 rounded-lg font-medium transition flex items-center ${filter === 'locked' ? 'bg-orange-500 text-white shadow' : 'bg-orange-50 text-orange-700 hover:bg-orange-100'}`}>
                    Đã khóa {counts.locked > 0 && <span className="ml-2 bg-white text-orange-600 text-xs px-2 py-0.5 rounded-full">{counts.locked}</span>}
                </button>
                <button onClick={() => setFilter('deleted')} className={`px-4 py-2 rounded-lg font-medium transition ${filter === 'deleted' ? 'bg-red-600 text-white shadow' : 'bg-red-50 text-red-700 hover:bg-red-100'}`}>
                    Đã xóa (Lưu trữ)
                </button>
            </div>

            {error && <div className="bg-red-100 border-l-4 border-red-500 text-red-700 p-4 mb-6 rounded">{error}</div>}

            {/* BẢNG DANH SÁCH */}
            <div className="bg-white rounded-lg shadow overflow-hidden flex-1">
                <div className="overflow-x-auto">
                    <table className="min-w-full divide-y divide-gray-200">
                        <thead className="bg-gray-50">
                            <tr>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Tên Quán</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Địa chỉ</th>
                                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Trạng thái / Lý do</th>
                                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Thao tác</th>
                            </tr>
                        </thead>
                        <tbody className="bg-white divide-y divide-gray-200">
                            {loading ? (
                                <tr><td colSpan="4" className="px-6 py-10 text-center">Đang tải dữ liệu...</td></tr>
                            ) : filteredEateries.length === 0 ? (
                                <tr><td colSpan="4" className="px-6 py-10 text-center text-gray-500">Chưa có quán ăn nào trong danh mục này.</td></tr>
                            ) : (
                                filteredEateries.map((eatery) => (
                                    <tr key={eatery.id} className={`transition-colors ${eatery.status === 'deleted' ? 'bg-red-50 opacity-80' : 'hover:bg-gray-50'}`}>
                                        <td className="px-6 py-4 whitespace-nowrap">
                                            <div className="flex items-center">
                                                <div className="flex-shrink-0 h-10 w-10 bg-gray-200 rounded-lg flex items-center justify-center text-gray-500 overflow-hidden">
                                                    {eatery.imagePath ? <img src={`http://localhost:5092${eatery.imagePath}`} className="w-full h-full object-cover" alt="" onError={(e)=>e.target.style.display='none'}/> : <Store size={20} />}
                                                </div>
                                                <div className="ml-4">
                                                    <div className={`text-sm font-bold ${eatery.status === 'deleted' ? 'text-red-700 line-through' : 'text-gray-900'}`}>{eatery.name}</div>
                                                    <div className="text-xs text-gray-500">ID: #{eatery.id}</div>
                                                </div>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4">
                                            <div className="text-sm text-gray-700 flex items-start">
                                                <MapPin size={16} className="mr-1 mt-0.5 text-gray-400 flex-shrink-0" />
                                                <span className="truncate max-w-[200px] block" title={eatery.address}>{eatery.address || "Chưa cập nhật"}</span>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4">
                                            {/* HIỂN THỊ TRẠNG THÁI & LÝ DO */}
                                            {eatery.status === 'approved' && <span className="px-3 py-1 inline-flex text-xs font-semibold rounded-full bg-green-100 text-green-800"><CheckCircle size={14} className="mr-1" /> Đã duyệt</span>}
                                            {eatery.status === 'pending' && <span className="px-3 py-1 inline-flex text-xs font-semibold rounded-full bg-yellow-100 text-yellow-800"><Clock size={14} className="mr-1" /> Chờ duyệt</span>}
                                            {eatery.status === 'locked' && (
                                                <div>
                                                    <span className="px-3 py-1 inline-flex text-xs font-semibold rounded-full bg-orange-100 text-orange-800 mb-1"><Lock size={14} className="mr-1" /> Đã khóa</span>
                                                    <div className="text-xs text-orange-600 truncate max-w-[200px]" title={eatery.actionReason}>Lý do: {eatery.actionReason}</div>
                                                </div>
                                            )}
                                            {eatery.status === 'deleted' && (
                                                <div>
                                                    <span className="px-3 py-1 inline-flex text-xs font-semibold rounded-full bg-red-100 text-red-800 mb-1"><Ban size={14} className="mr-1" /> Đã xóa</span>
                                                    <div className="text-xs text-red-600 truncate max-w-[200px]" title={eatery.actionReason}>Vi phạm: {eatery.actionReason}</div>
                                                </div>
                                            )}
                                        </td>
                                        <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium space-x-2">
                                            <button onClick={() => openViewModal(eatery)} className="text-blue-600 hover:text-blue-900 bg-blue-50 p-2 rounded transition" title="Xem chi tiết"><Eye size={16} /></button>
                                            
                                            {/* QUÁN ĐÃ XÓA THÌ CHỈ ĐƯỢC XEM, KHÔNG THỂ SỬA HAY THAO TÁC GÌ KHÁC */}
                                            {eatery.status !== 'deleted' && (
                                                <>
                                                    <button onClick={() => openEditModal(eatery)} className="text-gray-600 hover:text-gray-900 bg-gray-100 p-2 rounded transition" title="Sửa thông tin"><Edit size={16} /></button>
                                                    
                                                    {eatery.status === 'pending' && <button onClick={() => handleApproveOrUnlock(eatery.id)} className="text-white hover:bg-green-700 bg-green-600 p-2 rounded transition shadow-sm" title="Duyệt cấp phép"><CheckCircle size={16} /></button>}
                                                    {eatery.status === 'locked' && <button onClick={() => handleApproveOrUnlock(eatery.id)} className="text-green-600 hover:bg-green-100 bg-green-50 p-2 rounded transition" title="Mở khóa"><Unlock size={16} /></button>}
                                                    {eatery.status === 'approved' && <button onClick={() => openLockConfirm(eatery)} className="text-orange-600 hover:bg-orange-100 bg-orange-50 p-2 rounded transition" title="Khóa quán"><Lock size={16} /></button>}
                                                    
                                                    <button onClick={() => openDeleteConfirm(eatery)} className="text-red-600 hover:text-red-900 hover:bg-red-100 bg-red-50 p-2 rounded transition ml-2" title="Xóa quán"><Trash2 size={16} /></button>
                                                </>
                                            )}
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>

            {/* ================= MODAL NHẬP LÝ DO KHÓA QUÁN ================= */}
            {showLockModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50 p-4">
                    <div className="bg-white rounded-lg p-6 w-[450px] shadow-xl">
                        <div className="flex items-center text-orange-600 mb-4">
                            <Lock size={28} className="mr-2" />
                            <h2 className="text-xl font-bold text-gray-800">Khóa quán ăn</h2>
                        </div>
                        <p className="text-gray-600 mb-4 text-sm">Bạn đang thực hiện khóa quán <strong>{eateryToLock?.name}</strong>. Quán này sẽ bị ẩn khỏi ứng dụng người dùng.</p>
                        <form onSubmit={confirmLock}>
                            <textarea 
                                required autoFocus
                                rows="3" 
                                placeholder="Nhập lý do khóa quán (Ví dụ: Chủ quán yêu cầu, Đang sửa chữa...)"
                                value={lockReason} 
                                onChange={(e) => setLockReason(e.target.value)} 
                                className="w-full border rounded-lg p-3 focus:ring focus:ring-orange-200 mb-6 text-sm"
                            ></textarea>
                            <div className="flex justify-end space-x-3">
                                <button type="button" onClick={() => setShowLockModal(false)} className="px-4 py-2 border rounded hover:bg-gray-50 font-medium">Hủy</button>
                                <button type="submit" className="px-4 py-2 bg-orange-500 text-white rounded hover:bg-orange-600 font-medium">Xác nhận Khóa</button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {/* ================= MODAL NHẬP LÝ DO XÓA (VI PHẠM) ================= */}
            {showDeleteModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50 p-4">
                    <div className="bg-white rounded-lg p-6 w-[450px] shadow-xl">
                        <div className="flex items-center text-red-600 mb-4">
                            <AlertCircle size={28} className="mr-2" />
                            <h2 className="text-xl font-bold text-gray-800">Đình chỉ / Xóa quán ăn</h2>
                        </div>
                        <p className="text-gray-600 mb-4 text-sm">Cảnh báo: Hành động này sẽ chuyển quán <strong>{eateryToDelete?.name}</strong> vào danh sách đã xóa và <strong className="text-red-500">không thể khôi phục</strong>.</p>
                        <form onSubmit={confirmDelete}>
                            <textarea 
                                required autoFocus
                                rows="3" 
                                placeholder="Nhập lý do vi phạm (Ví dụ: Vi phạm VSATTP, Phản hồi xấu nhiều lần, Đóng cửa vĩnh viễn...)"
                                value={deleteReason} 
                                onChange={(e) => setDeleteReason(e.target.value)} 
                                className="w-full border border-red-200 rounded-lg p-3 focus:ring focus:ring-red-200 mb-6 text-sm"
                            ></textarea>
                            <div className="flex justify-end space-x-3">
                                <button type="button" onClick={() => setShowDeleteModal(false)} className="px-4 py-2 border rounded hover:bg-gray-50 font-medium">Hủy</button>
                                <button type="submit" className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700 font-medium flex items-center">
                                    <Ban size={16} className="mr-2" /> Đưa vào danh sách xóa
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {/* ================= MODAL THÊM / SỬA / XEM QUÁN ĂN (ĐÃ KHÔI PHỤC) ================= */}
            {showModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50 p-4 overflow-y-auto">
                    <div className="bg-white rounded-lg p-6 w-full max-w-3xl shadow-xl my-8">
                        <div className="flex justify-between items-center mb-6 border-b pb-3">
                            <h2 className="text-2xl font-bold text-gray-800">
                                {modalMode === 'add' ? 'Thêm Quán ăn mới' : modalMode === 'edit' ? 'Chỉnh sửa Quán ăn' : 'Chi tiết Quán ăn'}
                            </h2>
                            <button type="button" onClick={() => setShowModal(false)} className="text-gray-500 hover:text-red-500 bg-gray-100 hover:bg-red-50 rounded-full p-1 transition">
                                <X size={24} />
                            </button>
                        </div>

                        {/*FORM SỬA THÔNG TIN QUÁN ĂN*/}
                        <form onSubmit={handleSubmitForm} 
                        className="overflow-y-auto p-6 space-y-4">
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                {/* Cột 1 */}
                                <div className="space-y-4">
                                    <div>
                                        <label className="block text-sm font-medium text-gray-700 mb-1">Tên quán ăn *</label>
                                        <input type="text" required disabled={modalMode === 'view'} value={formData.name} onChange={(e) => setFormData({...formData, name: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100 disabled:text-gray-600"/>
                                    </div>
                                    <div>
                                        <label className="block text-sm font-medium text-gray-700 mb-1">Địa chỉ *</label>
                                        <input type="text" required disabled={modalMode === 'view'} value={formData.address} onChange={(e) => setFormData({...formData, address: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100 disabled:text-gray-600"/>
                                    </div>
                                    <div className="grid grid-cols-2 gap-4">
                                        <div>
                                            <label className="block text-sm font-medium text-gray-700 mb-1">Giờ mở cửa</label>
                                            <input type="time" disabled={modalMode === 'view'} value={formData.openTime} onChange={(e) => setFormData({...formData, openTime: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100"/>
                                        </div>
                                        <div>
                                            <label className="block text-sm font-medium text-gray-700 mb-1">Giờ đóng cửa</label>
                                            <input type="time" disabled={modalMode === 'view'} value={formData.closeTime} onChange={(e) => setFormData({...formData, closeTime: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100"/>
                                        </div>
                                    </div>

                                    {/*Drop box sửa danh mục(category)*/}
                                    <div>
                                        <label className="block text-sm font-medium text-gray-700 mb-1">
                                            Danh mục
                                        </label>

                                        <select
                                            disabled={modalMode === "view"}
                                            value={formData.categoryId}
                                            onChange={(e) =>
                                                setFormData({
                                                    ...formData,
                                                    categoryId: parseInt(e.target.value)
                                                })
                                            }
                                            className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100"
                                        >
                                            {categories.map(category => (
                                                <option
                                                    key={category.id}
                                                    value={category.id}
                                                >
                                                    {category.name}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                </div>

                                {/* Cột 2 */}
                                <div className="space-y-4">
                                    <div className="grid grid-cols-2 gap-4">
                                        <div>
                                            <label className="block text-sm font-medium text-gray-700 mb-1">Vĩ độ (Latitude)</label>
                                            <input type="number" step="any" disabled={modalMode === 'view'} value={formData.latitude} onChange={(e) => setFormData({...formData, latitude: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100"/>
                                        </div>
                                        <div>
                                            <label className="block text-sm font-medium text-gray-700 mb-1">Kinh độ (Longitude)</label>
                                            <input type="number" step="any" disabled={modalMode === 'view'} value={formData.longitude} onChange={(e) => setFormData({...formData, longitude: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100"/>
                                        </div>
                                    </div>
                                    <div>
                                        <label className="block text-sm font-medium text-gray-700 mb-1">Mức giá tham khảo</label>
                                        <input type="text" placeholder="VD: 50.000đ - 200.000đ" disabled={modalMode === 'view'} value={formData.priceRange} onChange={(e) => setFormData({...formData, priceRange: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100"/>
                                    </div>
                                    <div>
                                        <label className="block text-sm font-medium text-gray-700 mb-1">Ảnh đại diện (Link/Path)</label>
                                        <input type="text" disabled={modalMode === 'view'} value={formData.imagePath} onChange={(e) => setFormData({...formData, imagePath: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100 text-sm font-mono"/>
                                    </div>
                                    <div>
                                        <label className="block text-sm font-medium text-gray-700 mb-1">
                                            File Audio (Link/Path)
                                        </label>
                                        <input
                                            type="text"
                                            placeholder="uploads/audio/example.mp3"
                                            disabled={modalMode === 'view'}
                                            value={formData.audioFilePath}
                                            onChange={(e) =>
                                                setFormData({ ...formData, audioFilePath: e.target.value })
                                            }
                                            className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100 text-sm font-mono"
                                        />
                                    </div>
                                </div>
                            </div>

                            {/* Dòng full width */}
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">Mô tả quán</label>
                                <textarea disabled={modalMode === 'view'} rows="3" value={formData.description} onChange={(e) => setFormData({...formData, description: e.target.value})} className="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-2 focus:ring-blue-500 outline-none disabled:bg-gray-100"></textarea>
                            </div>

                            {/* Cụm nút bấm */}
                            <div className="flex justify-end space-x-3 pt-4 border-t mt-6">
                                <button type="button" onClick={() => setShowModal(false)} className="px-5 py-2.5 border border-gray-300 rounded-lg hover:bg-gray-50 font-medium transition text-gray-700">
                                    {modalMode === 'view' ? 'Đóng' : 'Hủy bỏ'}
                                </button>
                                {modalMode !== 'view' && (
                                    <button type="submit" className="px-5 py-2.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 font-medium transition flex items-center shadow-sm">
                                        Lưu thông tin
                                    </button>
                                )}
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </div>
    );
}