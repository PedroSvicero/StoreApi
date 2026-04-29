using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository.Dapper
{
    // Contexto simples do Dapper.
    // Diferente do DbContext do EF Core, aqui a responsabilidade e apenas abrir conexoes SQL.
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Cada consulta recebe sua propria conexao.
        // O Dapper trabalha sobre IDbConnection e nao controla rastreamento de entidades.
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
