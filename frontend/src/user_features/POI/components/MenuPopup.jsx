export default function MenuPopup({ menuItems, onClose }) {
  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">

      <div className="bg-white w-[500px] max-h-[80vh] overflow-y-auto p-4 rounded">

        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-bold">Menu</h2>

          <button onClick={onClose}>
            ✕
          </button>
        </div>

        {menuItems.length === 0 ? (
          <p>No items available</p>
        ) : (
          menuItems.map(item => (
            <div key={item.id} className="border-b py-2">

              <div className="flex justify-between">
                <span className="font-medium">
                  {item.name}
                </span>

                <span className="text-green-600">
                  {item.price}đ
                </span>
              </div>

              <p className="text-sm text-gray-500">
                {item.description}
              </p>

            </div>
          ))
        )}

      </div>
    </div>
  );
}