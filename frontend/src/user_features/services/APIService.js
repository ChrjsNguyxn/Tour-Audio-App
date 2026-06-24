// hàm fetch dữ liệu POI từ server

const API_URL = "http://localhost:5092/api/v1/tourist/eateries";

export async function getAllVendors() {
    const response = await fetch(API_URL);

    if (!response.ok) {
        throw new Error("Failed to fetch eateries");
    }

    return await response.json();
}

// API của menu item
const MENU_API_URL = "http://localhost:5092/api/v1/menuitem";

// Lấy ra menu của một POI(quán) cho tourist theo id của POI
export async function getAvailableMenuByEateryId(eateryId) {
    const response = await fetch(
        `${MENU_API_URL}/eateries/${eateryId}/menu`
    );

    if (!response.ok) {
        throw new Error("Failed to fetch available menu");
    }

    return await response.json();
}