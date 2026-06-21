import {
  Map,
  MapControls,
  MapMarker,
  MarkerContent,
  MarkerPopup
} from "@/components/ui/map";
import UserMarker from "./UserMarker";
import POIMarker from "./POIMarker";

// hàm nhận userLocation để hiển thị(mark) vị trí hiện tại của user
// latitude; longtitude
export default function MapView({ userLocation, pois, onSelectPOI}) {

   console.log("MapView:", userLocation);


  return (
    <div className="h-full w-full">
      <Map
      center={userLocation ?? [106.705, 10.759]}
      zoom={14}
      >
        <MapControls 
          showCompass
          showLocate
          showZoom
        />

        // User marker
        <UserMarker location={userLocation} />

        // POI marker: các marker địa điểm. POI sẽ là mảng nên phải chạy mảng
        {pois.map((poi) => (
          <POIMarker
            key={poi.id}
            poi={poi}
            onSelectPOI={onSelectPOI}
          />
        ))}
      </Map>
    </div>
  );
}