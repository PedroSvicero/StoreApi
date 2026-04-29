using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    // Mapeamento EF da entidade Livro para a tabela Livros.
    internal class LivroConfiguration : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            // Define como a entidade Livro vira estrutura fisica no SQL Server.
            builder.ToTable("Livros");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(e => e.DataCriacao).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
            builder.Property(e => e.Editora).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(e => e.Nome).HasColumnType("VARCHAR(100)").IsRequired();
        }
    }
}
