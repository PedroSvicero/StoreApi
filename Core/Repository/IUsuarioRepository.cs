using Core.Entity;

namespace Core.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        /// <summary>Busca um usuário pelo email — usado no login.</summary>
        Usuario? ObterPorEmail(string email);
    }
}