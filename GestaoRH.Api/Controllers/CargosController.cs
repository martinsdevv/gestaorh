using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoRH.Data;
using GestaoRH.Core.Models;

namespace GestaoRH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CargosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/cargos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> GetCargos()
        {
            var cargos = await _context.Cargos.ToListAsync();
            return Ok(cargos);
        }

        // GET: api/cargos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cargo>> GetCargo(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null) return NotFound();
            return Ok(cargo);
        }

        // POST: api/cargos
        [HttpPost]
        public async Task<ActionResult<Cargo>> CreateCargo(Cargo cargo)
        {
            _context.Cargos.Add(cargo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCargo), new { id = cargo.Id }, cargo);
        }

        // PUT: api/cargos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCargo(int id, Cargo cargo)
        {
            if (id != cargo.Id) return BadRequest();

            var cargoExistente = await _context.Cargos.FindAsync(id);
            if (cargoExistente == null) return NotFound();

            _context.Entry(cargoExistente).CurrentValues.SetValues(cargo);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/cargos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCargo(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null) return NotFound();

            _context.Cargos.Remove(cargo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
