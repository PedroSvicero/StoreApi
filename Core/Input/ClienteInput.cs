using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    /// <summary>
    /// Dados enviados para cadastrar um cliente.
    /// </summary>
    public class ClienteInput
    {
        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        [Required(ErrorMessage = "Nome e obrigatorio")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no maximo 100 caracteres")]
        public required string Nome { get; set; }

        /// <summary>
        /// CPF do cliente com 11 digitos.
        /// </summary>
        [Required(ErrorMessage = "CPF e obrigatorio")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter exatamente 11 digitos")]
        public required string CPF { get; set; }

        /// <summary>
        /// Data de nascimento do cliente.
        /// </summary>
        [Required(ErrorMessage = "Data de nascimento e obrigatoria")]
        public DateTime DataNascimento { get; set; }
    }
}
