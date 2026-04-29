using Core.DTO;
using Core.Entity;
using Core.Input;
using FiapStoreApi.Services;
using FiapStoreApi.Tests.TestDoubles;

namespace FiapStoreApi.Tests.Services;

public class LivroServiceTests
{
    [Fact]
    public void ObterTodos_DeveUsarRepositorioDapperNasLeituras()
    {
        var leituraEsperada = new List<LivroDTO>
        {
            new()
            {
                Id = 1,
                Nome = "Arquitetura Limpa",
                Editora = "Alta Books",
                DataCriacao = new DateTime(2026, 4, 1)
            }
        };

        var dapperRepository = new LivroDapperRepositoryFake
        {
            OnObterTodos = () => leituraEsperada
        };

        var efRepository = new LivroRepositoryFake();
        var service = new LivroService(dapperRepository, efRepository);

        var resultado = service.ObterTodos();

        // Esse teste mostra a intencao da arquitetura atual:
        // leitura via Dapper, escrita via EF Core.
        Assert.Same(leituraEsperada, resultado);
        Assert.Equal(1, dapperRepository.ObterTodosChamadas);
    }

    [Fact]
    public void Cadastrar_DevePersistirLivroERetornarDto()
    {
        Livro? livroSalvo = null;

        var dapperRepository = new LivroDapperRepositoryFake();
        var efRepository = new LivroRepositoryFake
        {
            OnCadastrar = livro =>
            {
                livro.Id = 3;
                livro.DataCriacao = new DateTime(2026, 4, 2);
                livroSalvo = livro;
            }
        };

        var service = new LivroService(dapperRepository, efRepository);

        var resultado = service.Cadastrar(new LivroInput
        {
            Nome = "Domain-Driven Design",
            Editora = "Addison-Wesley"
        });

        Assert.NotNull(livroSalvo);
        Assert.Equal(1, efRepository.CadastrarChamadas);
        Assert.Equal("Domain-Driven Design", livroSalvo!.Nome);
        Assert.Equal("Addison-Wesley", livroSalvo.Editora);
        Assert.Equal(3, resultado.Id);
        Assert.Equal("Domain-Driven Design", resultado.Nome);
    }

    [Fact]
    public void Alterar_DeveAtualizarLivroQuandoEleExistir()
    {
        var livro = new Livro
        {
            Id = 9,
            Nome = "Livro Antigo",
            Editora = "Editora Antiga",
            DataCriacao = new DateTime(2026, 1, 1)
        };

        var service = new LivroService(
            new LivroDapperRepositoryFake(),
            new LivroRepositoryFake
            {
                OnObterPorId = _ => livro
            });

        var alterou = service.Alterar(9, new LivroUpdateInput
        {
            Nome = "Livro Novo",
            Editora = "Nova Editora"
        });

        Assert.True(alterou);
        Assert.Equal("Livro Novo", livro.Nome);
        Assert.Equal("Nova Editora", livro.Editora);
    }
}
