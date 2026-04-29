using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository.EntityFramework
{
    // Implementacao concreta do repositorio de Livro.
    // Herdar do generico permite acrescentar apenas o que e exclusivo de livros.
    public class LivroRepository : EFRepository<Livro>, ILivroRepository
    {
        public LivroRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CadastrarEmMassa(IEnumerable<Livro> livros)
        {
            // Insercao em lote: util quando a aplicacao precisar cadastrar varios livros de uma vez.
            // O AddRange reduz repeticao quando comparado a chamar Cadastrar varias vezes.
            _context.Livro.AddRange(livros);
            _context.SaveChanges();
        }
    }
}
