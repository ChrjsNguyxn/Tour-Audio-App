import React, { useState, useEffect } from 'react';
import api from '../services/api';
import { Plus, Edit, Trash2, X, Music, Store } from 'lucide-react';

export default function MenuManagement() {
    const [menuItems, setMenuItems] = useState([]);
    const [eateries, setEateries] = useState([]); 
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [showModal, setShowModal] = useState(false);
    const [modalMode, setModalMode] = useState('add'); 
    const [formData, setFormData] = useState({ id: '', name: '', price: '', imagePath: '', audioPath: '', eateryId: '' });

    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [itemToDelete, setItemToDelete] = useState(null);
    const [currentAudio, setCurrentAudio] = useState(null);

    useEffect(() => {
        initData();
    }, []);

    const initData = async () => {
        try {
            setLoading(true);
            const [menuRes, eateryRes] = await Promise.all([
                api.get('/menuitem'), // ĐÃ SỬA THÀNH /menuitem
                api.get('/eatery/admin-all')
            ]);
            setMenuItems(menuRes.data);
            setEateries(eateryRes.data);
            setError(null);
        } catch (err) {
            setError("Không thể tải danh sách dữ liệu. Vui lòng kiểm tra Backend!");
        } finally {
            setLoading(false);
        }
    };

    const openAddModal = () => {
        setModalMode('add');
        setFormData({ id: '', name: '', price: '', imagePath: '', audioPath: '', eateryId: eateries[0]?.id || '' });
        setShowModal(true);
    };

    const openEditModal = (item) => {
        setModalMode('edit');
        setFormData({ id: item.id, name: item.name, price: item.price, imagePath: item.imagePath || '', audioPath: item.audioPath || '', eateryId: item.eateryId });
        setShowModal(true);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (modalMode === 'add') {
                await api.post('/menuitem', formData); // ĐÃ SỬA
            } else {
                await api.put(`/menuitem/${formData.id}`, formData); // ĐÃ SỬA
            }
            setShowModal(false);
            initData();
        } catch (err) {
            alert("Lỗi khi lưu thông tin món ăn!");
        }
    };

    const openDeleteConfirm = (item) => {
        setItemToDelete(item);
        setShowDeleteModal(true);
    };

    const confirmDelete = async () => {
        try {
            await api.delete(`/menuitem/${itemToDelete.id}`); // ĐÃ SỬA
            setShowDeleteModal(false);
            setMenuItems(menuItems.filter(m => m.id !== itemToDelete.id));
        } catch (err) {
            alert("Lỗi khi xóa món ăn!");
        }
    };

    const playAudioTest = (url) => {
        if (!url) return alert("Không có file audio!");
        const fullUrl = `http://localhost:5092${url}`;
        if (currentAudio) currentAudio.pause();
        const audio = new Audio(fullUrl);
        audio.play();
        setCurrentAudio(audio);
    };

    return (
        <div className="p-6">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-800">Quản lý Món ăn & Audio</h1>
                <button onClick={openAddModal} className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center shadow-sm">
                    <Plus size={18} className="mr-2" /> Thêm Món mới
                </button>
            </div>

            {error && <div className="bg-red-100 text-red-700 p-4 mb-6 rounded">{error}</div>}

            <div className="bg-white rounded-lg shadow overflow-hidden">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                        <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Món ăn / Hình ảnh</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Thuộc Quán</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Giá bán</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Audio Hướng dẫn</th>
                            <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Thao tác</th>
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                        {loading ? (
                            <tr><td colSpan="5" className="px-6 py-10 text-center">Đang tải dữ liệu...</td></tr>
                        ) : menuItems.length === 0 ? (
                            <tr><td colSpan="5" className="px-6 py-10 text-center text-gray-500">Chưa có món ăn nào.</td></tr>
                        ) : (
                            menuItems.map((item) => {
                                const eateryName = eateries.find(e => e.id === item.eateryId)?.name || "Không rõ";
                                return (
                                    <tr key={item.id} className="hover:bg-gray-50">
                                        <td className="px-6 py-4 whitespace-nowrap">
                                            <div className="flex items-center">
                                                <img src={item.imagePath ? `http://localhost:5092${item.imagePath}` : 'https://placehold.co/50'} alt={item.name} className="w-10 h-10 rounded object-cover border" onError={(e) => { e.target.src = 'https://placehold.co/50' }}/>
                                                <div className="ml-4">
                                                    <div className="text-sm font-bold text-gray-900">{item.name}</div>
                                                    <div className="text-xs text-gray-400">ID: #{item.id}</div>
                                                </div>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4 text-sm text-gray-600"><span className="flex items-center"><Store size={14} className="mr-1 text-gray-400" /> {eateryName}</span></td>
                                        <td className="px-6 py-4 text-sm font-semibold text-gray-900">{item.price?.toLocaleString('vi-VN')} đ</td>
                                        <td className="px-6 py-4">
                                            {item.audioPath ? (
                                                <button onClick={() => playAudioTest(item.audioPath)} className="text-blue-600 bg-blue-50 hover:bg-blue-100 px-3 py-1 rounded-full text-xs font-medium flex items-center"><Music size={12} className="mr-1" /> Nghe thử</button>
                                            ) : <span className="text-xs text-red-400">Chưa có âm thanh</span>}
                                        </td>
                                        <td className="px-6 py-4 text-right space-x-2">
                                            <button onClick={() => openEditModal(item)} className="text-gray-600 hover:text-gray-900 bg-gray-100 p-2 rounded inline-flex"><Edit size={16} /></button>
                                            <button onClick={() => openDeleteConfirm(item)} className="text-red-600 hover:text-red-900 bg-red-50 p-2 rounded inline-flex"><Trash2 size={16} /></button>
                                        </td>
                                    </tr>
                                )
                            })
                        )}
                    </tbody>
                </table>
            </div>

            {/* MODAL THÊM SỬA */}
            {showModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
                    <div className="bg-white rounded-lg p-6 w-[500px] shadow-xl">
                        <div className="flex justify-between items-center mb-4"><h2 className="text-xl font-bold">{modalMode === 'add' ? 'Thêm Món' : 'Chỉnh sửa'}</h2><button onClick={() => setShowModal(false)} className="text-gray-500 hover:text-red-500"><X size={24}/></button></div>
                        <form onSubmit={handleSubmit} className="space-y-4">
                            <div><label className="block text-sm font-medium text-gray-700">Thuộc Quán ăn</label><select value={formData.eateryId} onChange={(e) => setFormData({...formData, eateryId: e.target.value})} className="mt-1 w-full border rounded p-2 focus:ring">{eateries.map(e => <option key={e.id} value={e.id}>{e.name}</option>)}</select></div>
                            <div><label className="block text-sm font-medium text-gray-700">Tên món</label><input type="text" required value={formData.name} onChange={(e) => setFormData({...formData, name: e.target.value})} className="mt-1 w-full border rounded p-2 focus:ring"/></div>
                            <div><label className="block text-sm font-medium text-gray-700">Giá bán (VNĐ)</label><input type="number" required value={formData.price} onChange={(e) => setFormData({...formData, price: e.target.value})} className="mt-1 w-full border rounded p-2 focus:ring"/></div>
                            <div><label className="block text-sm font-medium text-gray-700">Hình ảnh</label><input type="text" value={formData.imagePath} onChange={(e) => setFormData({...formData, imagePath: e.target.value})} className="mt-1 w-full border rounded p-2 focus:ring text-sm font-mono"/></div>
                            <div><label className="block text-sm font-medium text-gray-700">Audio</label><input type="text" value={formData.audioPath} onChange={(e) => setFormData({...formData, audioPath: e.target.value})} className="mt-1 w-full border rounded p-2 focus:ring text-sm font-mono"/></div>
                            <div className="flex justify-end space-x-3 mt-6"><button type="button" onClick={() => setShowModal(false)} className="px-4 py-2 border rounded">Hủy</button><button type="submit" className="px-4 py-2 bg-blue-600 text-white rounded">Lưu</button></div>
                        </form>
                    </div>
                </div>
            )}

            {/* MODAL XÓA */}
            {showDeleteModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
                    <div className="bg-white rounded-lg p-6 w-[400px] shadow-xl text-center">
                        <Trash2 size={48} className="mx-auto text-red-500 mb-4" />
                        <h2 className="text-xl font-bold mb-2">Xác nhận xóa?</h2>
                        <div className="flex justify-center space-x-4 mt-6"><button onClick={() => setShowDeleteModal(false)} className="px-4 py-2 border rounded hover:bg-gray-50">Hủy</button><button onClick={confirmDelete} className="px-4 py-2 bg-red-600 text-white rounded">Xóa món</button></div>
                    </div>
                </div>
            )}
        </div>
    );
}