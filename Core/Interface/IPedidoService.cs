using Core.DTO;
using Core.Input;

namespace Core.Service
{
    // Contrato de negocio para o fluxo de pedidos.
    public interface IPedidoService
    {
        IList<PedidoDto> ObterTodos();
        PedidoDto? ObterPorId(int id);
        PedidoCreateResult Cadastrar(PedidoInput input);
        bool Deletar(int id);
    }
}
