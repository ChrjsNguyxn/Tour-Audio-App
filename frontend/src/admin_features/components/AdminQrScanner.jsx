import { useEffect, useRef, useState } from "react";
import { Html5Qrcode } from "html5-qrcode";

const API_BASE = "http://localhost:5092/api/v1";

// Lấy id quán từ URL QR, vd: http://localhost:5174/eatery/12 -> 12
function extractEateryId(qrText) {
    const match = qrText.match(/\/eatery\/(\d+)/);
    return match ? parseInt(match[1], 10) : null;
}

export default function AdminQrScanner() {
    const [eatery, setEatery] = useState(null);
    const [error, setError] = useState("");
    const scannerRef = useRef(null);
    const scannedRef = useRef(false);

    useEffect(() => {
        scannedRef.current = false;
        const html5QrCode = new Html5Qrcode("admin-qr-reader");
        scannerRef.current = html5QrCode;

        html5QrCode.start(
            { facingMode: "environment" },
            { fps: 10, qrbox: { width: 260, height: 260 } },
            async (decodedText) => {
                if (scannedRef.current) return;
                scannedRef.current = true;

                const id = extractEateryId(decodedText);
                if (!id) {
                    setError("QR không hợp lệ: " + decodedText);
                    return;
                }

                try {
                    // Dùng lại API admin-all đã có sẵn, không sửa backend
                    const res = await fetch(`${API_BASE}/eatery/admin-all`);
                    const all = await res.json();
                    const found = all.find((e) => e.id === id);
                    if (found) setEatery(found);
                    else setError(`Không tìm thấy quán có id = ${id}`);
                } catch {
                    setError("Lỗi khi gọi API");
                }
            },
            () => { } // ignore lỗi quét lẻ tẻ
        );

        return () => {
            html5QrCode.stop().then(() => html5QrCode.clear()).catch(() => { });
        };
    }, []);

    const approve = async (isApproved) => {
        await fetch(`${API_BASE}/eatery/${eatery.id}/approve`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ isApproved }),
        });
        setEatery({ ...eatery, isApproved });
    };

    return (
        <div style={{ maxWidth: 420, margin: "0 auto", padding: 20 }}>
            <h2>Quét QR quán ăn</h2>
            <div id="admin-qr-reader" style={{ width: "100%" }} />

            {error && <p style={{ color: "red" }}>{error}</p>}

            {eatery && (
                <div style={{ marginTop: 16, border: "1px solid #ddd", padding: 14, borderRadius: 8 }}>
                    <h3>{eatery.name}</h3>
                    <p>{eatery.address}</p>
                    <p>Trạng thái: {eatery.isApproved ? "Đã duyệt" : "Chưa duyệt"}</p>
                    {!eatery.isApproved && (
                        <button onClick={() => approve(true)}>Duyệt quán này</button>
                    )}
                </div>
            )}
        </div>
    );
}