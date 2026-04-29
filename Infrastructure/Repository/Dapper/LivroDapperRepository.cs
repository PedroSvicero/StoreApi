using Core.DTO;
using Core.Repository;
using Dapper;

namespace Infrastructure.Repository.Dapper
{
    // Repositorio de leitura de livros com Dapper.
    // Ele executa SQL manualmente e monta os DTOs sem usar tracking do EF Core.
    public class LivroDapperRepository : ILivroDapperRepository
    {
        private readonly DapperContext _dapperContext;

        public LivroDapperRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public IList<LivroDTO> ObterTodos()
        {
            const string sql = """
                SELECT Id, DataCriacao, Editora, Nome
                FROM Livros;

                SELECT Id, DataCriacao, ClienteId, LivroId
                FROM Pedidos;
                """;

            using var connection = _dapperContext.CreateConnection();
            using var multi = connection.QueryMultiple(sql);

            var livros = multi.Read<LivroDTO>().ToList();
            var pedidos = multi.Read<PedidoDto>().ToList();

            var pedidosPorLivro = pedidos
                .GroupBy(pedido => pedido.LivroId)
                .ToDictionary(grupo => grupo.Key, grupo => (ICollection<PedidoDto>)grupo.ToList());

            foreach (var livro in livros)
            {
                livro.Pedidos = pedidosPorLivro.TryGetValue(livro.Id, out var pedidosDoLivro)
                    ? pedidosDoLivro
                    : [];
            }

            return livros;
        }

        public LivroDTO? ObterPorId(int id)
        {
            const string sql = """
                SELECT Id, DataCriacao, Editora, Nome
                FROM Livros
                WHERE Id = @Id;

                SELECT Id, DataCriacao, ClienteId, LivroId
                FROM Pedidos
                WHERE LivroId = @Id;
                """;

            using var connection = _dapperContext.CreateConnection();
            using var multi = connection.QueryMultiple(sql, new { Id = id });

            var livro = multi.ReadFirstOrDefault<LivroDTO>();
            if (livro is null)
            {
                return null;
            }

            livro.Pedidos = multi.Read<PedidoDto>().ToList();
            return livro;
        }
    }
}
