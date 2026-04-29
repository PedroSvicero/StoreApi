using Core.DTO;
using Core.Input;

namespace Core.Service
{
    /// <summary>
    /// Contrato da camada de serviço de autenticação.
    /// Fica no Core — define o contrato sem saber nada de JWT ou HTTP.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica o usuário e retorna o token JWT.
        /// Retorna null se as credenciais forem inválidas.
        /// </summary>
        string? Login(LoginInput input);

        /// <summary>
        /// Registra um novo usuário.
        /// Lança exceção se o email já estiver em uso.
        /// </summary>
        UsuarioDto Register(RegisterInput input);
    }
}