using Core.DTO;
using Core.Input;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapStoreApi.Controllers
{
    /// <summary>
    /// Endpoints de clientes.
    /// </summary>
    /// <remarks>
    /// Leitura pode ser feita por qualquer usuario autenticado.
    /// Escrita fica restrita ao perfil Admin.
    /// </remarks>
    [ApiController]
    [Route("/[controller]")]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Lista todos os clientes cadastrados.
        /// </summary>
        /// <response code="200">Lista devolvida com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IList<ClienteDto>), StatusCodes.Status200OK)]
        public IActionResult GetTodos()
        {
            try
            {
                return Ok(_clienteService.ObterTodos());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Busca um cliente pelo identificador.
        /// </summary>
        /// <remarks>
        /// Exemplo de rota: `GET /Cliente/1`
        /// </remarks>
        /// <response code="200">Cliente encontrado.</response>
        /// <response code="404">Cliente nao localizado.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPorId([FromRoute] int id)
        {
            try
            {
                var cliente = _clienteService.ObterPorId(id);
                if (cliente is null)
                    return NotFound($"Cliente com Id {id} nao encontrado.");

                return Ok(cliente);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Busca um cliente trazendo apenas os pedidos dos ultimos seis meses.
        /// </summary>
        /// <remarks>
        /// Este endpoint e util para mostrar uma regra de negocio na camada de service.
        /// </remarks>
        /// <response code="200">Cliente encontrado com filtro temporal aplicado.</response>
        /// <response code="404">Cliente nao localizado.</response>
        [HttpGet("pedidos-seis-meses/{id:int}")]
        [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPedidosSeisMeses([FromRoute] int id)
        {
            try
            {
                var cliente = _clienteService.ObterComPedidosSeisMeses(id);
                if (cliente is null)
                    return NotFound($"Cliente com Id {id} nao encontrado.");

                return Ok(cliente);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Cadastra um novo cliente.
        /// </summary>
        /// <remarks>
        /// Exemplo de body:
        ///
        ///     POST /Cliente
        ///     {
        ///       "nome": "Maria Silva",
        ///       "cpf": "12345678901",
        ///       "dataNascimento": "1995-08-10T00:00:00"
        ///     }
        /// </remarks>
        /// <response code="201">Cliente criado com sucesso.</response>
        /// <response code="403">Usuario sem permissao de escrita.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Post([FromBody] ClienteInput input)
        {
            try
            {
                var clienteCriado = _clienteService.Cadastrar(input);

                // CreatedAtAction devolve 201 e informa ao cliente onde buscar o recurso criado.
                return CreatedAtAction(nameof(GetPorId), new { id = clienteCriado.Id }, clienteCriado);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Atualiza os dados de um cliente.
        /// </summary>
        /// <response code="200">Cliente atualizado com sucesso.</response>
        /// <response code="403">Usuario sem permissao de escrita.</response>
        /// <response code="404">Cliente nao localizado.</response>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put([FromRoute] int id, [FromBody] ClienteUpdateInput input)
        {
            try
            {
                var atualizado = _clienteService.Alterar(id, input);
                if (!atualizado)
                    return NotFound($"Cliente com Id {id} nao encontrado.");

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Remove um cliente pelo identificador.
        /// </summary>
        /// <response code="204">Cliente removido com sucesso.</response>
        /// <response code="403">Usuario sem permissao de escrita.</response>
        /// <response code="404">Cliente nao localizado.</response>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var deletado = _clienteService.Deletar(id);
                if (!deletado)
                    return NotFound($"Cliente com Id {id} nao encontrado.");

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
