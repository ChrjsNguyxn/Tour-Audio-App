import { useState, useEffect } from 'react'
import axios from 'axios'

const API = 'https://localhost:7287/api'

function Dashboard({ owner, onLogout }) {
    const [shops, setShops] = useState([])
    const [form, setForm] = useState({ name: '', category: 'nuoc', priceRange: '', description: '', latitude: '', longitude: '', audioFilePath: '', imagePath: '' })
    const [editing, setEditing] = useState(null)
    const [msg, setMsg] = useState('')
    const [uploading, setUploading] = useState('')
    const [selectedShop, setSelectedShop] = useState(null)
    const [menuItems, setMenuItems] = useState([])
    const [menuForm, setMenuForm] = useState({ name: '', price: '', quantity: '', description: '', imagePath: '' })

    useEffect(() => { loadShops() }, [])

    async function loadShops() {
        const res = await axios.get(`${API}/Shops`)
        setShops(res.data.filter(s => s.ownerId === owner.ownerId))
    }

    const handle = e => setForm({ ...form, [e.target.name]: e.target.value })
    const handleMenu = e => setMenuForm({ ...menuForm, [e.target.name]: e.target.value })

    const uploadFile = async (e, type, target) => {
        const file = e.target.files[0]
        if (!file) return
        setUploading(type + target)
        const fd = new FormData()
        fd.append('file', file)
        try {
            const res = await axios.post(`${API}/Upload/image`, fd, {
                headers: { 'Content-Type': 'multipart/form-data' }
            })
            if (target === 'shop') setForm(f => ({ ...f, imagePath: res.data.filePath }))
            else setMenuForm(f => ({ ...f, imagePath: res.data.filePath }))
            setMsg('Upload ảnh thành công!')
        } catch { setMsg('Upload thất bại!') }
        setUploading('')
    }

    const uploadAudio = async (e) => {
        const file = e.target.files[0]
        if (!file) return
        setUploading('audio')
        const fd = new FormData()
        fd.append('file', file)
        try {
            const res = await axios.post(`${API}/Upload/audio`, fd, {
                headers: { 'Content-Type': 'multipart/form-data' }
            })
            setForm(f => ({ ...f, audioFilePath: res.data.filePath }))
            setMsg('Upload audio thành công!')
        } catch { setMsg('Upload thất bại!') }
        setUploading('')
    }

    const save = async () => {
        const data = { ...form, ownerId: owner.ownerId, latitude: parseFloat(form.latitude) || 0, longitude: parseFloat(form.longitude) || 0 }
        try {
            if (editing) {
                await axios.put(`${API}/Shops/${editing}`, { ...data, id: editing })
                setMsg('Cập nhật thành công!')
            } else {
                await axios.post(`${API}/Shops`, data)
                setMsg('Tạo quán thành công!')
            }
            setForm({ name: '', category: 'nuoc', priceRange: '', description: '', latitude: '', longitude: '', audioFilePath: '', imagePath: '' })
            setEditing(null)
            loadShops()
        } catch { setMsg('Có lỗi xảy ra!') }
    }

    const editShop = s => {
        setForm({ name: s.name, category: s.category, priceRange: s.priceRange, description: s.description, latitude: s.latitude, longitude: s.longitude, audioFilePath: s.audioFilePath || '', imagePath: s.imagePath || '' })
        setEditing(s.id)
    }

    const deleteShop = async id => {
        if (!confirm('Xóa quán này?')) return
        await axios.delete(`${API}/Shops/${id}`)
        loadShops()
    }

    const openMenu = async shop => {
        setSelectedShop(shop)
        const res = await axios.get(`${API}/MenuItems/shop/${shop.id}`)
        setMenuItems(res.data)
    }

    const addMenuItem = async () => {
        if (!menuForm.name) return
        await axios.post(`${API}/MenuItems`, { ...menuForm, shopId: selectedShop.id, quantity: parseInt(menuForm.quantity) || 0 })
        setMenuForm({ name: '', price: '', quantity: '', description: '', imagePath: '' })
        const res = await axios.get(`${API}/MenuItems/shop/${selectedShop.id}`)
        setMenuItems(res.data)
    }

    const deleteMenuItem = async id => {
        await axios.delete(`${API}/MenuItems/${id}`)
        const res = await axios.get(`${API}/MenuItems/shop/${selectedShop.id}`)
        setMenuItems(res.data)
    }

    if (selectedShop) return (
        <div style={{ padding: '24px', maxWidth: '800px', margin: '0 auto' }}>
            <button onClick={() => setSelectedShop(null)} style={{ marginBottom: '16px', padding: '8px 16px', border: '1px solid #ddd', borderRadius: '8px', cursor: 'pointer' }}>← Quay lại</button>
            <h2>🍽️ Menu: {selectedShop.name}</h2>

            {/* Form thêm món */}
            <div style={{ background: '#f9f9f9', padding: '16px', borderRadius: '12px', margin: '16px 0' }}>
                <h3 style={{ marginBottom: '12px' }}>➕ Thêm món</h3>
                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '10px' }}>
                    <input name="name" placeholder="Tên món *" value={menuForm.name} onChange={handleMenu} style={iStyle} />
                    <input name="price" placeholder="Giá (vd: 25k)" value={menuForm.price} onChange={handleMenu} style={iStyle} />
                    <input name="quantity" placeholder="Số lượng" type="number" value={menuForm.quantity} onChange={handleMenu} style={iStyle} />
                    <input name="description" placeholder="Mô tả món" value={menuForm.description} onChange={handleMenu} style={iStyle} />
                </div>
                <div style={{ marginTop: '10px' }}>
                    <label style={{ fontSize: '13px', color: '#555' }}>🖼️ Ảnh món</label><br />
                    <input type="file" accept="image/*" onChange={e => uploadFile(e, 'image', 'menu')} style={{ marginTop: '4px' }} />
                    {menuForm.imagePath && <span style={{ fontSize: '12px', color: 'green' }}> ✓ Đã có ảnh</span>}
                </div>
                <button onClick={addMenuItem} style={{ marginTop: '12px', padding: '10px 20px', background: '#185FA5', color: '#fff', border: 'none', borderRadius: '8px', cursor: 'pointer' }}>
                    Thêm món
                </button>
            </div>

            {/* Danh sách món */}
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: '12px' }}>
                {menuItems.map(item => (
                    <div key={item.id} style={{ background: '#fff', border: '1px solid #eee', borderRadius: '10px', overflow: 'hidden' }}>
                        {item.imagePath
                            ? <img src={`https://localhost:7287${item.imagePath}`} style={{ width: '100%', height: '140px', objectFit: 'cover' }} />
                            : <div style={{ width: '100%', height: '140px', background: '#f5f5f5', display: 'flex', alignItems: 'center', justifyContent: 'center', color: '#ccc' }}>Không có ảnh</div>
                        }
                        <div style={{ padding: '10px' }}>
                            <div style={{ fontWeight: 600 }}>{item.name}</div>
                            <div style={{ fontSize: '13px', color: '#888' }}>💰 {item.price} · SL: {item.quantity}</div>
                            {item.description && <div style={{ fontSize: '12px', color: '#aaa', marginTop: '4px' }}>{item.description}</div>}
                            <button onClick={() => deleteMenuItem(item.id)} style={{ marginTop: '8px', padding: '4px 10px', border: '1px solid #e44', color: '#e44', borderRadius: '6px', cursor: 'pointer', background: 'none', fontSize: '12px' }}>Xóa</button>
                        </div>
                    </div>
                ))}
                {menuItems.length === 0 && <p style={{ color: '#aaa' }}>Chưa có món nào.</p>}
            </div>
        </div>
    )

    return (
        <div style={{ padding: '24px', maxWidth: '900px', margin: '0 auto' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '24px' }}>
                <h2>👋 Xin chào, {owner.name}!</h2>
                <button onClick={onLogout} style={{ padding: '8px 16px', border: '1px solid #ddd', borderRadius: '8px', cursor: 'pointer' }}>Đăng xuất</button>
            </div>

            <div style={{ background: '#f9f9f9', padding: '20px', borderRadius: '12px', marginBottom: '24px' }}>
                <h3 style={{ marginBottom: '16px' }}>{editing ? '✏️ Sửa quán' : '➕ Thêm quán mới'}</h3>
                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '12px' }}>
                    <input name="name" placeholder="Tên quán" value={form.name} onChange={handle} style={iStyle} />
                    <select name="category" value={form.category} onChange={handle} style={iStyle}>
                        <option value="nuoc">Món nước</option>
                        <option value="anvat">Ăn vặt</option>
                        <option value="trangmieng">Tráng miệng</option>
                    </select>
                    <input name="priceRange" placeholder="Giá (vd: 20-30k)" value={form.priceRange} onChange={handle} style={iStyle} />
                    <input name="description" placeholder="Mô tả" value={form.description} onChange={handle} style={iStyle} />
                    <input name="latitude" placeholder="Vĩ độ (vd: 10.7716)" value={form.latitude} onChange={handle} style={iStyle} />
                    <input name="longitude" placeholder="Kinh độ (vd: 106.685)" value={form.longitude} onChange={handle} style={iStyle} />
                </div>
                <div style={{ marginTop: '12px' }}>
                    <label style={{ fontSize: '13px', color: '#555', display: 'block', marginBottom: '4px' }}>🖼️ Ảnh bìa quán</label>
                    <input type="file" accept="image/*" onChange={e => uploadFile(e, 'image', 'shop')} />
                    {uploading === 'imageshop' && <span style={{ fontSize: '12px', color: '#888' }}> Đang upload...</span>}
                    {form.imagePath && <span style={{ fontSize: '12px', color: 'green' }}> ✓ Đã có ảnh</span>}
                </div>
                <div style={{ marginTop: '12px' }}>
                    <label style={{ fontSize: '13px', color: '#555', display: 'block', marginBottom: '4px' }}>🎧 Audio thuyết minh</label>
                    <input type="file" accept="audio/*" onChange={uploadAudio} />
                    {uploading === 'audio' && <span style={{ fontSize: '12px', color: '#888' }}> Đang upload...</span>}
                    {form.audioFilePath && <span style={{ fontSize: '12px', color: 'green' }}> ✓ Đã có audio</span>}
                </div>
                {msg && <p style={{ color: msg.includes('thất bại') || msg.includes('lỗi') ? 'red' : 'green', margin: '8px 0', fontSize: '13px' }}>{msg}</p>}
                <div style={{ display: 'flex', gap: '8px', marginTop: '16px' }}>
                    <button onClick={save} style={{ padding: '10px 20px', background: '#185FA5', color: '#fff', border: 'none', borderRadius: '8px', cursor: 'pointer' }}>
                        {editing ? 'Cập nhật' : 'Tạo quán'}
                    </button>
                    {editing && <button onClick={() => { setEditing(null); setForm({ name: '', category: 'nuoc', priceRange: '', description: '', latitude: '', longitude: '', audioFilePath: '', imagePath: '' }) }}
                        style={{ padding: '10px 20px', border: '1px solid #ddd', borderRadius: '8px', cursor: 'pointer' }}>Hủy</button>}
                </div>
            </div>

            <h3 style={{ marginBottom: '12px' }}>🏪 Quán của bạn ({shops.length})</h3>
            {shops.length === 0 ? <p style={{ color: '#aaa' }}>Chưa có quán nào.</p> :
                shops.map(s => (
                    <div key={s.id} style={{ background: '#fff', border: '1px solid #eee', borderRadius: '10px', padding: '16px', marginBottom: '10px', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <div style={{ display: 'flex', gap: '12px', alignItems: 'center' }}>
                            {s.imagePath
                                ? <img src={`https://localhost:7287${s.imagePath}`} style={{ width: '60px', height: '60px', objectFit: 'cover', borderRadius: '8px' }} />
                                : <div style={{ width: '60px', height: '60px', background: '#f5f5f5', borderRadius: '8px' }} />
                            }
                            <div>
                                <div style={{ fontWeight: 600 }}>{s.name}</div>
                                <div style={{ fontSize: '13px', color: '#888' }}>{s.category} · {s.priceRange}</div>
                                {s.audioFilePath && <div style={{ fontSize: '11px', color: '#185FA5' }}>🎧 Có audio</div>}
                            </div>
                        </div>
                        <div style={{ display: 'flex', gap: '8px' }}>
                            <button onClick={() => openMenu(s)} style={{ padding: '6px 12px', border: '1px solid #28a745', color: '#28a745', borderRadius: '6px', cursor: 'pointer', background: 'none' }}>🍽️ Menu</button>
                            <button onClick={() => editShop(s)} style={{ padding: '6px 12px', border: '1px solid #185FA5', color: '#185FA5', borderRadius: '6px', cursor: 'pointer', background: 'none' }}>Sửa</button>
                            <button onClick={() => deleteShop(s.id)} style={{ padding: '6px 12px', border: '1px solid #e44', color: '#e44', borderRadius: '6px', cursor: 'pointer', background: 'none' }}>Xóa</button>
                        </div>
                    </div>
                ))
            }
        </div>
    )
}

const iStyle = { padding: '10px', border: '1px solid #ddd', borderRadius: '8px', fontSize: '14px' }

export default Dashboard