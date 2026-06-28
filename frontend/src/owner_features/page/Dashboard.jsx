import { useState, useEffect, useRef } from 'react'
import { QRCodeCanvas } from 'qrcode.react'
import * as ownerService from '../services/ownerService'

// ============ DESIGN TOKENS ============
const colors = {
    navy: '#0F2240',
    navyLight: '#1A3A63',
    orange: '#E8732A',
    orangeLight: '#FFF1E8',
    bg: '#F5F6F8',
    card: '#FFFFFF',
    border: '#E5E8EC',
    text: '#1A1F2B',
    textMuted: '#6B7280',
    textFaint: '#9CA3AF',
    green: '#1B9E5A',
    red: '#D8453A',
}

// TODO: đổi thành domain thật của web Tourist khi deploy lên production
const TOURIST_BASE_URL = 'http://localhost:5174'

export default function OwnerDashboard({ owner, onLogout }) {
    const [view, setView] = useState('eateries') // eateries | menu | stats
    const [eateries, setEateries] = useState([])
    const [categories, setCategories] = useState([]) // lấy thật từ API, không đoán ID nữa
    const [form, setForm] = useState(emptyForm())
    const [editing, setEditing] = useState(null)
    const [msg, setMsg] = useState({ text: '', type: '' })
    const [uploading, setUploading] = useState('')
    const [activeEatery, setActiveEatery] = useState(null)
    const [menuItems, setMenuItems] = useState([])
    const [menuForm, setMenuForm] = useState({ name: '', price: '', description: '', imagePath: '' })
    const [stats, setStats] = useState(null)
    const [showForm, setShowForm] = useState(false)
    const [qrEatery, setQrEatery] = useState(null)

    function emptyForm() {
        return {
            name: '',
            address: '',
            categoryId: '',
            priceRange: '',
            description: '',
            latitude: '',
            longitude: '',
            audioFilePath: '',
            imagePath: '',
            openTime: '06:00',
            closeTime: '22:00',
            isOpenNow: true,
            narrationText: '',
            narrationLanguage: 'vi-VN'
        }
    }

    useEffect(() => {
        loadEateries()
        loadCategories()
    }, [])

    async function loadCategories() {
        try {
            const res = await ownerService.getCategories()
            setCategories(res.data)
            // Nếu form chưa có danh mục nào được chọn, mặc định chọn cái đầu tiên
            setForm(f => (f.categoryId === '' && res.data.length > 0) ? { ...f, categoryId: res.data[0].id } : f)
        } catch {
            flash('Không tải được danh mục. Hãy kiểm tra bảng categories đã có dữ liệu chưa.', 'error')
        }
    }

    async function loadEateries() {
        try {
            const list = await ownerService.getEateriesByOwner()
            setEateries(list)
        } catch {
            flash('Không tải được danh sách quán', 'error')
        }
    }

    const flash = (text, type = 'success') => {
        setMsg({ text, type })
        setTimeout(() => setMsg({ text: '', type: '' }), 3500)
    }

    const handle = e => {
        const { name, value, type, checked } = e.target
        if (name === 'categoryId') {
            setForm({ ...form, categoryId: parseInt(value, 10) || 0 })
            return
        }
        setForm({ ...form, [name]: type === 'checkbox' ? checked : value })
    }
    const handleMenu = e => setMenuForm({ ...menuForm, [e.target.name]: e.target.value })

    const speakPreview = (text, lang) => {
        if (!text) { flash('Hãy nhập nội dung trước khi nghe thử', 'error'); return }
        const utter = new SpeechSynthesisUtterance(text)
        utter.lang = lang
        speechSynthesis.cancel()
        speechSynthesis.speak(utter)
    }

    const getLocation = () => {
        if (!navigator.geolocation) {
            flash('Trình duyệt không hỗ trợ định vị', 'error')
            return
        }
        flash('Đang lấy vị trí…', 'success')
        navigator.geolocation.getCurrentPosition(
            pos => {
                setForm(f => ({
                    ...f,
                    latitude: pos.coords.latitude.toFixed(6),
                    longitude: pos.coords.longitude.toFixed(6)
                }))
                flash('Đã lấy vị trí hiện tại')
            },
            () => flash('Không lấy được vị trí — kiểm tra đã cho phép quyền định vị chưa', 'error')
        )
    }

    const uploadFile = async (e, target) => {
        const file = e.target.files[0]
        if (!file) return
        setUploading(target)
        const fd = new FormData()
        fd.append('file', file)
        try {
            const res = await ownerService.uploadImage(fd)
            if (target === 'eatery') setForm(f => ({ ...f, imagePath: res.data.url }))
            else setMenuForm(f => ({ ...f, imagePath: res.data.url }))
            flash('Tải ảnh lên thành công')
        } catch { flash('Tải ảnh thất bại', 'error') }
        setUploading('')
    }

    const save = async () => {
        if (!form.name.trim()) { flash('Vui lòng nhập tên quán', 'error'); return }
        if (!form.address.trim()) { flash('Vui lòng nhập địa chỉ quán', 'error'); return }
        if (!form.categoryId) { flash('Vui lòng chọn danh mục', 'error'); return }

        const data = {
            ...form,
            ownerId: owner.ownerId,
            categoryId: parseInt(form.categoryId, 10) || 0,
            latitude: parseFloat(form.latitude) || 0,
            longitude: parseFloat(form.longitude) || 0
        }

        try {
            if (editing) {
                await ownerService.updateEatery(editing, { ...data, id: editing })
                flash('Đã cập nhật quán')
            } else {
                await ownerService.createEatery(data)
                flash('Đã tạo quán mới, đang chờ Admin duyệt')
            }
            setForm(emptyForm())
            setEditing(null)
            setShowForm(false)
            loadEateries()
        } catch (err) {
            const apiMsg = err?.response?.data?.message
            flash(typeof apiMsg === 'string' ? apiMsg : 'Có lỗi xảy ra, thử lại', 'error')
        }
    }

    const editEatery = v => {
        console.log("EDIT CLICKED");
    console.log("Eatery:", JSON.stringify(v, null, 2));

        setForm({
            name: v.name || '',
            address: v.address || '',
            categoryId: v.categoryId || '',
            priceRange: v.priceRange || '',
            description: v.description || '',
            latitude: v.latitude ?? '',
            longitude: v.longitude ?? '',
            audioFilePath: v.audioFilePath || '',
            imagePath: v.imagePath || '',
            openTime: v.openTime || '06:00',
            closeTime: v.closeTime || '22:00',
            isOpenNow: v.isOpenNow ?? true,
            narrationText: v.narrationText || '',
            narrationLanguage: v.narrationLanguage || 'vi-VN'
        })
        setEditing(v.id)
        setShowForm(true)
    }

    const deleteEateryHandler = async id => {
        if (!confirm('Xóa quán này? Hành động không thể hoàn tác.')) return
        try {
            await ownerService.deleteEatery(id)
            flash('Đã xóa quán')
            loadEateries()
        } catch {
            flash('Xóa thất bại, thử lại', 'error')
        }
    }

    const openMenu = async eatery => {
        setActiveEatery(eatery)
        setView('menu')
        try {
            const res = await ownerService.getMenuByEatery(eatery.id)
            setMenuItems(res.data)
        } catch {
            flash('Không tải được thực đơn', 'error')
        }
    }

    const addMenuItem = async () => {
        if (!menuForm.name.trim()) { flash('Vui lòng nhập tên món', 'error'); return }
        try {
            await ownerService.createMenuItem({
                name: menuForm.name,
                price: parseInt(menuForm.price, 10) || 0,
                description: menuForm.description,
                imagePath: menuForm.imagePath,
                isAvailable: true,
                eateryId: activeEatery.id
            })
            setMenuForm({ name: '', price: '', description: '', imagePath: '' })
            flash('Đã thêm món')
            const res = await ownerService.getMenuByEatery(activeEatery.id)
            setMenuItems(res.data)
        } catch {
            flash('Thêm món thất bại, thử lại', 'error')
        }
    }

    const deleteMenuItemHandler = async id => {
        try {
            await ownerService.deleteMenuItem(id)
            const res = await ownerService.getMenuByEatery(activeEatery.id)
            setMenuItems(res.data)
        } catch {
            flash('Xóa món thất bại', 'error')
        }
    }

    const openStats = async eatery => {
        setActiveEatery(eatery)
        setView('stats')
        setStats(null)
        try {
            const res = await ownerService.getEateryStats(eatery.id)
            setStats(res.data)
        } catch {
            flash('Không tải được thống kê', 'error')
        }
    }

    const initials = (owner.name || '?').trim().split(' ').slice(-2).map(w => w[0]).join('').toUpperCase()

    return (
        <div style={S.shell}>
            {/* ===== SIDEBAR ===== */}
            <aside style={S.sidebar}>
                <div style={S.brand}>
                    <div style={S.brandMark}>🍜</div>
                    <div>
                        <div style={S.brandName}>Eatery Panel</div>
                        <div style={S.brandSub}>Hồ Thị Kỷ Food Map</div>
                    </div>
                </div>

                <nav style={S.nav}>
                    <NavItem icon="🏪" label="Quán của tôi" active={view === 'eateries'} onClick={() => { setView('eateries'); setShowForm(false) }} />
                    <NavItem icon="🍽️" label="Thực đơn" active={view === 'menu'} disabled={!activeEatery} onClick={() => activeEatery && setView('menu')} />
                    <NavItem icon="📊" label="Thống kê" active={view === 'stats'} disabled={!activeEatery} onClick={() => activeEatery && setView('stats')} />
                </nav>

                <div style={S.sidebarFoot}>
                    <div style={S.userRow}>
                        <div style={S.avatar}>{initials}</div>
                        <div style={{ minWidth: 0 }}>
                            <div style={S.userName}>{owner.name}</div>
                            <div style={S.userRole}>Chủ quán</div>
                        </div>
                    </div>
                    <button onClick={onLogout} style={S.logoutBtn}>
                        <span>Đăng xuất</span>
                    </button>
                </div>
            </aside>

            {/* ===== MAIN ===== */}
            <main style={S.main}>
                <header style={S.topbar}>
                    <div>
                        <h1 style={S.pageTitle}>
                            {view === 'eateries' && 'Quán của tôi'}
                            {view === 'menu' && `Thực đơn — ${activeEatery?.name || ''}`}
                            {view === 'stats' && `Thống kê — ${activeEatery?.name || ''}`}
                        </h1>
                        <p style={S.pageSub}>
                            {view === 'eateries' && `${eateries.length} quán đang được quản lý`}
                            {view === 'menu' && 'Quản lý món ăn, giá và hình ảnh'}
                            {view === 'stats' && 'Lượt xem, lượt nghe và mức độ quan tâm'}
                        </p>
                    </div>
                    {view === 'eateries' && (
                        <button onClick={() => { setShowForm(true); setEditing(null); setForm(emptyForm()) }} style={S.primaryBtn}>
                            + Thêm quán mới
                        </button>
                    )}
                    {view !== 'eateries' && (
                        <button onClick={() => { setView('eateries'); setActiveEatery(null) }} style={S.ghostBtn}>← Tất cả quán</button>
                    )}
                </header>

                {msg.text && (
                    <div style={{ ...S.toast, ...(msg.type === 'error' ? S.toastError : S.toastOk) }}>
                        {msg.type === 'error' ? '⚠' : '✓'} {msg.text}
                    </div>
                )}

                <div style={S.content}>
                    {view === 'eateries' && !showForm && (
                        <EateriesTable eateries={eateries} categories={categories} onEdit={editEatery} onDelete={deleteEateryHandler} onMenu={openMenu} onStats={openStats} onQr={setQrEatery} />
                    )}

                    {view === 'eateries' && showForm && (
                        <EateryForm
                            form={form} editing={editing} uploading={uploading} categories={categories}
                            onChange={handle} onUpload={uploadFile} onSpeak={speakPreview} onLocate={getLocation}
                            onSave={save} onCancel={() => { setShowForm(false); setEditing(null); setForm(emptyForm()) }}
                        />
                    )}

                    {view === 'menu' && activeEatery && (
                        <MenuView
                            menuItems={menuItems} menuForm={menuForm}
                            onChange={handleMenu} onUpload={e => uploadFile(e, 'menu')}
                            onAdd={addMenuItem} onDelete={deleteMenuItemHandler}
                        />
                    )}

                    {view === 'stats' && activeEatery && <StatsView stats={stats} />}
                </div>
            </main>

            {qrEatery && <QrModal eatery={qrEatery} onClose={() => setQrEatery(null)} />}
        </div>
    )
}

// ============ SUBCOMPONENTS ============

function NavItem({ icon, label, active, disabled, onClick }) {
    return (
        <button
            onClick={onClick}
            disabled={disabled}
            style={{
                ...S.navItem,
                ...(active ? S.navItemActive : {}),
                opacity: disabled ? 0.35 : 1,
                cursor: disabled ? 'not-allowed' : 'pointer'
            }}>
            <span style={{ fontSize: '17px' }}>{icon}</span>
            <span>{label}</span>
        </button>
    )
}

function EateriesTable({ eateries, categories, onEdit, onDelete, onMenu, onStats, onQr }) {
    if (eateries.length === 0) {
        return (
            <div style={S.emptyState}>
                <div style={{ fontSize: '40px', marginBottom: '8px' }}>🏪</div>
                <h3 style={{ margin: '0 0 4px', color: colors.text }}>Chưa có quán nào</h3>
                <p style={{ color: colors.textMuted, margin: 0 }}>Bấm "Thêm quán mới" để bắt đầu quản lý cửa hàng của bạn.</p>
            </div>
        )
    }
    return (
        <div style={S.card}>
            <table style={S.table}>
                <thead>
                    <tr>
                        <th style={S.th}>Quán</th>
                        <th style={S.th}>Danh mục</th>
                        <th style={S.th}>Giá</th>
                        <th style={S.th}>Trạng thái</th>
                        <th style={{ ...S.th, textAlign: 'right' }}>Thao tác</th>
                    </tr>
                </thead>
                <tbody>
                    {eateries.map(v => (
                        <tr key={v.id} style={S.tr}>
                            <td style={S.td}>
                                <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                                    {v.imagePath
                                        ? <img src={`http://localhost:5092${v.imagePath}`} style={S.thumb} />
                                        : <div style={{ ...S.thumb, background: colors.bg, display: 'flex', alignItems: 'center', justifyContent: 'center', color: colors.textFaint, fontSize: '18px' }}>🍴</div>
                                    }
                                    <div>
                                        <div style={{ fontWeight: 600, color: colors.text }}>{v.name}</div>
                                        <div style={{ fontSize: '12px', color: colors.textFaint }}>{v.openTime}–{v.closeTime}</div>
                                    </div>
                                </div>
                            </td>
                            <td style={S.td}><span style={S.tag}>{catLabel(v.categoryId, categories)}</span></td>
                            <td style={S.td}>{v.priceRange || '—'}</td>
                            <td style={S.td}>
                                <span style={{ ...S.badge, ...(v.isApproved ? S.badgeOpen : S.badgeClosed) }}>
                                    {v.isApproved ? '● Đã duyệt' : '● Chờ duyệt'}
                                </span>
                            </td>
                            <td style={{ ...S.td, textAlign: 'right' }}>
                                <div style={{ display: 'inline-flex', gap: '6px' }}>
                                    <button onClick={() => onQr(v)} style={S.iconBtn} title="Mã QR">📱</button>
                                    <button onClick={() => onStats(v)} style={S.iconBtn} title="Thống kê">📊</button>
                                    <button onClick={() => onMenu(v)} style={S.iconBtn} title="Thực đơn">🍽️</button>
                                    <button onClick={() => onEdit(v)} style={S.iconBtn} title="Sửa">✏️</button>
                                    <button onClick={() => onDelete(v.id)} style={{ ...S.iconBtn, color: colors.red }} title="Xóa">🗑️</button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

function EateryForm({ form, editing, uploading, categories, onChange, onUpload, onSpeak, onLocate, onSave, onCancel }) {
    return (
        <div style={S.card}>
            <div style={S.cardHeader}>{editing ? 'Sửa thông tin quán' : 'Thêm quán mới'}</div>
            <div style={S.formGrid}>
                <Field label="Tên quán *"><input name="name" value={form.name} onChange={onChange} style={S.input} placeholder="VD: Bún Num Bò Chóc" /></Field>
                <Field label="Danh mục *">
                    <select name="categoryId" value={form.categoryId} onChange={onChange} style={S.input}>
                        <option value="" disabled>-- Chọn danh mục --</option>
                        {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
                    </select>
                    {categories.length === 0 && <span style={S.hint}>Chưa có danh mục nào — cần thêm dữ liệu vào bảng categories trước.</span>}
                </Field>
                <Field label="Địa chỉ *"><input name="address" value={form.address} onChange={onChange} style={S.input} placeholder="VD: 123 Hồ Thị Kỷ, Q.10, TP.HCM" /></Field>
                <Field label="Khoảng giá"><input name="priceRange" value={form.priceRange} onChange={onChange} style={S.input} placeholder="VD: 20-30k" /></Field>
            </div>

            <Field label="Mô tả ngắn"><input name="description" value={form.description} onChange={onChange} style={S.input} placeholder="Một câu giới thiệu quán" /></Field>

            <div style={S.formGrid}>
                <Field label="Vĩ độ">
                    <input name="latitude" value={form.latitude} onChange={onChange} style={S.input} placeholder="10.7716" />
                </Field>
                <Field label="Kinh độ">
                    <input name="longitude" value={form.longitude} onChange={onChange} style={S.input} placeholder="106.685" />
                </Field>
            </div>
            <button type="button" onClick={onLocate} style={{ ...S.ghostBtn, marginBottom: '14px' }}>
                📍 Lấy vị trí hiện tại
            </button>

            <div style={S.formGrid}>
                <Field label="Giờ mở cửa"><input type="time" name="openTime" value={form.openTime} onChange={onChange} style={S.input} /></Field>
                <Field label="Giờ đóng cửa"><input type="time" name="closeTime" value={form.closeTime} onChange={onChange} style={S.input} /></Field>
            </div>

            <label style={S.checkRow}>
                <input type="checkbox" name="isOpenNow" checked={form.isOpenNow} onChange={onChange} />
                Quán đang mở cửa
            </label>

            <div style={S.divider} />

            <div style={S.formGrid}>
                <Field label="Ảnh bìa quán">
                    <input type="file" accept="image/*" onChange={e => onUpload(e, 'eatery')} style={S.fileInput} />
                    {uploading === 'eatery' && <span style={S.hint}>Đang tải lên…</span>}
                    {form.imagePath && <span style={S.hintOk}>✓ Đã có ảnh</span>}
                </Field>
            </div>

            <Field label="Kịch bản thuyết minh (Text-to-Speech)">
                <textarea name="narrationText" value={form.narrationText} onChange={onChange} rows={3}
                    placeholder="VD: Quán bún Num Bò Chóc có truyền thống hơn 20 năm giữa lòng khu phố Hồ Thị Kỷ..."
                    style={{ ...S.input, resize: 'vertical', fontFamily: 'inherit' }} />
                <div style={{ display: 'flex', gap: '8px', marginTop: '8px' }}>
                    <select name="narrationLanguage" value={form.narrationLanguage} onChange={onChange} style={{ ...S.input, width: 'auto' }}>
                        <option value="vi-VN">🇻🇳 Tiếng Việt</option>
                        <option value="en-US">🇺🇸 English</option>
                        <option value="zh-CN">🇨🇳 中文</option>
                    </select>
                    <button type="button" onClick={() => onSpeak(form.narrationText, form.narrationLanguage)} style={S.ghostBtn}>
                        ▶ Nghe thử
                    </button>
                </div>
            </Field>

            <div style={S.formActions}>
                <button onClick={onSave} style={S.primaryBtn}>{editing ? 'Lưu thay đổi' : 'Tạo quán'}</button>
                <button onClick={onCancel} style={S.ghostBtn}>Hủy</button>
            </div>
        </div>
    )
}

function MenuView({ menuItems, menuForm, onChange, onUpload, onAdd, onDelete }) {
    return (
        <>
            <div style={S.card}>
                <div style={S.cardHeader}>Thêm món mới</div>
                <div style={S.formGrid}>
                    <Field label="Tên món *"><input name="name" value={menuForm.name} onChange={onChange} style={S.input} placeholder="VD: Bún Num Bò Chóc đặc biệt" /></Field>
                    <Field label="Giá (số, vd: 35000)"><input name="price" type="number" value={menuForm.price} onChange={onChange} style={S.input} placeholder="VD: 35000" /></Field>
                    <Field label="Mô tả"><input name="description" value={menuForm.description} onChange={onChange} style={S.input} /></Field>
                </div>
                <Field label="Ảnh món ăn">
                    <input type="file" accept="image/*" onChange={onUpload} style={S.fileInput} />
                    {menuForm.imagePath && <span style={S.hintOk}>✓ Đã có ảnh</span>}
                </Field>
                <div style={S.formActions}><button onClick={onAdd} style={S.primaryBtn}>+ Thêm món</button></div>
            </div>

            {menuItems.length === 0 ? (
                <div style={S.emptyState}>
                    <div style={{ fontSize: '36px', marginBottom: '8px' }}>🍽️</div>
                    <p style={{ color: colors.textMuted, margin: 0 }}>Chưa có món nào trong thực đơn.</p>
                </div>
            ) : (
                <div style={S.menuGrid}>
                    {menuItems.map(item => (
                        <div key={item.id} style={S.menuCard}>
                            {item.imagePath
                                ? <img src={`http://localhost:5092${item.imagePath}`} style={S.menuImg} />
                                : <div style={{ ...S.menuImg, background: colors.bg, display: 'flex', alignItems: 'center', justifyContent: 'center', color: colors.textFaint }}>Không có ảnh</div>
                            }
                            <div style={{ padding: '14px' }}>
                                <div style={{ fontWeight: 600, color: colors.text }}>{item.name}</div>
                                <div style={{ fontSize: '13px', color: colors.textMuted, margin: '4px 0' }}>{item.price?.toLocaleString('vi-VN')}đ</div>
                                {item.description && <div style={{ fontSize: '12px', color: colors.textFaint }}>{item.description}</div>}
                                <button onClick={() => onDelete(item.id)} style={S.deleteLink}>Xóa món</button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </>
    )
}

function StatsView({ stats }) {
    if (!stats) return <div style={S.emptyState}><p style={{ color: colors.textMuted }}>Đang tải dữ liệu…</p></div>
    const maxCount = stats.last7Days?.length ? Math.max(...stats.last7Days.map(d => d.count), 1) : 1
    return (
        <>
            <div style={S.statsRow}>
                <StatCard label="Lượt xem" value={stats.totalViews} icon="👁️" color={colors.navy} />
                <StatCard label="Lượt nghe thuyết minh" value={stats.totalListens} icon="🎧" color={colors.green} />
                <StatCard label="Lượt yêu thích" value={stats.totalFavorites} icon="❤️" color={colors.orange} />
            </div>
            <div style={S.card}>
                <div style={S.cardHeader}>Lượt xem 7 ngày gần nhất</div>
                {(!stats.last7Days || stats.last7Days.length === 0) ? (
                    <p style={{ color: colors.textMuted }}>Chưa có dữ liệu trong 7 ngày qua.</p>
                ) : (
                    <div style={S.chartWrap}>
                        {stats.last7Days.map((d, i) => (
                            <div key={i} style={S.chartCol}>
                                <div style={S.chartValue}>{d.count}</div>
                                <div style={{ ...S.chartBar, height: `${(d.count / maxCount) * 120}px` }} />
                                <div style={S.chartLabel}>{d.date}</div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </>
    )
}

function StatCard({ label, value, icon, color }) {
    return (
        <div style={S.statCard}>
            <div style={{ ...S.statIcon, background: `${color}14`, color }}>{icon}</div>
            <div>
                <div style={S.statValue}>{value}</div>
                <div style={S.statLabel}>{label}</div>
            </div>
        </div>
    )
}

function Field({ label, children }) {
    return (
        <div style={S.field}>
            <label style={S.fieldLabel}>{label}</label>
            {children}
        </div>
    )
}

function QrModal({ eatery, onClose }) {
    const canvasWrapRef = useRef(null)
    const qrUrl = `${TOURIST_BASE_URL}/eatery/${eatery.id}`

    const downloadQr = () => {
        const canvas = canvasWrapRef.current?.querySelector('canvas')
        if (!canvas) return
        const link = document.createElement('a')
        link.href = canvas.toDataURL('image/png')
        link.download = `QR-${eatery.name.replace(/\s+/g, '_')}.png`
        link.click()
    }

    return (
        <div style={S.modalOverlay} onClick={onClose}>
            <div style={S.modalBox} onClick={e => e.stopPropagation()}>
                <div style={S.cardHeader}>Mã QR — {eatery.name}</div>
                <div ref={canvasWrapRef} style={{ display: 'flex', justifyContent: 'center', padding: '12px 0 16px' }}>
                    <QRCodeCanvas value={qrUrl} size={200} includeMargin />
                </div>
                <p style={{ fontSize: '12px', color: colors.textMuted, textAlign: 'center', wordBreak: 'break-all', margin: '0 0 18px' }}>{qrUrl}</p>
                <div style={S.formActions}>
                    <button onClick={downloadQr} style={S.primaryBtn}>⬇ Tải ảnh QR</button>
                    <button onClick={onClose} style={S.ghostBtn}>Đóng</button>
                </div>
            </div>
        </div>
    )
}
function catLabel(categoryId, categories) {
    const found = categories.find(c => c.id === categoryId)
    return found ? found.name : `Danh mục #${categoryId}`
}

// ============ STYLES ============
const S = {
    shell: { display: 'flex', minHeight: '100vh', background: colors.bg, fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif', color: colors.text },

    sidebar: { width: '248px', flexShrink: 0, background: colors.navy, display: 'flex', flexDirection: 'column', position: 'sticky', top: 0, height: '100vh' },
    brand: { display: 'flex', alignItems: 'center', gap: '10px', padding: '22px 20px', borderBottom: '1px solid rgba(255,255,255,0.08)' },
    brandMark: { width: '36px', height: '36px', borderRadius: '10px', background: colors.orange, display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '18px', flexShrink: 0 },
    brandName: { color: '#fff', fontWeight: 700, fontSize: '14px', letterSpacing: '0.2px' },
    brandSub: { color: 'rgba(255,255,255,0.5)', fontSize: '11px', marginTop: '1px' },

    nav: { padding: '16px 12px', display: 'flex', flexDirection: 'column', gap: '4px', flex: 1 },
    navItem: { display: 'flex', alignItems: 'center', gap: '10px', padding: '10px 12px', borderRadius: '8px', border: 'none', background: 'transparent', color: 'rgba(255,255,255,0.65)', fontSize: '14px', fontWeight: 500, textAlign: 'left', transition: 'background .15s' },
    navItemActive: { background: 'rgba(255,255,255,0.1)', color: '#fff' },

    sidebarFoot: { padding: '16px', borderTop: '1px solid rgba(255,255,255,0.08)' },
    userRow: { display: 'flex', alignItems: 'center', gap: '10px', marginBottom: '12px' },
    avatar: { width: '34px', height: '34px', borderRadius: '50%', background: colors.orange, color: '#fff', display: 'flex', alignItems: 'center', justifyContent: 'center', fontWeight: 700, fontSize: '13px', flexShrink: 0 },
    userName: { color: '#fff', fontSize: '13px', fontWeight: 600, whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis' },
    userRole: { color: 'rgba(255,255,255,0.45)', fontSize: '11px' },
    logoutBtn: { width: '100%', padding: '9px', borderRadius: '8px', border: '1px solid rgba(255,255,255,0.15)', background: 'transparent', color: 'rgba(255,255,255,0.8)', fontSize: '13px', cursor: 'pointer' },

    main: { flex: 1, minWidth: 0, padding: '28px 32px' },
    topbar: { display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '20px', gap: '16px', flexWrap: 'wrap' },
    pageTitle: { fontSize: '22px', fontWeight: 700, margin: 0, color: colors.text },
    pageSub: { fontSize: '13px', color: colors.textMuted, margin: '4px 0 0' },

    primaryBtn: { padding: '10px 18px', background: colors.navy, color: '#fff', border: 'none', borderRadius: '8px', fontSize: '13px', fontWeight: 600, cursor: 'pointer' },
    ghostBtn: { padding: '10px 18px', background: '#fff', color: colors.text, border: `1px solid ${colors.border}`, borderRadius: '8px', fontSize: '13px', fontWeight: 600, cursor: 'pointer' },

    toast: { padding: '10px 14px', borderRadius: '8px', fontSize: '13px', marginBottom: '16px', fontWeight: 500 },
    toastOk: { background: '#E9F8EF', color: colors.green },
    toastError: { background: '#FCEAE8', color: colors.red },

    content: { display: 'flex', flexDirection: 'column', gap: '20px' },

    card: { background: colors.card, borderRadius: '12px', border: `1px solid ${colors.border}`, padding: '22px' },
    cardHeader: { fontSize: '15px', fontWeight: 700, color: colors.text, marginBottom: '18px' },

    table: { width: '100%', borderCollapse: 'collapse' },
    th: { textAlign: 'left', fontSize: '12px', fontWeight: 600, color: colors.textFaint, textTransform: 'uppercase', letterSpacing: '0.4px', padding: '0 12px 12px', borderBottom: `1px solid ${colors.border}` },
    tr: { borderBottom: `1px solid ${colors.border}` },
    td: { padding: '14px 12px', fontSize: '13px', verticalAlign: 'middle' },
    thumb: { width: '44px', height: '44px', borderRadius: '8px', objectFit: 'cover' },
    tag: { fontSize: '12px', color: colors.navy, background: '#EEF2F8', padding: '4px 10px', borderRadius: '20px', fontWeight: 500 },
    badge: { fontSize: '12px', fontWeight: 600, padding: '4px 10px', borderRadius: '20px' },
    badgeOpen: { background: '#E9F8EF', color: colors.green },
    badgeClosed: { background: '#FCEAE8', color: colors.red },
    iconBtn: { width: '30px', height: '30px', borderRadius: '7px', border: `1px solid ${colors.border}`, background: '#fff', cursor: 'pointer', fontSize: '13px', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' },

    emptyState: { background: colors.card, border: `1px dashed ${colors.border}`, borderRadius: '12px', padding: '48px 24px', textAlign: 'center' },

    formGrid: { display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '14px', marginBottom: '6px' },
    field: { display: 'flex', flexDirection: 'column', gap: '6px', marginBottom: '14px' },
    fieldLabel: { fontSize: '12px', fontWeight: 600, color: colors.textMuted },
    input: { padding: '10px 12px', border: `1px solid ${colors.border}`, borderRadius: '8px', fontSize: '13px', fontFamily: 'inherit', color: colors.text, width: '100%', boxSizing: 'border-box' },
    fileInput: { fontSize: '13px' },
    hint: { fontSize: '12px', color: colors.textFaint, marginLeft: '8px' },
    hintOk: { fontSize: '12px', color: colors.green, marginLeft: '8px', fontWeight: 500 },
    checkRow: { display: 'flex', alignItems: 'center', gap: '8px', fontSize: '13px', color: colors.text, marginBottom: '8px' },
    divider: { height: '1px', background: colors.border, margin: '8px 0 18px' },
    formActions: { display: 'flex', gap: '10px', marginTop: '6px' },

    menuGrid: { display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(190px, 1fr))', gap: '14px' },
    menuCard: { background: colors.card, border: `1px solid ${colors.border}`, borderRadius: '12px', overflow: 'hidden' },
    menuImg: { width: '100%', height: '120px', objectFit: 'cover' },
    deleteLink: { marginTop: '10px', padding: '5px 0', border: 'none', background: 'none', color: colors.red, fontSize: '12px', fontWeight: 600, cursor: 'pointer' },

    statsRow: { display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '14px' },
    statCard: { background: colors.card, border: `1px solid ${colors.border}`, borderRadius: '12px', padding: '18px', display: 'flex', alignItems: 'center', gap: '14px' },
    statIcon: { width: '44px', height: '44px', borderRadius: '10px', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '20px', flexShrink: 0 },
    statValue: { fontSize: '22px', fontWeight: 700, color: colors.text, lineHeight: 1 },
    statLabel: { fontSize: '12px', color: colors.textMuted, marginTop: '4px' },

    chartWrap: { display: 'flex', alignItems: 'flex-end', gap: '14px', height: '170px', paddingTop: '10px' },
    chartCol: { flex: 1, display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'flex-end', height: '100%' },
    chartValue: { fontSize: '12px', fontWeight: 700, color: colors.navy, marginBottom: '6px' },
    chartBar: { width: '100%', background: colors.orange, borderRadius: '6px 6px 0 0', minHeight: '4px' },
    chartLabel: { fontSize: '11px', color: colors.textFaint, marginTop: '8px' },

    modalOverlay: { position: 'fixed', inset: 0, background: 'rgba(15,34,64,0.55)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000, padding: '20px' },
    modalBox: { background: colors.card, borderRadius: '14px', padding: '24px', width: '100%', maxWidth: '340px', boxShadow: '0 12px 40px rgba(0,0,0,0.25)' },
}

//export default OwnerDashboard