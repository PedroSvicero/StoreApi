using Core.Entity;
using Core.Input;
using FiapStoreApi.Services;
using FiapStoreApi.Tests.TestDoubles;

namespace FiapStoreApi.Tests.Services;

public class ClienteServiceTests
{
    [Fact]
    public void ObterComPedidosSeisMeses_DeveFiltrarPedidosMaisAntigos()
    {
        var agora = DateTime.Now;
        var clienteRepository = new ClienteRepositoryFake
        {
            OnObterPorId = _ => new Cliente
            {
                Id = 1,
                Nome = "Pedro",
                CPF = "12345678901",
                DataNascimento = new DateTime(1990, 1, 1),
                DataCriacao = new DateTime(2026, 1, 1),
                Pedidos =
                [
                    new Pedido
                    {
                        Id = 100,
                        ClienteId = 1,
                        LivroId = 10,
                        DataCriacao = agora.AddMonths(-5),
                        Livro = new Livro
                        {
                            Id = 10,
                            Nome = "Clean Code",
                            Editora = "Prentice Hall",
                            DataCriacao = new DateTime(2025, 1, 1)
                        }
                    },
                    new Pedido
                    {
                        Id = 200,
                        ClienteId = 1,
                        LivroId = 20,
                        DataCriacao = agora.AddMonths(-7),
                        Livro = new Livro
                        {
                            Id = 20,
                            Nome = "Refactoring",
                            Editora = "Addison-Wesley",
                            DataCriacao = new DateTime(2025, 2, 1)
                        }
                    }
                ]
            }
        };

        var service = new ClienteService(clienteRepository);

        var resultado = service.ObterComPedidosSeisMeses(1);

        Assert.NotNull(resultado);
        Assert.Single(resultado!.Pedidos);
        Assert.Equal(100, resultado.Pedidos.Single().Id);
        Assert.Equal("Clean Code", resultado.Pedidos.Single().Livro!.Nome);
    }

    [Fact]
    public void Alterar_DeveRetornarFalse_QuandoClienteNaoExistir()
    {
        var clienteRepository = new ClienteRepositoryFake
        {
            OnObterPorId = _ => null
        };

        var service = new ClienteService(clienteRepository);

        var alterou = service.Alterar(99, new ClienteUpdateInput
        {
            Nome = "Novo Nome",
            DataNascimento = new DateTime(1991, 1, 1)
        });

        Assert.False(alterou);
        Assert.Equal(0, clienteRepository.AlterarChamadas);
    }

    [Fact]
    public void Cadastrar_DeveMapearInputParaEntidadeEDto()
    {
        Cliente? clienteSalvo = null;

        var clienteRepository = new ClienteRepositoryFake
        {
            OnCadastrar = cliente =>
            {
                cliente.Id = 5;
                cliente.DataCriacao = new DateTime(2026, 4, 2);
                clienteSalvo = cliente;
            }
        };

        var service = new ClienteService(clienteRepository);

        var resultado = service.Cadastrar(new ClienteInput
        {
            Nome = "Maria",
            CPF = "10987654321",
            DataNascimento = new DateTime(1995, 5, 20)
        });

        Assert.NotNull(clienteSalvo);
        Assert.Equal(1, clienteRepository.CadastrarChamadas);
        Assert.Equal("Maria", clienteSalvo!.Nome);
        Assert.Equal("10987654321", clienteSalvo.CPF);
        Assert.Equal(5, resultado.Id);
        Assert.Equal("Maria", resultado.Nome);
        Assert.Equal("10987654321", resultado.CPF);
    }
}
