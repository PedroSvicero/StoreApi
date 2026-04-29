using Microsoft.AspNetCore.Mvc;

namespace FiapStoreApi.Controllers
{
    /// <summary>
    /// Endpoint auxiliar para demonstrar logs da aplicacao.
    /// </summary>
    /// <remarks>
    /// Este endpoint nao representa regra de negocio.
    /// Ele existe apenas para mostrar como diferentes niveis de log podem ser gerados.
    /// </remarks>
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gera logs de informacao, aviso e erro para fins de teste.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Log()
        {
            // Cada chamada abaixo demonstra um nivel diferente de severidade.
            _logger.LogInformation("Log de informacao");
            _logger.LogWarning("Log de aviso");
            _logger.LogError("Log de erro");

            return Ok("Logs registrados com sucesso.");
        }
    }
}
