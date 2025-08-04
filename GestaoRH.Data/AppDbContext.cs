using Microsoft.EntityFrameworkCore;
using GestaoRH.Core.Models;

namespace GestaoRH.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<HistoricoSalario> HistoricoSalarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de precisão para propriedades decimal
            modelBuilder.Entity<Cargo>()
                .Property(c => c.FaixaSalarialMin)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cargo>()
                .Property(c => c.FaixaSalarialMax)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Funcionario>()
                .Property(f => f.SalarioAtual)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HistoricoSalario>()
                .Property(h => h.Salario)
                .HasPrecision(18, 2);
        }
    }
}
