using Core.DTO;
using Core.Entity;
using Core.Input;
using Core.Repository;
using Core.Service;

namespace FiapStoreApi.Services
{
    // Service de pedidos.
    // Seu papel e validar relacionamentos e montar respostas coerentes.
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoDapperRepository _pedidoDapperRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ILivroRepository _livroRepository;

        public PedidoService(
            IPedidoDapperRepository pedidoDapperRepository,
            IPedidoRepository pedidoRepository,
            IClienteRepository clienteRepository,
            ILivroRepository livroRepository)
        {
            _pedidoDapperRepository = pedidoDapperRepository;
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
            _livroRepository = livroRepository;
        }

        public IList<PedidoDto> ObterTodos()
        {
            // Leitura via Dapper para demonstrar joins e projecoes manuais.
            return _pedidoDapperRepository.ObterTodos();
        }

        public PedidoDto? ObterPorId(int id)
        {
            return _pedidoDapperRepository.ObterPorId(id);
        }

        public PedidoCreateResult Cadastrar(PedidoInput input)
        {
            // Antes de salvar, validamos as referencias para evitar relacionamentos invalidos.
            var cliente = _clienteRepository.ObterPorId(input.ClienteId);
            if (cliente is null)
            {
                return PedidoCreateResult.NaoEncontrado($"Cliente com Id {input.ClienteId} nao encontrado.");
            }

            var livro = _livroRepository.ObterPorId(input.LivroId);
            if (livro is null)
            {
                return PedidoCreateResult.NaoEncontrado($"Livro com Id {input.LivroId} nao encontrado.");
            }

            var pedido = new Pedido
            {
                ClienteId = input.ClienteId,
                LivroId = input.LivroId,
                // Preencher navegacoes aqui ajuda a devolver um DTO completo logo apos a criacao.
                Cliente = cliente,
                Livro = livro
            };

            _pedidoRepository.Cadastrar(pedido);

            return PedidoCreateResult.Criado(MapearParaDto(pedido));
        }

        public bool Deletar(int id)
        {
            var pedido = _pedidoRepository.ObterPorId(id);
            if (pedido is null)
            {
                return false;
            }

            _pedidoRepository.Deletar(id);

            return true;
        }

        // Conversao centralizada da entidade Pedido para o formato publico da API.
        private static PedidoDto MapearParaDto(Pedido pedido)
        {
            return new PedidoDto
            {
                Id = pedido.Id,
                DataCriacao = pedido.DataCriacao,
                ClienteId = pedido.ClienteId,
                LivroId = pedido.LivroId,
                Cliente = pedido.Cliente is null
                    ? null
                    : new ClienteDto
                    {
                        Id = pedido.Cliente.Id,
                        Nome = pedido.Cliente.Nome,
                        CPF = pedido.Cliente.CPF,
                        DataNascimento = pedido.Cliente.DataNascimento,
                        DataCriacao = pedido.Cliente.DataCriacao
                    },
                Livro = pedido.Livro is null
                    ? null
                    : new LivroDTO
                    {
                        Id = pedido.Livro.Id,
                        Nome = pedido.Livro.Nome,
                        Editora = pedido.Livro.Editora,
                        DataCriacao = pedido.Livro.DataCriacao
                    }
            };
        }
    }
}
