using Microsoft.AspNetCore.Mvc.RazorPages;
using GestaoRH.Core.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace GestaoRH.Web.Pages.Cargos
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public List<Cargo> Cargos { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? FiltroNome { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? FiltroSalarioMin { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? FiltroSalarioMax { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");
            var todosCargos = await client.GetFromJsonAsync<List<Cargo>>("/api/cargos") ?? new();

            var query = todosCargos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(FiltroNome))
                query = query.Where(c => c.Nome.Contains(FiltroNome, StringComparison.OrdinalIgnoreCase));

            if (FiltroSalarioMin.HasValue)
                query = query.Where(c => c.FaixaSalarialMin >= FiltroSalarioMin.Value);

            if (FiltroSalarioMax.HasValue)
                query = query.Where(c => c.FaixaSalarialMax <= FiltroSalarioMax.Value);

            Cargos = query.ToList();
        }
    }
}
