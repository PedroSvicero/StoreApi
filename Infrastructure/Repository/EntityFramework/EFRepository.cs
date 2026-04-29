using Core.Entity;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.EntityFramework
{
    // Implementacao generica das operacoes CRUD usando EF Core.
    // Ela evita repetir a mesma infraestrutura basica em cada repositorio concreto.
    public class EFRepository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EFRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Alterar(T entidade)
        {
            // Update marca a entidade como modificada e SaveChanges persiste a alteracao.
            _dbSet.Update(entidade);
            _context.SaveChanges();
        }

        public void Cadastrar(T entidade)
        {
            // Garante uma data padrao de criacao antes do insert.
            entidade.DataCriacao = DateTime.Now;
            _dbSet.Add(entidade);
            _context.SaveChanges();
        }

        public void Deletar(int id)
        {
            var entidade = ObterPorId(id);
            if (entidade is null)
            {
                // Em vez de lancar excecao, o repositorio simplesmente ignora a remocao inexistente.
                return;
            }

            _dbSet.Remove(entidade);
            _context.SaveChanges();
        }

        public T? ObterPorId(int id)
        {
            // FirstOrDefault retorna null quando nao encontra registro, o que simplifica o tratamento acima.
            return _dbSet.FirstOrDefault(entity => entity.Id == id);
        }

        public IList<T> ObterTodos()
        {
            // Materializa a consulta em memoria para entregar uma lista pronta ao chamador.
            return _dbSet.ToList();
        }
    }
}
