using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository.EntityFramework
{
    // Repositorio de Cliente.
    // Mesmo sem metodos extras hoje, ele isola o ponto onde consultas especificas de cliente podem nascer.
    public class ClienteRepository : EFRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
