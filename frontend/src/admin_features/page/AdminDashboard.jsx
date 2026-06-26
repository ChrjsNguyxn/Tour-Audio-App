import { useState, useEffect } from 'react'
import axios from 'axios'

const API = 'http://localhost:5092/api/v1'

function AdminDashboard() {
    const [eateries, setEateries] = useState([])
    const [msg, setMsg] = useState({ text: '', type: '' })
    const [loading, setLoading] = useState(true)
    const [filter, setFilter] = useState('all') // all | pending | approved

    useEffect(() => {
        loadEateries()
    }, [])

    async function loadEateries() {
        setLoading(true)
        try {
            const res = await axios.get(`${API}/eatery/admin-all`)
            setEateries(res.data)
        } catch {
            flash('Không tải được danh sách quán', 'error')
        }
        setLoading(false)
    }

    const flash = (text, type = 'success') => {
        setMsg({ text, type })
        setTimeout(() => setMsg({ text: '', type: '' }), 3500)
    }

    const approve = async (id, value) => {
        try {
            await axios.put(`${API}/eatery/${id}/approve`, {
                isApproved: value
            })
            flash(value ? '✅ Đã duyệt quán!' : '🔒 Đã khóa quán!')
            loadEateries() // reload lại danh sách
        } catch {
            flash('Thao tác thất bại, thử lại', 'error')
        }
    }

    const filtered = eateries.filter(e => {
        if (filter === 'pending') return !e.isApproved
        if (filter === 'approved') return e.isApproved
        return true
    })

    const pendingCount = eateries.filter(e => !e.isApproved).length
    const approvedCount = eateries.filter(e => e.isApproved).length

    return (
        <div style={S.page}>
            {/* Header */}
            <div style={S.header}>
                <h1 style={S.title}>🛡️ Admin — Duyệt quán ăn</h1>
                <p style={S.sub}>Quản lý trạng thái hiển thị của các quán trên bản đồ</p>
            </div>

            {/* Thông báo */}
            {msg.text && (
                <div style={{
                    ...S.toast,
                    background: msg.type === 'error' ? '#FCEAE8' : '#E9F8EF',
                    color: msg.type === 'error' ? '#D8453A' : '#1B9E5A'
                }}>
                    {msg.text}
                </div>
            )}

            {/* Thống kê nhanh */}
            <div style={S.statsRow}>
                <div style={S.statCard}>
                    <div style={{ fontSize: '28px', fontWeight: 700 }}>{eateries.length}</div>
                    <div style={{ color: '#6B7280', fontSize: '13px' }}>Tổng số quán</div>
                </div>
                <div style={{ ...S.statCard, borderColor: '#FCD34D' }}>
                    <div style={{ fontSize: '28px', fontWeight: 700, color: '#D97706' }}>{pendingCount}</div>
                    <div style={{ color: '#6B7280', fontSize: '13px' }}>Chờ duyệt</div>
                </div>
                <div style={{ ...S.statCard, borderColor: '#6EE7B7' }}>
                    <div style={{ fontSize: '28px', fontWeight: 700, color: '#1B9E5A' }}>{approvedCount}</div>
                    <div style={{ color: '#6B7280', fontSize: '13px' }}>Đã duyệt</div>
                </div>
            </div>

            {/* Filter tabs */}
            <div style={S.tabs}>
                {[
                    { key: 'all', label: `Tất cả (${eateries.length})` },
                    { key: 'pending', label: `⏳ Chờ duyệt (${pendingCount})` },
                    { key: 'approved', label: `✅ Đã duyệt (${approvedCount})` },
                ].map(tab => (
                    <button
                        key={tab.key}
                        onClick={() => setFilter(tab.key)}
                        style={{
                            ...S.tab,
                            ...(filter === tab.key ? S.tabActive : {})
                        }}>
                        {tab.label}
                    </button>
                ))}
                <button onClick={loadEateries} style={S.refreshBtn}>🔄 Làm mới</button>
            </div>

            {/* Danh sách quán */}
            {loading ? (
                <div style={S.empty}>Đang tải...</div>
            ) : filtered.length === 0 ? (
                <div style={S.empty}>Không có quán nào trong danh sách này.</div>
            ) : (
                <div style={S.list}>
                    {filtered.map(e => (
                        <div key={e.id} style={{
                            ...S.card,
                            borderLeft: `4px solid ${e.isApproved ? '#1B9E5A' : '#F59E0B'}`
                        }}>
                            {/* Ảnh + Info */}
                            <div style={S.cardLeft}>
                                {e.imagePath ? (
                                    <img
                                        src={`http://localhost:5092${e.imagePath}`}
                                        style={S.img}
                                        onError={ev => ev.target.style.display = 'none'}
                                    />
                                ) : (
                                    <div style={{ ...S.img, background: '#F3F4F6', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '24px' }}>🍴</div>
                                )}
                                <div style={S.info}>
                                    <div style={S.eateryName}>{e.name}</div>
                                    <div style={S.eateryDetail}>📍 {e.address}</div>
                                    <div style={S.eateryDetail}>🕐 {e.openTime} – {e.closeTime} &nbsp;|&nbsp; 💰 {e.priceRange || 'Chưa cập nhật'}</div>
                                    <div style={S.eateryDetail}>
                                        👤 OwnerId: {e.ownerId} &nbsp;|&nbsp; 📂 CategoryId: {e.categoryId}
                                    </div>
                                </div>
                            </div>

                            {/* Status + Buttons */}
                            <div style={S.cardRight}>
                                <span style={{
                                    ...S.badge,
                                    background: e.isApproved ? '#D1FAE5' : '#FEF3C7',
                                    color: e.isApproved ? '#065F46' : '#92400E'
                                }}>
                                    {e.isApproved ? '✅ Đã duyệt' : '⏳ Chờ duyệt'}
                                </span>

                                <div style={S.btnGroup}>
                                    {!e.isApproved ? (
                                        <button
                                            onClick={() => approve(e.id, true)}
                                            style={S.approveBtn}>
                                            ✅ Duyệt quán
                                        </button>
                                    ) : (
                                        <button
                                            onClick={() => approve(e.id, false)}
                                            style={S.lockBtn}>
                                            🔒 Khóa quán
                                        </button>
                                    )}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    )
}

// ===== STYLES =====
const S = {
    page: {
        maxWidth: '900px', margin: '0 auto', padding: '32px 20px',
        fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif'
    },
    header: { marginBottom: '24px' },
    title: { fontSize: '24px', fontWeight: 700, margin: '0 0 4px', color: '#0F2240' },
    sub: { fontSize: '14px', color: '#6B7280', margin: 0 },

    toast: {
        padding: '12px 16px', borderRadius: '8px',
        marginBottom: '16px', fontWeight: 500, fontSize: '14px'
    },

    statsRow: { display: 'flex', gap: '14px', marginBottom: '24px' },
    statCard: {
        flex: 1, background: '#fff', border: '2px solid #E5E8EC',
        borderRadius: '12px', padding: '18px', textAlign: 'center'
    },

    tabs: { display: 'flex', gap: '8px', marginBottom: '16px', flexWrap: 'wrap', alignItems: 'center' },
    tab: {
        padding: '8px 16px', border: '1px solid #E5E8EC',
        borderRadius: '20px', background: '#fff', cursor: 'pointer',
        fontSize: '13px', fontWeight: 500, color: '#374151'
    },
    tabActive: { background: '#0F2240', color: '#fff', border: '1px solid #0F2240' },
    refreshBtn: {
        marginLeft: 'auto', padding: '8px 14px',
        border: '1px solid #E5E8EC', borderRadius: '8px',
        background: '#fff', cursor: 'pointer', fontSize: '13px'
    },

    empty: {
        textAlign: 'center', padding: '48px',
        color: '#9CA3AF', background: '#fff',
        borderRadius: '12px', border: '1px dashed #E5E8EC'
    },

    list: { display: 'flex', flexDirection: 'column', gap: '12px' },
    card: {
        background: '#fff', borderRadius: '12px',
        border: '1px solid #E5E8EC', padding: '16px',
        display: 'flex', justifyContent: 'space-between',
        alignItems: 'center', gap: '16px', flexWrap: 'wrap'
    },
    cardLeft: { display: 'flex', gap: '14px', alignItems: 'center', flex: 1, minWidth: 0 },
    img: { width: '70px', height: '70px', borderRadius: '10px', objectFit: 'cover', flexShrink: 0 },
    info: { minWidth: 0 },
    eateryName: { fontWeight: 700, fontSize: '15px', color: '#1A1F2B', marginBottom: '4px' },
    eateryDetail: { fontSize: '12px', color: '#6B7280', marginTop: '3px' },

    cardRight: { display: 'flex', flexDirection: 'column', alignItems: 'flex-end', gap: '10px' },
    badge: { padding: '4px 12px', borderRadius: '20px', fontSize: '12px', fontWeight: 600 },
    btnGroup: { display: 'flex', gap: '8px' },
    approveBtn: {
        padding: '8px 18px', background: '#1B9E5A', color: '#fff',
        border: 'none', borderRadius: '8px', cursor: 'pointer',
        fontSize: '13px', fontWeight: 600
    },
    lockBtn: {
        padding: '8px 18px', background: '#fff', color: '#D8453A',
        border: '1px solid #D8453A', borderRadius: '8px',
        cursor: 'pointer', fontSize: '13px', fontWeight: 600
    },
}

export default AdminDashboard