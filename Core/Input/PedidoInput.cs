using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    /// <summary>
    /// Dados enviados para criar um pedido.
    /// </summary>
    public class PedidoInput
    {
        /// <summary>
        /// Identificador do cliente que esta fazendo o pedido.
        /// </summary>
        [Required(ErrorMessage = "ClienteId e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "ClienteId deve ser maior que zero")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificador do livro solicitado.
        /// </summary>
        [Required(ErrorMessage = "LivroId e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "LivroId deve ser maior que zero")]
        public int LivroId { get; set; }
    }
}
