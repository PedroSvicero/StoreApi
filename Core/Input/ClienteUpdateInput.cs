using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    /// <summary>
    /// Dados enviados para atualizar um cliente existente.
    /// </summary>
    public class ClienteUpdateInput
    {
        /// <summary>
        /// Novo nome do cliente.
        /// </summary>
        [Required(ErrorMessage = "Nome e obrigatorio")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no maximo 100 caracteres")]
        public required string Nome { get; set; }

        /// <summary>
        /// Nova data de nascimento do cliente.
        /// </summary>
        [Required(ErrorMessage = "Data de nascimento e obrigatoria")]
        public DateTime DataNascimento { get; set; }
    }
}
