namespace Core.Entity
{
    // Classe base das entidades: concentra campos comuns reutilizados no dominio.
    public class EntityBase
    {
        // Chave primaria usada pelo EF Core e pelas relacoes entre tabelas.
        public int Id { get; set; }

        // Momento em que o registro foi criado no sistema.
        public DateTime DataCriacao { get; set; }
    }
}
