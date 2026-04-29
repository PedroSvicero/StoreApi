using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    /// <summary>
    /// Dados enviados para atualizar um livro.
    /// </summary>
    public class LivroUpdateInput
    {
        /// <summary>
        /// Novo titulo do livro.
        /// </summary>
        [Required(ErrorMessage = "Nome e obrigatorio")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no maximo 100 caracteres")]
        public required string Nome { get; set; }

        /// <summary>
        /// Nova editora do livro.
        /// </summary>
        [Required(ErrorMessage = "Editora e obrigatoria")]
        [MaxLength(100, ErrorMessage = "Editora deve ter no maximo 100 caracteres")]
        public required string Editora { get; set; }
    }
}
