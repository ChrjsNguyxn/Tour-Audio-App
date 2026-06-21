// hàm fetch dữ liệu POI từ server

const API_URL = "http://localhost:5092/api/eateries";

export async function getAllVendors() {
    const response = await fetch(API_URL);

    if (!response.ok) {
        throw new Error("Failed to fetch eateries");
    }

    return await response.json();
}