namespace Core.Entity
{
    // Representa um livro disponivel para ser pedido.
    public class Livro : EntityBase
    {
        public required string Editora { get; set; }
        public required string Nome { get; set; }

        // Navegacao para todos os pedidos que referenciam este livro.
        public virtual ICollection<Pedido> Pedidos { get; set; } = [];
    }
}
