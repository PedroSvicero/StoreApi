using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Configurations
{
    // Mapeamento EF da entidade Cliente para a tabela Clientes.
    // Arquivos de Configuration servem para tirar detalhes de banco de dentro da entidade.
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cliente> builder)
        {
            // Define nome da tabela e chave primaria.
            builder.ToTable("Clientes");
            builder.HasKey(e => e.Id);

            // Define tipos e restricoes das colunas no banco.
            builder.Property(e => e.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(e => e.DataCriacao).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
            builder.Property(e => e.Nome).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(e => e.DataNascimento).HasColumnType("DATETIME");
            builder.Property(e => e.CPF).HasColumnType("VARCHAR(11)").IsRequired();
        }
    }
}
