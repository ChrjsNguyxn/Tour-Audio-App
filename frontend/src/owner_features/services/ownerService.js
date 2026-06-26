import axios from 'axios'

// Dùng đúng cổng HTTP thật trong backend/Properties/launchSettings.json
// (http://localhost:5092). Nếu bạn chạy bằng profile "https", đổi thành
// https://localhost:7129/api/v1 cho khớp.
const API = 'http://localhost:5092/api/v1'

const client = axios.create({ baseURL: API })

// Tự gắn token (nếu có) vào mọi request — phòng khi sau này thêm [Authorize]
client.interceptors.request.use(config => {
    const token = localStorage.getItem('owner_token')
    if (token) config.headers.Authorization = `Bearer ${token}`
    return config
})

// ---------- Auth ----------
export function login(username, password) {
    return client.post('/owner-auth/login', { username, password })
}

export function register({ username, password, fullName, email }) {
    return client.post('/owner-auth/register', { username, password, fullName, email })
}

// ---------- Categories ----------
export function getCategories() {
    return client.get('/owner/categories')
}

// ---------- Eateries ----------
export function getEateriesByOwner(ownerId) {
    return client.get(`/owner/${ownerId}/eateries`).then(res => res.data)
}

export function createEatery(data) {
    return client.post('/owner/eateries', data)
}

export function updateEatery(id, data) {
    return client.put(`/owner/eateries/${id}`, data)
}

export function deleteEatery(id) {
    return client.delete(`/owner/eateries/${id}`)
}

export function getEateryStats(id) {
    return client.get(`/owner/eateries/${id}/stats`)
}

// ---------- Menu ----------
export function getMenuByEatery(eateryId) {
    return client.get(`/owner/menu/eatery/${eateryId}`)
}

export function createMenuItem(data) {
    return client.post('/owner/menu', data)
}

export function deleteMenuItem(id) {
    return client.delete(`/owner/menu/${id}`)
}

// ---------- Upload (dùng lại UploadController đã có sẵn) ----------
export function uploadImage(formData) {
    return client.post('/upload/image', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    })
}
