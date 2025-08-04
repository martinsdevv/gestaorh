using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GestaoRH.Core.Models
{
    public class Cargo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do cargo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A faixa salarial mínima é obrigatória")]
        [Range(0, double.MaxValue, ErrorMessage = "A faixa salarial mínima deve ser maior que zero")]
        public decimal FaixaSalarialMin { get; set; }

        [Required(ErrorMessage = "A faixa salarial máxima é obrigatória")]
        [Range(0, double.MaxValue, ErrorMessage = "A faixa salarial máxima deve ser maior que zero")]
        public decimal FaixaSalarialMax { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [ValidateNever]
        [JsonIgnore]
        public ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FaixaSalarialMax <= FaixaSalarialMin)
            {
                yield return new ValidationResult(
                    "A faixa salarial máxima deve ser maior que a mínima",
                    new[] { nameof(FaixaSalarialMax) });
            }
        }
    }

}
