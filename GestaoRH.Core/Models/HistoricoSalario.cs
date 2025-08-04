namespace GestaoRH.Core.Models
{
    public class HistoricoSalario
    {
        public int Id { get; set; }
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; } = null!;
        public decimal Salario { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
