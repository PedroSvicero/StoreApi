using Core.Entity;
using Core.Input;
using FiapStoreApi.Services;
using FiapStoreApi.Tests.TestDoubles;

namespace FiapStoreApi.Tests.Services;

public class PedidoServiceTests
{
    [Fact]
    public void Cadastrar_DeveRetornarErroQuandoClienteNaoExistir()
    {
        var service = new PedidoService(
            new PedidoDapperRepositoryFake(),
            new PedidoRepositoryFake(),
            new ClienteRepositoryFake
            {
                OnObterPorId = _ => null
            },
            new LivroRepositoryFake());

        var resultado = service.Cadastrar(new PedidoInput
        {
            ClienteId = 99,
            LivroId = 1
        });

        Assert.False(resultado.Success);
        Assert.Null(resultado.Pedido);
        Assert.Contains("Cliente com Id 99", resultado.ErrorMessage);
    }

    [Fact]
    public void Cadastrar_DeveRetornarErroQuandoLivroNaoExistir()
    {
        var service = new PedidoService(
            new PedidoDapperRepositoryFake(),
            new PedidoRepositoryFake(),
            new ClienteRepositoryFake
            {
                OnObterPorId = _ => new Cliente
                {
                    Id = 1,
                    Nome = "Pedro",
                    CPF = "12345678901",
                    DataNascimento = new DateTime(1990, 1, 1),
                    DataCriacao = new DateTime(2026, 1, 1)
                }
            },
            new LivroRepositoryFake
            {
                OnObterPorId = _ => null
            });

        var resultado = service.Cadastrar(new PedidoInput
        {
            ClienteId = 1,
            LivroId = 77
        });

        Assert.False(resultado.Success);
        Assert.Null(resultado.Pedido);
        Assert.Contains("Livro com Id 77", resultado.ErrorMessage);
    }

    [Fact]
    public void Cadastrar_DeveCriarPedidoComDtoCompletoQuandoDependenciasExistirem()
    {
        Pedido? pedidoSalvo = null;

        var cliente = new Cliente
        {
            Id = 1,
            Nome = "Pedro",
            CPF = "12345678901",
            DataNascimento = new DateTime(1990, 1, 1),
            DataCriacao = new DateTime(2026, 1, 1)
        };

        var livro = new Livro
        {
            Id = 10,
            Nome = "ASP.NET Core",
            Editora = "Casa do Codigo",
            DataCriacao = new DateTime(2026, 1, 5)
        };

        var pedidoRepository = new PedidoRepositoryFake
        {
            OnCadastrar = pedido =>
            {
                pedido.Id = 50;
                pedido.DataCriacao = new DateTime(2026, 4, 2);
                pedidoSalvo = pedido;
            }
        };

        var service = new PedidoService(
            new PedidoDapperRepositoryFake(),
            pedidoRepository,
            new ClienteRepositoryFake
            {
                OnObterPorId = _ => cliente
            },
            new LivroRepositoryFake
            {
                OnObterPorId = _ => livro
            });

        var resultado = service.Cadastrar(new PedidoInput
        {
            ClienteId = 1,
            LivroId = 10
        });

        Assert.True(resultado.Success);
        Assert.NotNull(pedidoSalvo);
        Assert.Equal(1, pedidoRepository.CadastrarChamadas);
        Assert.Equal(50, resultado.Pedido!.Id);
        Assert.Equal("Pedro", resultado.Pedido.Cliente!.Nome);
        Assert.Equal("ASP.NET Core", resultado.Pedido.Livro!.Nome);
    }
}
