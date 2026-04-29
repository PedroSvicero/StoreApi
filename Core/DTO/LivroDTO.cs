namespace Core.DTO
{
    /// <summary>
    /// Livro devolvido pela API.
    /// </summary>
    public class LivroDTO
    {
        /// <summary>
        /// Identificador do livro.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Data de criacao do registro.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Editora do livro.
        /// </summary>
        public required string Editora { get; set; }

        /// <summary>
        /// Titulo do livro.
        /// </summary>
        public required string Nome { get; set; }

        /// <summary>
        /// Pedidos relacionados ao livro quando a consulta devolve esses detalhes.
        /// </summary>
        public ICollection<PedidoDto> Pedidos { get; set; } = [];
    }
}
