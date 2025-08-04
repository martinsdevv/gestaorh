using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using GestaoRH.Core.Models;

namespace GestaoRH.Web.Pages.Funcionarios
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Funcionario> Funcionarios { get; set; } = new();
        public List<Cargo> Cargos { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? FiltroNome { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FiltroCargoId { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");

            var todosFuncionarios = await client.GetFromJsonAsync<List<Funcionario>>("/api/funcionarios") ?? new();
            Cargos = await client.GetFromJsonAsync<List<Cargo>>("/api/cargos") ?? new();

            var query = todosFuncionarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(FiltroNome))
                query = query.Where(f => f.Nome.Contains(FiltroNome, StringComparison.OrdinalIgnoreCase));

            if (FiltroCargoId.HasValue)
                query = query.Where(f => f.CargoId == FiltroCargoId.Value);

            Funcionarios = query.ToList();
        }
    }
}
