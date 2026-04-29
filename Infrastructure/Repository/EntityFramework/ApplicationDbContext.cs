using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.EntityFramework
{
    // DbContext e a porta do Entity Framework para o banco.
    // Ele conhece quais entidades existem e como aplicar suas configuracoes.
    public class ApplicationDbContext : DbContext
    {
        private readonly string? _connectionString;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Cada DbSet representa uma colecao consultavel/persistivel de uma entidade.
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Livro> Livro { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrWhiteSpace(_connectionString))
            {
                // Esse caminho alternativo permite criar o contexto tambem fora do fluxo normal da API.
                optionsBuilder.UseSqlServer(_connectionString);
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Procura automaticamente todas as classes IEntityTypeConfiguration do assembly.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
