namespace Core.DTO
{
    /// <summary>
    /// Cliente devolvido pela API.
    /// </summary>
    public class ClienteDto
    {
        /// <summary>
        /// Identificador do cliente.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Data de criacao do registro.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        public required string Nome { get; set; }

        /// <summary>
        /// Data de nascimento do cliente.
        /// </summary>
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// CPF do cliente.
        /// </summary>
        public required string CPF { get; set; }

        /// <summary>
        /// Pedidos relacionados ao cliente quando a consulta pedir esse nivel de detalhe.
        /// </summary>
        public ICollection<PedidoDto> Pedidos { get; set; } = [];
    }
}
