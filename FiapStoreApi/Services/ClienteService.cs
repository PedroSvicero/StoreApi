using Core.DTO;
using Core.Entity;
using Core.Input;
using Core.Repository;
using Core.Service;

namespace FiapStoreApi.Services
{
    // Service de clientes.
    // Ele concentra regra de negocio e mapeamento, deixando o controller focado em HTTP.
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public IList<ClienteDto> ObterTodos()
        {
            var clientes = _clienteRepository.ObterTodos();
            return clientes.Select(MapearParaDto).ToList();
        }

        public ClienteDto? ObterPorId(int id)
        {
            var cliente = _clienteRepository.ObterPorId(id);
            return cliente is null ? null : MapearParaDto(cliente);
        }

        public ClienteDto? ObterComPedidosSeisMeses(int id)
        {
            var cliente = _clienteRepository.ObterPorId(id);
            if (cliente is null)
            {
                return null;
            }

            // Exemplo de regra temporal do dominio.
            var seisMesesAtras = DateTime.Now.AddMonths(-6);

            return new ClienteDto
            {
                Id = cliente.Id,
                DataCriacao = cliente.DataCriacao,
                Nome = cliente.Nome,
                CPF = cliente.CPF,
                DataNascimento = cliente.DataNascimento,
                // A filtragem temporal fica aqui porque e uma regra da aplicacao, nao do controller.
                Pedidos = cliente.Pedidos
                    .Where(p => p.DataCriacao >= seisMesesAtras)
                    .Select(p => new PedidoDto
                    {
                        Id = p.Id,
                        DataCriacao = p.DataCriacao,
                        ClienteId = p.ClienteId,
                        LivroId = p.LivroId,
                        Livro = p.Livro is null
                            ? null
                            : new LivroDTO
                            {
                                Id = p.Livro.Id,
                                Nome = p.Livro.Nome,
                                Editora = p.Livro.Editora,
                                DataCriacao = p.Livro.DataCriacao
                            }
                    })
                    .ToList()
            };
        }

        public ClienteDto Cadastrar(ClienteInput input)
        {
            // Converte o DTO recebido pela API em entidade de dominio.
            var cliente = new Cliente
            {
                Nome = input.Nome,
                CPF = input.CPF,
                DataNascimento = input.DataNascimento
            };

            _clienteRepository.Cadastrar(cliente);

            return MapearParaDto(cliente);
        }

        public bool Alterar(int id, ClienteUpdateInput input)
        {
            var cliente = _clienteRepository.ObterPorId(id);
            if (cliente is null)
            {
                return false;
            }

            cliente.Nome = input.Nome;
            cliente.DataNascimento = input.DataNascimento;

            _clienteRepository.Alterar(cliente);

            return true;
        }

        public bool Deletar(int id)
        {
            var cliente = _clienteRepository.ObterPorId(id);
            if (cliente is null)
            {
                return false;
            }

            _clienteRepository.Deletar(id);

            return true;
        }

        private static ClienteDto MapearParaDto(Cliente cliente)
        {
            // Centralizar esse mapeamento evita repeticao e mantem a resposta consistente.
            return new ClienteDto
            {
                Id = cliente.Id,
                DataCriacao = cliente.DataCriacao,
                Nome = cliente.Nome,
                CPF = cliente.CPF,
                DataNascimento = cliente.DataNascimento
            };
        }
    }
}
