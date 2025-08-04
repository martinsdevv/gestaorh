using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoRH.Core.Models;
using System.Net.Http.Json;

namespace GestaoRH.Web.Pages.Funcionarios
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Funcionario Funcionario { get; set; }

        public List<SelectListItem> CargosOptions { get; set; }

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");

            var funcionario = await client.GetFromJsonAsync<Funcionario>($"/api/funcionarios/{id}");
            if (funcionario == null) return RedirectToPage("Index");

            Funcionario = funcionario;

            var cargos = await client.GetFromJsonAsync<List<Cargo>>("/api/cargos");
            CargosOptions = cargos?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList() ?? new();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCargosAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.PutAsJsonAsync($"/api/funcionarios/{Funcionario.Id}", Funcionario);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Erro ao atualizar funcionário.");
            await LoadCargosAsync();
            return Page();
        }

        private async Task LoadCargosAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var cargos = await client.GetFromJsonAsync<List<Cargo>>("/api/cargos");
            CargosOptions = cargos?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList() ?? new();
        }
    }
}
