import { useEffect, useRef } from "react";
import { Html5Qrcode } from "html5-qrcode";

// tạo một popup hướng dẫn quét mã QR
const QRScanner = ({ onScanSuccess, onClose }) => {
  const scannerRef = useRef(null);
  const scannedRef = useRef(false);

  useEffect(() => {
    scannedRef.current = false;

    const startScanner = async () => {
      try {
        const qrReader = document.getElementById("qr-reader");

        if (qrReader) {
          qrReader.innerHTML = "";
        }

        const html5QrCode = new Html5Qrcode("qr-reader");

        scannerRef.current = html5QrCode;

        await html5QrCode.start(
          { facingMode: "environment" },
          {
            fps: 10,
            qrbox: { width: 280, height: 280 },
          },
          (decodedText) => {
            if (scannedRef.current) return;

            scannedRef.current = true;

            onScanSuccess(decodedText);
          },
          () => {
            // Ignore scan failures
          }
        );
      } catch (err) {
        console.error("Failed to start scanner:", err);
      }
    };

    startScanner();

    return () => {
      const stopScanner = async () => {
        try {
          if (
            scannerRef.current &&
            scannerRef.current.isScanning
          ) {
            await scannerRef.current.stop();
          }

          if (scannerRef.current) {
            await scannerRef.current.clear();
          }
        } catch (err) {
          console.log("Scanner cleanup error:", err);
        }
      };

      stopScanner();
    };
  }, []);

  return (
    <div className="fixed inset-0 bg-black/90 z-50 flex items-center justify-center">
      <div className="bg-white rounded-2xl p-6 w-full max-w-md mx-4">
        <h2 className="text-2xl font-bold text-center mb-4">
          Quét mã QR
        </h2>

        <p className="text-center text-gray-600 mb-4">
          Di chuyển camera đến mã QR
        </p>

        <div
          id="qr-reader"
          className="w-full h-[350px] overflow-hidden rounded-xl"
        />

        <button
          onClick={onClose}
          className="mt-6 w-full py-3 bg-gray-200 hover:bg-gray-300 rounded-xl font-medium"
        >
          Hủy
        </button>
      </div>
    </div>
  );
};

export default QRScanner;