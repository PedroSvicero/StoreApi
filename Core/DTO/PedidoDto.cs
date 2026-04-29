namespace Core.DTO
{
    /// <summary>
    /// Pedido devolvido pela API.
    /// </summary>
    public class PedidoDto
    {
        /// <summary>
        /// Identificador do pedido.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Data em que o pedido foi criado.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Identificador do cliente associado.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificador do livro associado.
        /// </summary>
        public int LivroId { get; set; }

        /// <summary>
        /// Cliente relacionado, quando a API devolver esse detalhe expandido.
        /// </summary>
        public ClienteDto? Cliente { get; set; }

        /// <summary>
        /// Livro relacionado, quando a API devolver esse detalhe expandido.
        /// </summary>
        public LivroDTO? Livro { get; set; }
    }
}
