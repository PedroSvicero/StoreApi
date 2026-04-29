namespace Core.Entity
{
    // Entidade de associacao entre Cliente e Livro.
    public class Pedido : EntityBase
    {
        // Chaves estrangeiras persistidas no banco.
        public int ClienteId { get; set; }
        public int LivroId { get; set; }

        // Navegacoes usadas pelo EF para carregar os dados relacionados.
        public virtual Cliente? Cliente { get; set; }
        public virtual Livro? Livro { get; set; }
    }
}
