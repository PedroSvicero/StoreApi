using Core.DTO;

namespace Core.Repository
{
    // Contrato de leitura de livros usando Dapper.
    // A ideia aqui e demonstrar um modelo hibrido: Dapper para consulta e EF Core para escrita.
    public interface ILivroDapperRepository
    {
        IList<LivroDTO> ObterTodos();
        LivroDTO? ObterPorId(int id);
    }
}
