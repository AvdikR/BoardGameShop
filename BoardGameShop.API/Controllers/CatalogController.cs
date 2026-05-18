using BoardGameShop.Application.DTOs;
using BoardGameShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        // -----------------------------
        // GET: api/catalog
        // -----------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var catalogs = await _catalogService.GetAllAsync();
            return Ok(catalogs);
        }

        // -----------------------------
        // GET: api/catalog/{id}
        // -----------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var catalog = await _catalogService.GetByIdAsync(id);

            if (catalog == null)
                return NotFound($"Catalog with ID {id} not found");

            return Ok(catalog);
        }

        // -----------------------------
        // POST: api/catalog
        // -----------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCatalogDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _catalogService.CreateAsync(dto);

            return Ok("Catalog created successfully");
        }
    }
}
