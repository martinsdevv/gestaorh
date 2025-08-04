using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestaoRH.Core.Models;
using System.Net.Http.Json;

namespace GestaoRH.Web.Pages.Funcionarios
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Funcionario Funcionario { get; set; }

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.GetAsync($"/api/funcionarios/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            Funcionario = await response.Content.ReadFromJsonAsync<Funcionario>() ?? new();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.DeleteAsync($"/api/funcionarios/{Funcionario.Id}");

            return RedirectToPage("Index");
        }
    }
}
