using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace GestaoRH.Core.Models
{
    public class Funcionario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do funcionário é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O cargo é obrigatório")]
        public int CargoId { get; set; }

        [ValidateNever]
        public Cargo Cargo { get; set; } = null!;

        [Required(ErrorMessage = "O salário atual é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "O salário deve ser maior que zero")]
        public decimal SalarioAtual { get; set; }

        [Required(ErrorMessage = "A data de admissão é obrigatória")]
        public DateTime DataAdmissao { get; set; }

        public ICollection<HistoricoSalario> HistoricoSalarios { get; set; } = new List<HistoricoSalario>();
    }

}
