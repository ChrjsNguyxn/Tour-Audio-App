using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("api/eateries")] // route cứng hoặc route theo api/[controller] đều được
public class EateryController : ControllerBase
{
    private readonly EateryService _eateryService;

    public EateryController(EateryService eateryService)
    {
        _eateryService = eateryService;
    }

    /// <summary>
    /// Get all eateries.
    /// GET: /api/eateries
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllWithCategoryName()
    {
        var eateries = await _eateryService.GetAllWithCategoryNameAsync();

        return Ok(eateries);
    }
}