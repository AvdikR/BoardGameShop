using BoardGameShop.Application.DTOs;
using BoardGameShop.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _service;

        public ReservationController(ReservationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }
    }
}
