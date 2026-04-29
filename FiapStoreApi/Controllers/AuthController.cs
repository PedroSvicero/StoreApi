using Core.DTO;
using Core.Input;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace FiapStoreApi.Controllers
{
    /// <summary>
    /// Endpoints publicos de autenticacao e registro.
    /// </summary>
    /// <remarks>
    /// Fluxo recomendado:
    /// 1. Registre um usuario.
    /// 2. Faca login.
    /// 3. Use o token JWT retornado no botao Authorize do Swagger.
    /// </remarks>
    [ApiController]
    [Route("/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Autentica um usuario e devolve o token JWT.
        /// </summary>
        /// <remarks>
        /// Exemplo de body:
        ///
        ///     POST /Auth/login
        ///     {
        ///       "email": "admin@fiapstore.com",
        ///       "senha": "123456"
        ///     }
        ///
        /// O token devolvido deve ser enviado no cabecalho:
        /// `Authorization: Bearer {token}`.
        /// </remarks>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="401">Email ou senha invalidos.</response>
        /// <response code="500">Falha inesperada no servidor.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody] LoginInput input)
        {
            try
            {
                var token = _authService.Login(input);

                // Nulo significa falha de autenticacao; um token real libera acesso aos endpoints protegidos.
                if (token is null)
                    return Unauthorized("Email ou senha invalidos.");

                return Ok(new AuthTokenDto { Token = token });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Registra um novo usuario da API.
        /// </summary>
        /// <remarks>
        /// Exemplo de body:
        ///
        ///     POST /Auth/register
        ///     {
        ///       "email": "admin@fiapstore.com",
        ///       "senha": "123456",
        ///       "role": "Admin"
        ///     }
        ///
        /// O campo `role` aceita apenas `Admin` ou `User`.
        /// </remarks>
        /// <response code="201">Usuario criado com sucesso.</response>
        /// <response code="400">Dados invalidos ou email ja utilizado.</response>
        /// <response code="500">Falha inesperada no servidor.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromBody] RegisterInput input)
        {
            try
            {
                var usuario = _authService.Register(input);
                return StatusCode(StatusCodes.Status201Created, usuario);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
