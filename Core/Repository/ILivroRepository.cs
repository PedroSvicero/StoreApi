using Core.Entity;

namespace Core.Repository
{
    // Repositorio de Livro com uma operacao adicional de insercao em lote.
    public interface ILivroRepository : IRepository<Livro>
    {
        void CadastrarEmMassa(IEnumerable<Livro> livros);
    }
}
