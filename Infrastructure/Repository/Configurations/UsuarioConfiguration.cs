using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    // Mapeamento da entidade Usuario.
    // Ele define a tabela usada na autenticacao e suas restricoes.
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(e => e.DataCriacao).HasColumnType("DATETIME").IsRequired();
            builder.Property(e => e.Email).HasColumnType("VARCHAR(200)").IsRequired();
            builder.Property(e => e.SenhaHash).HasColumnType("VARCHAR(500)").IsRequired();
            builder.Property(e => e.Role).HasColumnType("VARCHAR(20)").IsRequired();

            // Email unico protege a regra de login e evita duplicidade de identidade.
            builder.HasIndex(e => e.Email).IsUnique();
        }
    }
}
