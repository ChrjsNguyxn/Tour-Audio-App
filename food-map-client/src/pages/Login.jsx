// JavaScript source code
import { useState } from 'react'
import axios from 'axios'

const API = 'https://localhost:7287/api'

function Login({ onLogin }) {
    const [isRegister, setIsRegister] = useState(false)
    const [form, setForm] = useState({ fullName: '', email: '', password: '', phoneNumber: '' })
    const [error, setError] = useState('')
    const [loading, setLoading] = useState(false)

    const handle = e => setForm({ ...form, [e.target.name]: e.target.value })

    const submit = async () => {
        setLoading(true); setError('')
        try {
            if (isRegister) {
                await axios.post(`${API}/Owners/register`, {
                    fullName: form.fullName, email: form.email,
                    passwordHash: form.password, phoneNumber: form.phoneNumber
                })
                setIsRegister(false)
                setError('Đăng ký thành công! Hãy đăng nhập.')
            } else {
                const res = await axios.post(`${API}/Owners/login`, {
                    email: form.email, password: form.password
                })
                onLogin(res.data)
            }
        } catch (e) {
            setError(e.response?.data || 'Có lỗi xảy ra!')
        }
        setLoading(false)
    }

    return (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh', background: '#f5f5f5' }}>
            <div style={{ background: '#fff', padding: '32px', borderRadius: '12px', width: '360px', boxShadow: '0 2px 12px rgba(0,0,0,0.1)' }}>
                <h2 style={{ textAlign: 'center', marginBottom: '24px' }}>
                    {isRegister ? '📝 Đăng ký Owner' : '🔐 Đăng nhập Owner'}
                </h2>

                {isRegister && (
                    <>
                        <input name="fullName" placeholder="Họ tên" value={form.fullName} onChange={handle}
                            style={inputStyle} />
                        <input name="phoneNumber" placeholder="Số điện thoại" value={form.phoneNumber} onChange={handle}
                            style={inputStyle} />
                    </>
                )}

                <input name="email" placeholder="Email" value={form.email} onChange={handle} style={inputStyle} />
                <input name="password" type="password" placeholder="Mật khẩu" value={form.password} onChange={handle}
                    style={inputStyle} />

                {error && <p style={{ color: error.includes('thành công') ? 'green' : 'red', fontSize: '13px' }}>{error}</p>}

                <button onClick={submit} disabled={loading}
                    style={{ width: '100%', padding: '10px', background: '#185FA5', color: '#fff', border: 'none', borderRadius: '8px', cursor: 'pointer', fontSize: '15px' }}>
                    {loading ? 'Đang xử lý...' : isRegister ? 'Đăng ký' : 'Đăng nhập'}
                </button>

                <p style={{ textAlign: 'center', marginTop: '16px', fontSize: '13px', color: '#666', cursor: 'pointer' }}
                    onClick={() => { setIsRegister(!isRegister); setError('') }}>
                    {isRegister ? 'Đã có tài khoản? Đăng nhập' : 'Chưa có tài khoản? Đăng ký'}
                </p>
            </div>
        </div>
    )
}

const inputStyle = {
    width: '100%', padding: '10px', marginBottom: '12px',
    border: '1px solid #ddd', borderRadius: '8px', fontSize: '14px',
    boxSizing: 'border-box'
}

export default Login