namespace Core.Entity
{
    // Representa o cliente no dominio e no banco de dados.
    public class Cliente : EntityBase
    {
        public required string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public required string CPF { get; set; }

        // Navegacao para os pedidos associados a este cliente.
        public virtual ICollection<Pedido> Pedidos { get; set; } = [];
    }
}
