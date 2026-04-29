using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Configurations
{
    // Mapeamento EF da entidade Pedido e das relacoes com Cliente e Livro.
    // Aqui a infraestrutura explica ao EF como montar as foreign keys e navegacoes.
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Pedido> builder)
        {
            // Estrutura basica da tabela.
            builder.ToTable("Pedidos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(e => e.DataCriacao).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
            builder.Property(e => e.ClienteId).HasColumnType("INT").IsRequired();
            builder.Property(e => e.LivroId).HasColumnType("INT").IsRequired();

            // Um cliente pode ter muitos pedidos.
            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.Pedidos)
                .HasPrincipalKey(e => e.Id);

            // Um livro pode aparecer em muitos pedidos.
            builder.HasOne(e => e.Livro)
                .WithMany(c => c.Pedidos)
                .HasPrincipalKey(e => e.Id);
        }
    }
}
