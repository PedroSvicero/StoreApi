using Core.DTO;

namespace Core.Repository
{
    // Contrato de leitura de pedidos usando Dapper.
    // Ele devolve DTOs prontos porque o foco aqui e leitura performatica e objetiva.
    public interface IPedidoDapperRepository
    {
        IList<PedidoDto> ObterTodos();
        PedidoDto? ObterPorId(int id);
    }
}
