using Core.DTO;
using Core.Input;

namespace Core.Service
{
    // Contrato de negocio para o fluxo de livros.
    public interface ILivroService
    {
        IList<LivroDTO> ObterTodos();
        LivroDTO? ObterPorId(int id);
        LivroDTO Cadastrar(LivroInput input);
        bool Alterar(int id, LivroUpdateInput input);
        bool Deletar(int id);
    }
}
