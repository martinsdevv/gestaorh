using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestaoRH.Core.Models;
using System.Net.Http.Json;

namespace GestaoRH.Web.Pages.Funcionarios
{
    public class HistoricoModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Funcionario Funcionario { get; set; }
        public List<HistoricoSalario> Historico { get; set; }

        public HistoricoModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("Api");

            // Buscar funcionário
            var funcionarioResponse = await client.GetAsync($"/api/funcionarios/{id}");
            if (!funcionarioResponse.IsSuccessStatusCode)
                return RedirectToPage("Index");

            Funcionario = await funcionarioResponse.Content.ReadFromJsonAsync<Funcionario>() ?? new();

            // Buscar histórico
            var historicoResponse = await client.GetAsync($"/api/funcionarios/{id}/historico");
            if (!historicoResponse.IsSuccessStatusCode)
            {
                Historico = new();
                ModelState.AddModelError(string.Empty, "Erro ao carregar histórico salarial.");
            }
            else
            {
                Historico = await historicoResponse.Content.ReadFromJsonAsync<List<HistoricoSalario>>() ?? new();
            }

            return Page();
        }
    }
}
