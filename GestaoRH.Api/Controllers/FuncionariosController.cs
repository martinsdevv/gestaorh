using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoRH.Data;
using GestaoRH.Core.Models;

namespace GestaoRH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/funcionarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
        {
            var funcionarios = await _context.Funcionarios
                                             .Include(f => f.Cargo)
                                             .ToListAsync();
            return Ok(funcionarios);
        }

        // GET: api/funcionarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios
                                            .Include(f => f.Cargo)
                                            .FirstOrDefaultAsync(f => f.Id == id);
            if (funcionario == null) return NotFound();
            return Ok(funcionario);
        }

        // POST: api/funcionarios
        [HttpPost]
        public async Task<ActionResult<Funcionario>> CreateFuncionario(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.Id }, funcionario);
        }

        // PUT: api/funcionarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuncionario(int id, Funcionario funcionario)
        {
            if (id != funcionario.Id) return BadRequest();

            var funcionarioExistente = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Id == id);

            if (funcionarioExistente == null) return NotFound();

            // Verifica se houve alteração de salário
            if (funcionarioExistente.SalarioAtual != funcionario.SalarioAtual)
            {
                _context.HistoricoSalarios.Add(new HistoricoSalario
                {
                    FuncionarioId = funcionario.Id,
                    Salario = funcionario.SalarioAtual,
                    DataAlteracao = DateTime.Now
                });
            }

            _context.Entry(funcionarioExistente).CurrentValues.SetValues(funcionario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/historico")]
        public async Task<ActionResult<IEnumerable<HistoricoSalario>>> GetHistoricoSalario(int id)
        {
            var funcionarioExiste = await _context.Funcionarios.AnyAsync(f => f.Id == id);
            if (!funcionarioExiste) return NotFound();

            var historico = await _context.HistoricoSalarios
                                        .Where(h => h.FuncionarioId == id)
                                        .OrderByDescending(h => h.DataAlteracao)
                                        .ToListAsync();

            return Ok(historico);
        }


        // DELETE: api/funcionarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null) return NotFound();

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
