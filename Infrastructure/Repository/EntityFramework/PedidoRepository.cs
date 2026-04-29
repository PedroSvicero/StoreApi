using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository.EntityFramework
{
    // Repositorio de Pedido.
    // Hoje ele so reutiliza o comportamento generico, mas continua separado para crescer sem afetar outros repositorios.
    public class PedidoRepository : EFRepository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
