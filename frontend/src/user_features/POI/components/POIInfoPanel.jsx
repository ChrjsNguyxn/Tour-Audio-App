import { useState, useEffect } from "react";
import MenuPopup from "./MenuPopup";
import { getAvailableMenuByEateryId } from "@/user_features/services/APIService";

export default function POIInfoPanel({ poi }) {
  const [showMenu, setShowMenu] = useState(false); // dùng cho menu popup

  const [menuItems, setMenuItems] = useState([]); // dùng để lọc menu cho menu popup

  // URL dùng để lấy ảnh và file âm thanh, chú ý "http" chứ không phải "https"
  const BACKEND_URL = "http://localhost:5092/";

  // chuẩn bị menu popup
  const handleViewMenu = async () => {
    try {
      console.log("Fetching menu for:", poi.id);

      const menu = await getAvailableMenuByEateryId(poi.id);

      setMenuItems(menu);   // store menu
      setShowMenu(true);    // open popup
    }
    catch (error) {
      console.error("Failed to load menu:", error);
    }
  };
  
  if (!poi) {
    return (
      <div className="p-4 text-gray-500">
        Select a place on the map
      </div>
    );
  }

  console.log("Image URL:", `${BACKEND_URL}${poi.imagePath}`);

  return (
    <>
    <div className="h-full overflow-y-auto">

      {/* IMAGE */}
      <div className="h-56 bg-gray-200 flex items-center justify-center text-gray-500">
        {poi.imagePath ? (
          <img
            src={`${BACKEND_URL}${poi.imagePath}`}
            alt={poi.name}
            className="w-full h-56 object-cover"
          />
          ) : (
            <div className="h-56 bg-gray-200 flex items-center justify-center text-gray-500">
              Không có ảnh
            </div>
          )
        }
      </div>

      {/* CONTENT */}
      <div className="p-4 space-y-4">

        {/* NAME */}
        <h2 className="text-2xl font-bold">
          {poi.name}
        </h2>

        {/* BASIC INFO */}
        <div className="space-y-2 text-sm text-gray-600">

          <div>
            <span className="font-semibold">Danh mục:</span>{" "}
            {poi.categoryName}
          </div>

          <div>
            <span className="font-semibold">Giờ mở cửa:</span>{" "}
            {poi.openTime} - {poi.closeTime}
          </div>

          <div>
            <span className="font-semibold">Tầm giá:</span>{" "}
            {poi.priceRange}
          </div>

        </div>

        {/* DESCRIPTION */}
        <div>
          <h3 className="font-semibold mb-2">
            Mô tả
          </h3>

          <p className="text-gray-700">
            {poi.description}
          </p>
        </div>

        {/* ACTION */}
        <div className="space-y-2">

          <button
            onClick={handleViewMenu}
            className="w-full py-2 bg-green-500 text-white rounded hover:bg-blue-600"
          >
            Xem Menu
          </button>

          <button
            className="w-full py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
          >
            Đi tới đây
          </button>

        </div>

      </div>
    </div>

    
    {showMenu && (
      <MenuPopup
        menuItems={menuItems}
        onClose={() => setShowMenu(false)}
      />
    )}

  </>
  );
}

/*
Ghi chú
- Đây là panel hiển thị đầy đủ thông tin của POI khi nhấn marker. Panel này sẽ có hình ảnh,
mô tả đầy đủ của POI.
*/