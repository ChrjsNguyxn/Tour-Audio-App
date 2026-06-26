import useUserLocation from "../map/hooks/useUserLocation";
import MapView from "../map/components/MapView";
import POIInfoPanel from "../POI/components/POIInfoPanel";
import QRScanner from "../POI/components/QRScanner";

import { useState, useEffect, useRef } from "react";

import { getAllVendors } from "../services/APIService";

import { Html5Qrcode } from "html5-qrcode";


function Home() {
  const userLocation = useUserLocation();

  const [selectedPOI, setSelectedPOI] = useState(null);

  const [pois, setPois] = useState([]);

  // QR scan
  const [showQRScanner, setShowQRScanner] = useState(false);
  
  const audioRef = useRef(new Audio());   // ← Add this

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

        audioFilePath: vendor.audioFilePath,
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

  const handleQRScan = (qrCode) => {
  setShowQRScanner(false);

  // Reset scanned state for next time
  setTimeout(() => {
    // Small delay to let cleanup finish
  }, 300);

  if (!qrCode || !pois.length) {
    alert("Không tìm thấy dữ liệu. Vui lòng thử lại.");
    return;
  }

  // QR code
  // Find the POI from local array
  const foundPOI = pois.find(poi => {
      return (
        poi.id === qrCode ||                    // if QR contains number id
        poi.id?.toString() === qrCode ||        // if QR contains string id
        poi.name?.toLowerCase() === qrCode.toLowerCase() // fallback (optional)
      );
    });

    if (foundPOI) {
      setSelectedPOI(foundPOI);

      // Auto play audio if available
      if (foundPOI.audioUrl) {
        audioRef.current.src = foundPOI.audioUrl;
        audioRef.current.play().catch(err => {
          console.log("Audio playback failed:", err);
        });
      }

      // Optional: Center map on this POI (we'll add this later)
      console.log("POI found:", foundPOI.name);
    } else {
      alert(`Không tìm thấy điểm tham quan với mã: ${qrCode}`);
    }
  };

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
        <button 
          onClick={() => setShowQRScanner(true)}
          className="px-3 py-1 bg-gray-200 rounded">
          Quét QR
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

      {/* QR Scanner */}
      {showQRScanner && (
        <QRScanner 
          onScanSuccess={handleQRScan} 
          onClose={() => setShowQRScanner(false)} 
        />
      )}
    </div>
  );
}

export default Home;