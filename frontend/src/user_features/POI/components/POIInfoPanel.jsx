export default function POIInfoPanel({ poi }) {
  if (!poi) {
    return (
      <div className="p-4 text-gray-500">
        Select a place on the map
      </div>
    );
  }

  return (
    <div className="p-4 space-y-3">
      <h2 className="text-xl font-bold">{poi.name}</h2>

      <div className="h-40 bg-gray-200 rounded" />

      <p className="text-sm text-gray-600">
        {poi.description}
      </p>

      <button className="w-full py-2 bg-blue-500 text-white rounded">
        Navigate here
      </button>
    </div>
  );
}

/*
Ghi chú
- Đây là panel hiển thị đầy đủ thông tin của POI khi nhấn marker. Panel này sẽ có hình ảnh,
mô tả đầy đủ của POI.
*/