using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using GestaoRH.Core.DTOs;

namespace GestaoRH.Web.Pages.Conta
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public RegisterRequest Usuario { get; set; } = new();

        [TempData]
        public string? Erro { get; set; }

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");

            var response = await client.PostAsJsonAsync("/api/auth/register", Usuario);
            if (!response.IsSuccessStatusCode)
            {
                Erro = "Erro ao registrar. Verifique os dados.";
                return Page();
            }

            return RedirectToPage("Login");
        }
    }
}
