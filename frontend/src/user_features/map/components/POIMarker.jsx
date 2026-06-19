import {
  MapMarker,
  MarkerContent,
  MarkerPopup
} from "@/components/ui/map";

export default function POIMarker({ poi, onSelectPOI }) {
  console.log("onSelect", onSelectPOI);
  
  return (
    <MapMarker
      longitude={poi.longitude}
      latitude={poi.latitude}
    >
      <MarkerContent>
        <div className="size-5 cursor-pointer rounded-full border-2 border-white 
        bg-rose-500 shadow-lg transition-transform hover:scale-110" 
        onClick={() => onSelectPOI(poi)}
        />
      </MarkerContent>

      <MarkerPopup>
        <div>
          <h3>{poi.name}</h3>
          <p>{poi.description}</p>
        </div>
      </MarkerPopup>
    </MapMarker>
  );
}