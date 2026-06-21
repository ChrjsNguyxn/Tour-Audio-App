using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quan4.AdminApi.DTOs;
using Quan4.AdminApi.Services;
using System.Security.Claims;

namespace Quan4.AdminApi.Controllers
{
    [Route("api/v1/vendors")]
    [ApiController]
    [Authorize]
    public class VendorController : ControllerBase
    {
        private readonly VendorService _vendorService;

        // Tiêm VendorService vào Controller
        public VendorController(VendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVendor(CreateVendorRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();
            int currentUserId = int.Parse(userIdClaim);

            var result = await _vendorService.CreateVendorAsync(request, currentUserId);
            
            if (!result.Success) return BadRequest(result.Message);
            return Ok(new { message = result.Message });
        }

        [HttpGet("unapproved")]
        public async Task<IActionResult> GetUnapprovedVendors()
        {
            var vendors = await _vendorService.GetUnapprovedVendorsAsync();
            return Ok(vendors);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveVendor(int id)
        {
            var result = await _vendorService.ApproveVendorAsync(id);
            
            if (!result.Success) return NotFound(result.Message);
            return Ok(new { message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var result = await _vendorService.DeleteVendorAsync(id);
            
            if (!result.Success) return NotFound(result.Message);
            return Ok(new { message = result.Message });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllApprovedVendors()
        {
            var vendors = await _vendorService.GetAllApprovedVendorsAsync();
            return Ok(vendors);
        }
    }
}