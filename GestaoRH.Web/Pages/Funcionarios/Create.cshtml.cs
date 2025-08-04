using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoRH.Core.Models;
using System.Net.Http.Json;

namespace GestaoRH.Web.Pages.Funcionarios
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Funcionario Funcionario { get; set; }

        public List<SelectListItem> CargosOptions { get; set; }

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.GetAsync("/api/cargos");

            if (response.IsSuccessStatusCode)
            {
                var cargos = await response.Content.ReadFromJsonAsync<List<Cargo>>();
                CargosOptions = cargos
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nome })
                    .ToList();
            }
            else
            {
                CargosOptions = new();
                ModelState.AddModelError(string.Empty, "Erro ao carregar cargos.");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // Recarrega os cargos
                return Page();
            }

            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync("/api/funcionarios", Funcionario);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Erro ao criar funcionário.");
            await OnGetAsync(); // Recarrega os cargos
            return Page();
        }
    }
}
