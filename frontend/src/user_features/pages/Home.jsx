import useUserLocation from "../map/hooks/useUserLocation";
import MapView from "../map/components/MapView";
import POIInfoPanel from "../POI/components/POIInfoPanel";

import { useState, useEffect } from "react";

import { getAllVendors } from "../services/APIService";

function Home() {
  const userLocation = useUserLocation();

  const [selectedPOI, setSelectedPOI] = useState(null);

  const [pois, setPois] = useState([]);

  useEffect(() => {
    loadVendors();
  }, []);

  async function loadVendors() {
    try {
      const vendors = await getAllVendors();

      const mappedPOIs = vendors.map(vendor => ({
        id: vendor.id,
        name: vendor.name,
        longitude: vendor.longitude,
        latitude: vendor.latitude,

        priceRange: vendor.priceRange,
        
        category: vendor.categoryId,

        categoryName: vendor.categoryName,

        description:
          vendor.description ||
          "No description available",

        imagePath: vendor.imagePath,
        openTime: vendor.openTime,
        closeTime: vendor.closeTime
      }));

      setPois(mappedPOIs);

      console.log("Loaded vendors:", mappedPOIs);
    }
    catch (error) {
      console.error("Failed to load vendors:", error);
    }
  }

  return (
    <div className="h-screen flex flex-col">
      {/* HEADER */}
      <header className="h-14 border-b flex items-center px-4 bg-white">
        <h1 className="font-bold">Map Explorer</h1>
      </header>

      {/* MAIN AREA */}
      <div className="flex flex-1 min-h-0">

        {/* MAP */}
        <div className="w-2/3 min-h-0">
          <MapView
            userLocation={userLocation}
            pois={pois}
            onSelectPOI={setSelectedPOI}
          />
        </div>

        {/* INFO PANEL */}
        <div className="w-1/3 border-l bg-white overflow-y-auto">
          <POIInfoPanel 
            poi={selectedPOI} />
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

      <footer className="h-10 border-t flex items-center px-4 text-xs text-gray-500">
        Map project prototype
      </footer>
    </div>
  );
}

export default Home;