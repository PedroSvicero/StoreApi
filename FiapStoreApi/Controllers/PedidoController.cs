using Core.DTO;
using Core.Input;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapStoreApi.Controllers
{
    /// <summary>
    /// Endpoints de pedidos.
    /// </summary>
    /// <remarks>
    /// Pedido conecta cliente e livro.
    /// O controller fala apenas com a camada de service.
    /// Internamente, o service usa Dapper nas leituras e repositorios/EF Core nas escritas.
    /// </remarks>
    [ApiController]
    [Route("/[controller]")]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        /// <summary>
        /// Lista todos os pedidos cadastrados.
        /// </summary>
        /// <remarks>
        /// A resposta pode trazer cliente e livro embutidos, o que ajuda a entender relacionamentos.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IList<PedidoDto>), StatusCodes.Status200OK)]
        public IActionResult GetTodos()
        {
            try
            {
                return Ok(_pedidoService.ObterTodos());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Busca um pedido especifico pelo identificador.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPorId([FromRoute] int id)
        {
            try
            {
                var pedido = _pedidoService.ObterPorId(id);
                if (pedido is null)
                    return NotFound($"Pedido com Id {id} nao encontrado.");

                return Ok(pedido);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Cria um novo pedido ligando cliente e livro.
        /// </summary>
        /// <remarks>
        /// Exemplo de body:
        ///
        ///     POST /Pedido
        ///     {
        ///       "clienteId": 1,
        ///       "livroId": 3
        ///     }
        ///
        /// Antes de salvar, a API valida se o cliente e o livro realmente existem.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post([FromBody] PedidoInput input)
        {
            try
            {
                var resultado = _pedidoService.Cadastrar(input);
                if (!resultado.Success)
                    return NotFound(resultado.ErrorMessage);

                return CreatedAtAction(nameof(GetPorId), new { id = resultado.Pedido!.Id }, resultado.Pedido);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Remove um pedido existente.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var deletado = _pedidoService.Deletar(id);
                if (!deletado)
                    return NotFound($"Pedido com Id {id} nao encontrado.");
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
