import {
  MapMarker,
  MarkerContent,
} from "@/components/ui/map";

export default function UserMarker({ location }) {
  if (!location) {
    console.log('location fail');
    return null;
  }

  return (
    <MapMarker
      longitude={location[0]}
      latitude={location[1]}
    >
      <MarkerContent>
        <div className="size-5 cursor-pointer rounded-full border-2 border-white bg-blue-500 shadow-lg transition-transform hover:scale-110" />
      </MarkerContent>
    </MapMarker>
  );
}

/*
Ghi chú
- Hàm này dùng để đánh dấu vị trí hiện tại của user
- Sẽ có một marker màu xanh biển tại vị trí đó
*/