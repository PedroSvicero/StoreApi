using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    /// <summary>
    /// Dados enviados para cadastrar um livro.
    /// </summary>
    public class LivroInput
    {
        /// <summary>
        /// Titulo do livro.
        /// </summary>
        [Required(ErrorMessage = "Nome e obrigatorio")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no maximo 100 caracteres")]
        public required string Nome { get; set; }

        /// <summary>
        /// Editora responsavel pelo livro.
        /// </summary>
        [Required(ErrorMessage = "Editora e obrigatoria")]
        [MaxLength(100, ErrorMessage = "Editora deve ter no maximo 100 caracteres")]
        public required string Editora { get; set; }
    }
}
