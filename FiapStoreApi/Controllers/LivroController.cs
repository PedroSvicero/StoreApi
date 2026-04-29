using Core.DTO;
using Core.Input;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapStoreApi.Controllers
{
    /// <summary>
    /// Endpoints do catalogo de livros.
    /// </summary>
    /// <remarks>
    /// O controller fala apenas com a camada de service.
    /// Internamente, o service usa Dapper nas leituras e EF Core nas escritas.
    /// </remarks>
    [ApiController]
    [Route("/[controller]")]
    [Authorize]
    public class LivroController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public LivroController(ILivroService livroService)
        {
            _livroService = livroService;
        }

        /// <summary>
        /// Lista todos os livros cadastrados.
        /// </summary>
        /// <remarks>
        /// Esta leitura passa pelo Dapper e devolve tambem os pedidos relacionados quando existirem.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IList<LivroDTO>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            try
            {
                return Ok(_livroService.ObterTodos());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Busca um livro especifico pelo identificador.
        /// </summary>
        /// <response code="200">Livro encontrado.</response>
        /// <response code="404">Livro nao localizado.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LivroDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                var livro = _livroService.ObterPorId(id);
                if (livro is null)
                    return NotFound($"Livro com Id {id} nao encontrado.");

                return Ok(livro);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Cadastra um novo livro.
        /// </summary>
        /// <remarks>
        /// Exemplo de body:
        ///
        ///     POST /Livro
        ///     {
        ///       "nome": "Clean Code",
        ///       "editora": "Prentice Hall"
        ///     }
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(LivroDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Post([FromBody] LivroInput input)
        {
            try
            {
                var livro = _livroService.Cadastrar(input);
                return CreatedAtAction(nameof(Get), new { id = livro.Id }, livro);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Atualiza um livro existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put([FromRoute] int id, [FromBody] LivroUpdateInput input)
        {
            try
            {
                var atualizado = _livroService.Alterar(id, input);
                if (!atualizado)
                    return NotFound($"Livro com Id {id} nao encontrado.");
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Remove um livro pelo identificador.
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
                var deletado = _livroService.Deletar(id);
                if (!deletado)
                    return NotFound($"Livro com Id {id} nao encontrado.");
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
