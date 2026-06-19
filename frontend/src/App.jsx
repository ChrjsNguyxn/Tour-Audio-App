import useUserLocation from "./user_features/map/hooks/useUserLocation";
import MapView from "./user_features/map/components/MapView";
import POIInfoPanel from "./user_features/POI/components/POIInfoPanel";
import { useState } from "react";

function App() {
  const userLocation = useUserLocation();

  // biến lưu POI đã chọn trên map
  const [selectedPOI, setSelectedPOI] = useState(null);

  // dữ liệu giả cho POI
  const pois = [
    {
      id: 1,
      name: "Bún Bò Huế O Mai",
      longitude: 106.6992,
      latitude: 10.7581,
      category: "noodle",
      description: "Spicy Hue beef noodle soup with rich broth.",
    },
    {
      id: 2,
      name: "Cơm Tấm Ba Ghiền",
      longitude: 106.6845,
      latitude: 10.7742,
      category: "rice",
      description: "Famous broken rice with grilled pork chop.",
    },
    {
      id: 3,
      name: "Bánh Mì Hòa Mã",
      longitude: 106.6931,
      latitude: 10.7698,
      category: "sandwich",
      description: "Traditional Vietnamese street baguette.",
    },
    {
      id: 4,
      name: "Phở Lý Quốc Sư Mini",
      longitude: 106.7015,
      latitude: 10.7602,
      category: "noodle",
      description: "Classic northern beef pho.",
    },
    {
      id: 5,
      name: "Trà Sữa Uncle Tea",
      longitude: 106.6958,
      latitude: 10.7633,
      category: "milktea",
      description: "Milk tea with brown sugar pearls.",
    }
  ];

  //console.log("App:", userLocation);

  return (
    <div className="h-screen flex flex-col">
      {/* HEADER */}
      <header className="h-14 border-b flex items-center px-4 bg-white">
        <h1 className="font-bold">Map Explorer</h1>
      </header>

      {/* MAIN AREA (MAP + POI PANEL) */}
      <div className="flex flex-1 min-h-0">
        
        {/* MAP (2/3) */}
        <div className="w-2/3 min-h-0">
          <MapView
            userLocation={userLocation}
            pois={pois}
            onSelectPOI={setSelectedPOI}
          />
        </div>

        {/* POI INFO (1/3) */}
        <div className="w-1/3 border-l bg-white overflow-y-auto">
          <POIInfoPanel poi={selectedPOI} />
        </div>
      </div>

      {/* FEATURES BAR */}
      <div className="h-14 border-t flex items-center gap-2 px-4 bg-gray-50">
        <button className="px-3 py-1 bg-gray-200 rounded">
          Filter
        </button>

        <button className="px-3 py-1 bg-gray-200 rounded">
          Routing
        </button>

        <button className="px-3 py-1 bg-gray-200 rounded">
          Search
        </button>
      </div>

      {/* FOOTER (optional) */}
      <footer className="h-10 border-t flex items-center px-4 text-xs text-gray-500">
        Map project prototype
      </footer>
    </div>
  );
}

export default App;