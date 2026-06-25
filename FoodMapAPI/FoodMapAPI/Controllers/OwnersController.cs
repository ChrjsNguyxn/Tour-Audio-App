using Microsoft.AspNetCore.Mvc;
using FoodMapAPI.DTOs;
using FoodMapAPI.Service;

namespace FoodMapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerService;

        public OwnersController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        // POST: api/owners/register
        [HttpPost("register")]
        public async Task<ActionResult<OwnerResponseDto>> Register(RegisterOwnerDto dto)
        {
            try
            {
                var result = await _ownerService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/owners/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginOwnerDto dto)
        {
            try
            {
                var result = await _ownerService.LoginAsync(dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // GET: api/owners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerResponseDto>> GetOwner(int id)
        {
            var owner = await _ownerService.GetByIdAsync(id);
            if (owner == null) return NotFound();
            return Ok(owner);
        }
    }
}