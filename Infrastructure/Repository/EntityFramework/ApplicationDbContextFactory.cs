using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository.EntityFramework
{
    // Factory usada pelo EF Core em tempo de design.
    // Ela existe principalmente para comandos de migration e update database.
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Em tempo de design (migrations), o EF nao passa pelo Program.cs.
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "FiapStoreApi");

            // Le os mesmos arquivos de configuracao da API para evitar divergencias.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // Reproduz a mesma configuracao usada na API para manter o comportamento consistente.
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
