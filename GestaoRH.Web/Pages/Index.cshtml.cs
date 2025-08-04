using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using GestaoRH.Core.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace GestaoRH.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public int TotalFuncionarios { get; set; }
        public int TotalCargos { get; set; }
        public decimal MediaSalarial { get; set; }

        public List<string> CargosNomes { get; set; } = new();
        public List<decimal> CargosMedias { get; set; } = new();

        public string UsuarioNome { get; set; } = string.Empty;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");

            // Envia o token da autenticação para a API
            if (Request.Cookies.TryGetValue("jwt", out var token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var funcionarios = await client.GetFromJsonAsync<List<Funcionario>>("/api/funcionarios") ?? new();
            var cargos = await client.GetFromJsonAsync<List<Cargo>>("/api/cargos") ?? new();

            TotalFuncionarios = funcionarios.Count;
            TotalCargos = cargos.Count;
            MediaSalarial = funcionarios.Any() ? funcionarios.Average(f => f.SalarioAtual) : 0;

            foreach (var cargo in cargos)
            {
                var funcionariosDoCargo = funcionarios.Where(f => f.CargoId == cargo.Id).ToList();
                if (funcionariosDoCargo.Any())
                {
                    CargosNomes.Add(cargo.Nome);
                    CargosMedias.Add(funcionariosDoCargo.Average(f => f.SalarioAtual));
                }
            }
        }
    }
}
