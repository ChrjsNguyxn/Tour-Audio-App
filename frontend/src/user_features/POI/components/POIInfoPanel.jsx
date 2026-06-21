export default function POIInfoPanel({ poi }) {
  if (!poi) {
    return (
      <div className="p-4 text-gray-500">
        Select a place on the map
      </div>
    );
  }

  return (
    <div className="h-full overflow-y-auto">

      {/* IMAGE */}
      <div className="h-56 bg-gray-200 flex items-center justify-center text-gray-500">
        {poi.imagePath ? (
          <img
            src={poi.imagePath}
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
        <button
          className="w-full py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
        >
          Navigate Here
        </button>

      </div>
    </div>
  );
}

/*
Ghi chú
- Đây là panel hiển thị đầy đủ thông tin của POI khi nhấn marker. Panel này sẽ có hình ảnh,
mô tả đầy đủ của POI.
*/