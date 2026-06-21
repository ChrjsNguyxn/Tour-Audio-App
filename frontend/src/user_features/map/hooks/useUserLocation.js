import { useEffect, useState } from "react";

export default function useUserLocation() {
  const [location, setLocation] = useState(null);

  useEffect(() => {
    navigator.geolocation.getCurrentPosition(
      (position) => {
        setLocation([
          position.coords.longitude,
          position.coords.latitude,
        ]);
      },
      (error) => {
        console.error(error);
      }
    );
  }, []);

  console.log(location);
  return location;
}

/*
- Ghi chú: hàm dùng để xác định vị trí hiện tại của người dùng. Hàm gọi API của Geolocation và
trả về chuỗi json chứa vĩ độ và kinh độ của người dùng.
*/