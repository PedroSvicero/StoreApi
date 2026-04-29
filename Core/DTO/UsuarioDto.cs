namespace Core.DTO
{
    /// <summary>
    /// Dados devolvidos pela API ao expor um usuario.
    /// </summary>
    public class UsuarioDto
    {
        /// <summary>
        /// Identificador do usuario.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Email do usuario.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Perfil de acesso do usuario.
        /// </summary>
        public required string Role { get; set; }
    }
}
