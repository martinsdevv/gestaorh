using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestaoRH.Core.Models;
using System.Net.Http.Json;

namespace GestaoRH.Web.Pages.Cargos
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Cargo Cargo { get; set; } = new();

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var cargo = await client.GetFromJsonAsync<Cargo>($"/api/cargos/{id}");
            if (cargo == null) return NotFound();

            Cargo = cargo;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.PutAsJsonAsync($"/api/cargos/{Cargo.Id}", Cargo);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Erro ao atualizar cargo.");
            return Page();
        }
    }
}
