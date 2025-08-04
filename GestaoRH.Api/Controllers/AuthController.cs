using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoRH.Core.DTOs;
using GestaoRH.Api.Services;
using GestaoRH.Core.Models;
using GestaoRH.Data;

namespace GestaoRH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwt;

        public AuthController(AppDbContext context, JwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email já cadastrado.");

            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var token = _jwt.GenerateToken(usuario.Nome, usuario.Email);

            return Ok(new AuthResponse
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
                return Unauthorized("Email ou senha inválidos.");

            var token = _jwt.GenerateToken(usuario.Nome, usuario.Email);

            // Adiciona o token no cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(4)
            });

            return Ok(new AuthResponse
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email
            });
        }

    }
}
