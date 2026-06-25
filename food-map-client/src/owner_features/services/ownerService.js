import axios from 'axios'

const API = 'https://localhost:7287/api'

// ===== AUTH =====
export const registerOwner = (data) =>
    axios.post(`${API}/Owners/register`, data)

export const loginOwner = (data) =>
    axios.post(`${API}/Owners/login`, data)

// ===== EATERIES =====
export const getAllEateries = () =>
    axios.get(`${API}/eateries`)

export const getEateryById = async (id) => {
    const res = await axios.get(`${API}/eateries/${id}`)
    return res.data
}

export const getEateriesByOwner = async (ownerId) => {
    const res = await axios.get(`${API}/eateries/owner/${ownerId}`)
    return res.data
}

export const createEatery = (data) =>
    axios.post(`${API}/eateries`, data)

export const updateEatery = (id, data) =>
    axios.put(`${API}/eateries/${id}`, data)

export const deleteEatery = (id) =>
    axios.delete(`${API}/eateries/${id}`)

// ===== MENU ITEMS =====
export const getMenuByEatery = (eateryId) =>
    axios.get(`${API}/MenuItems/eatery/${eateryId}`)

export const createMenuItem = (data) =>
    axios.post(`${API}/MenuItems`, data)

export const updateMenuItem = (id, data) =>
    axios.put(`${API}/MenuItems/${id}`, data)

export const deleteMenuItem = (id) =>
    axios.delete(`${API}/MenuItems/${id}`)

// ===== UPLOAD =====
export const uploadImage = (formData) =>
    axios.post(`${API}/Upload/image`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    })

export const uploadAudio = (formData) =>
    axios.post(`${API}/Upload/audio`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    })

// ===== VIEW LOGS / STATS =====
export const getEateryStats = (eateryId) =>
    axios.get(`${API}/ViewLogs/eatery/${eateryId}`)
export const getCategories = () => axios.get(`${API}/Categories`)