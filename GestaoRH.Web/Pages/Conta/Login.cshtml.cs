using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using GestaoRH.Core.DTOs;

namespace GestaoRH.Web.Pages.Conta
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public LoginRequest Login { get; set; } = new();

        [TempData]
        public string? Erro { get; set; }

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");

            var response = await client.PostAsJsonAsync("/api/auth/login", Login);
            if (!response.IsSuccessStatusCode)
            {
                Erro = "Login inválido.";
                return Page();
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result is null || string.IsNullOrEmpty(result.Token))
            {
                Erro = "Erro ao autenticar.";
                return Page();
            }

            HttpContext.Response.Cookies.Append("token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1)
            });

            return RedirectToPage("/Index");
        }
    }
}
