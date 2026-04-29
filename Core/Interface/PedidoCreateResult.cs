using Core.DTO;

namespace Core.Service
{
    // Resultado de criacao de pedido: evita retornar apenas bool e perder contexto do erro.
    public sealed record PedidoCreateResult(PedidoDto? Pedido, string? ErrorMessage)
    {
        public bool Success => Pedido is not null;

        public static PedidoCreateResult Criado(PedidoDto pedido) => new(pedido, null);

        public static PedidoCreateResult NaoEncontrado(string errorMessage) => new(null, errorMessage);
    }
}
