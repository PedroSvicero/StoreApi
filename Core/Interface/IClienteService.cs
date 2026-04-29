using Core.DTO;
using Core.Input;

namespace Core.Service
{
    /// <summary>
    /// Contrato da camada de servico de Cliente.
    /// Aqui moram as operacoes de negocio vistas pelo controller.
    /// </summary>
    public interface IClienteService
    {
        IList<ClienteDto> ObterTodos();
        ClienteDto? ObterPorId(int id);
        ClienteDto? ObterComPedidosSeisMeses(int id);
        ClienteDto Cadastrar(ClienteInput input);
        bool Alterar(int id, ClienteUpdateInput input);
        bool Deletar(int id);
    }
}
