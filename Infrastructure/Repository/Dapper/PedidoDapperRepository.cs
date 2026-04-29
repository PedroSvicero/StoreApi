using Core.DTO;
using Core.Repository;
using Dapper;

namespace Infrastructure.Repository.Dapper
{
    // Repositorio de leitura de pedidos com Dapper.
    // Aqui a consulta e composta manualmente e os relacionamentos sao montados em memoria.
    public class PedidoDapperRepository : IPedidoDapperRepository
    {
        private readonly DapperContext _dapperContext;

        public PedidoDapperRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public IList<PedidoDto> ObterTodos()
        {
            const string sql = """
                SELECT Id, DataCriacao, ClienteId, LivroId
                FROM Pedidos;

                SELECT Id, DataCriacao, Nome, DataNascimento, CPF
                FROM Clientes;

                SELECT Id, DataCriacao, Nome, Editora
                FROM Livros;
                """;

            using var connection = _dapperContext.CreateConnection();
            using var multi = connection.QueryMultiple(sql);

            var pedidos = multi.Read<PedidoDto>().ToList();
            var clientes = multi.Read<ClienteDto>().ToDictionary(cliente => cliente.Id);
            var livros = multi.Read<LivroDTO>().ToDictionary(livro => livro.Id);

            foreach (var pedido in pedidos)
            {
                pedido.Cliente = clientes.GetValueOrDefault(pedido.ClienteId);
                pedido.Livro = livros.GetValueOrDefault(pedido.LivroId);
            }

            return pedidos;
        }

        public PedidoDto? ObterPorId(int id)
        {
            const string sql = """
                SELECT Id, DataCriacao, ClienteId, LivroId
                FROM Pedidos
                WHERE Id = @Id;

                SELECT c.Id, c.DataCriacao, c.Nome, c.DataNascimento, c.CPF
                FROM Clientes c
                INNER JOIN Pedidos p ON p.ClienteId = c.Id
                WHERE p.Id = @Id;

                SELECT l.Id, l.DataCriacao, l.Nome, l.Editora
                FROM Livros l
                INNER JOIN Pedidos p ON p.LivroId = l.Id
                WHERE p.Id = @Id;
                """;

            using var connection = _dapperContext.CreateConnection();
            using var multi = connection.QueryMultiple(sql, new { Id = id });

            var pedido = multi.ReadFirstOrDefault<PedidoDto>();
            if (pedido is null)
            {
                return null;
            }

            pedido.Cliente = multi.ReadFirstOrDefault<ClienteDto>();
            pedido.Livro = multi.ReadFirstOrDefault<LivroDTO>();

            return pedido;
        }
    }
}
