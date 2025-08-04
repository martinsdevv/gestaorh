using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GestaoRH.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Tenta pegar da variável de ambiente
            var envConnection = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");

            string connectionString;

            if (!string.IsNullOrEmpty(envConnection))
            {
                connectionString = envConnection;
                Console.WriteLine("Usando conexão da variável de ambiente.");
            }
            else
            {
                // Fallback para o efcore.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("efcore.json")
                    .Build();

                connectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("DefaultConnection");
                Console.WriteLine("Usando conexão do efcore.json.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
