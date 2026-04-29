using Core.Entity;
using Core.Repository;
using Dapper;
using Infrastructure.Repository.Dapper;

namespace Infrastructure.Repository.EntityFramework
{
    // Repositorio de usuarios da autenticacao.
    // Ele adiciona a busca por email acima do CRUD generico.
    public class UsuarioRepository : EFRepository<Usuario>, IUsuarioRepository
    {
        private readonly DapperContext _dapperContext;

        public UsuarioRepository(ApplicationDbContext context, DapperContext dapperContext) : base(context)
        {
            _dapperContext = dapperContext;
        }

        public Usuario? ObterPorEmail(string email)
        {
            // Consulta especifica usada no login e na validacao de email duplicado.
            // Como a busca e direta por um unico campo, Dapper se encaixa muito bem aqui.
            const string sql = """
                SELECT TOP 1 Id, DataCriacao, Email, SenhaHash, Role
                FROM Usuarios
                WHERE Email = @Email;
                """;

            using var connection = _dapperContext.CreateConnection();
            return connection.QueryFirstOrDefault<Usuario>(sql, new { Email = email });
        }
    }
}
