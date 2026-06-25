using Microsoft.AspNetCore.Mvc;
using FoodMapAPI.DTOs;
using FoodMapAPI.Models;
using FoodMapAPI.Service;

namespace FoodMapAPI.Controllers
{
    [ApiController]
    [Route("api/eateries")]
    public class EateryController : ControllerBase
    {
        private readonly IEateryService _eateryService;

        public EateryController(IEateryService eateryService)
        {
            _eateryService = eateryService;
        }

        // GET: api/eateries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eatery>>> GetAll()
        {
            return Ok(await _eateryService.GetAllAsync());
        }

        // GET: api/eateries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Eatery>> GetById(int id)
        {
            var eatery = await _eateryService.GetByIdAsync(id);
            if (eatery == null) return NotFound();
            return Ok(eatery);
        }

        // GET: api/eateries/owner/5
        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<Eatery>>> GetByOwner(int ownerId)
        {
            return Ok(await _eateryService.GetByOwnerIdAsync(ownerId));
        }

        // POST: api/eateries
        [HttpPost]
        public async Task<ActionResult<Eatery>> Create(EateryDto dto)
        {
            try
            {
                var created = await _eateryService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/eateries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EateryDto dto)
        {
            try
            {
                var success = await _eateryService.UpdateAsync(id, dto);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/eateries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _eateryService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}