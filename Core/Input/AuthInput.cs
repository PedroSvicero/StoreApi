using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    /// <summary>
    /// Dados enviados no login.
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// Email do usuario que deseja autenticar na API.
        /// </summary>
        [Required(ErrorMessage = "Email e obrigatorio")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public required string Email { get; set; }

        /// <summary>
        /// Senha em texto puro informada no momento do login.
        /// </summary>
        [Required(ErrorMessage = "Senha e obrigatoria")]
        public required string Senha { get; set; }
    }

    /// <summary>
    /// Dados enviados para registrar um novo usuario.
    /// </summary>
    public class RegisterInput
    {
        /// <summary>
        /// Email que sera usado como identidade de acesso.
        /// </summary>
        [Required(ErrorMessage = "Email e obrigatorio")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public required string Email { get; set; }

        /// <summary>
        /// Senha inicial do usuario.
        /// </summary>
        [Required(ErrorMessage = "Senha e obrigatoria")]
        [MinLength(6, ErrorMessage = "Senha deve ter no minimo 6 caracteres")]
        public required string Senha { get; set; }

        /// <summary>
        /// Perfil de acesso do usuario. Neste projeto, pode ser Admin ou User.
        /// </summary>
        [Required(ErrorMessage = "Role e obrigatoria")]
        [RegularExpression("^(Admin|User)$", ErrorMessage = "Role deve ser 'Admin' ou 'User'")]
        public required string Role { get; set; }
    }
}
