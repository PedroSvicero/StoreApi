namespace Core.DTO
{
    /// <summary>
    /// Resposta devolvida no login com o token JWT.
    /// </summary>
    public class AuthTokenDto
    {
        /// <summary>
        /// Token JWT que deve ser enviado no cabecalho Authorization.
        /// </summary>
        public required string Token { get; set; }
    }
}
