using backend.DTOs;
using backend.Repository;

namespace backend.Services;

// Tất cả mọi method trong đây mặc định luôn chỉ lấy dữ liệu với điều kiện
// dữ liệu đó đang hoạt động.
public class TouristService
{
    private readonly MixedRepository _mixedRepository;

    public TouristService(MixedRepository mixedRepository)
    {
        _mixedRepository = mixedRepository;
    }

    // lấy POI cho controller
    public async Task<List<POIResponseDTO>> GetAllPOIsAsync()
    {
        return await _mixedRepository.GetApprovedPOIsAsync();
    }

    // Lấy một POI cụ thể theo id, có thể trả về null
    public async Task<POIResponseDTO> GetPOIbyID(int id)
    {
        return await _mixedRepository.GetApprovedPOIByIdAsync(id);
    }
}