using Core.DTO;
using Core.Entity;
using Core.Repository;

namespace FiapStoreApi.Tests.TestDoubles;

// Dublês simples de repositório para testes de unidade.
// Eles substituem banco, EF Core e Dapper para que o teste foque só na regra do service.

internal sealed class ClienteRepositoryFake : IClienteRepository
{
    public Func<int, Cliente?> OnObterPorId { get; set; } = _ => null;
    public Func<IList<Cliente>> OnObterTodos { get; set; } = () => [];
    public Action<Cliente>? OnCadastrar { get; set; }
    public Action<Cliente>? OnAlterar { get; set; }
    public Action<int>? OnDeletar { get; set; }

    public int CadastrarChamadas { get; private set; }
    public int AlterarChamadas { get; private set; }
    public int DeletarChamadas { get; private set; }

    public void Alterar(Cliente entidade)
    {
        AlterarChamadas++;
        OnAlterar?.Invoke(entidade);
    }

    public void Cadastrar(Cliente entidade)
    {
        CadastrarChamadas++;
        OnCadastrar?.Invoke(entidade);
    }

    public void Deletar(int id)
    {
        DeletarChamadas++;
        OnDeletar?.Invoke(id);
    }

    public Cliente? ObterPorId(int id) => OnObterPorId(id);

    public IList<Cliente> ObterTodos() => OnObterTodos();
}

internal sealed class LivroRepositoryFake : ILivroRepository
{
    public Func<int, Livro?> OnObterPorId { get; set; } = _ => null;
    public Func<IList<Livro>> OnObterTodos { get; set; } = () => [];
    public Action<Livro>? OnCadastrar { get; set; }
    public Action<Livro>? OnAlterar { get; set; }
    public Action<int>? OnDeletar { get; set; }
    public Action<IEnumerable<Livro>>? OnCadastrarEmMassa { get; set; }

    public int CadastrarChamadas { get; private set; }
    public int AlterarChamadas { get; private set; }
    public int DeletarChamadas { get; private set; }

    public void Alterar(Livro entidade)
    {
        AlterarChamadas++;
        OnAlterar?.Invoke(entidade);
    }

    public void Cadastrar(Livro entidade)
    {
        CadastrarChamadas++;
        OnCadastrar?.Invoke(entidade);
    }

    public void CadastrarEmMassa(IEnumerable<Livro> livros)
    {
        OnCadastrarEmMassa?.Invoke(livros);
    }

    public void Deletar(int id)
    {
        DeletarChamadas++;
        OnDeletar?.Invoke(id);
    }

    public Livro? ObterPorId(int id) => OnObterPorId(id);

    public IList<Livro> ObterTodos() => OnObterTodos();
}

internal sealed class PedidoRepositoryFake : IPedidoRepository
{
    public Func<int, Pedido?> OnObterPorId { get; set; } = _ => null;
    public Func<IList<Pedido>> OnObterTodos { get; set; } = () => [];
    public Action<Pedido>? OnCadastrar { get; set; }
    public Action<Pedido>? OnAlterar { get; set; }
    public Action<int>? OnDeletar { get; set; }

    public int CadastrarChamadas { get; private set; }
    public int DeletarChamadas { get; private set; }

    public void Alterar(Pedido entidade)
    {
        OnAlterar?.Invoke(entidade);
    }

    public void Cadastrar(Pedido entidade)
    {
        CadastrarChamadas++;
        OnCadastrar?.Invoke(entidade);
    }

    public void Deletar(int id)
    {
        DeletarChamadas++;
        OnDeletar?.Invoke(id);
    }

    public Pedido? ObterPorId(int id) => OnObterPorId(id);

    public IList<Pedido> ObterTodos() => OnObterTodos();
}

internal sealed class UsuarioRepositoryFake : IUsuarioRepository
{
    public Func<string, Usuario?> OnObterPorEmail { get; set; } = _ => null;
    public Action<Usuario>? OnCadastrar { get; set; }

    public int CadastrarChamadas { get; private set; }

    public void Alterar(Usuario entidade)
    {
    }

    public void Cadastrar(Usuario entidade)
    {
        CadastrarChamadas++;
        OnCadastrar?.Invoke(entidade);
    }

    public void Deletar(int id)
    {
    }

    public Usuario? ObterPorEmail(string email) => OnObterPorEmail(email);

    public Usuario? ObterPorId(int id) => null;

    public IList<Usuario> ObterTodos() => [];
}

internal sealed class LivroDapperRepositoryFake : ILivroDapperRepository
{
    public Func<IList<LivroDTO>> OnObterTodos { get; set; } = () => [];
    public Func<int, LivroDTO?> OnObterPorId { get; set; } = _ => null;

    public int ObterTodosChamadas { get; private set; }
    public int ObterPorIdChamadas { get; private set; }

    public LivroDTO? ObterPorId(int id)
    {
        ObterPorIdChamadas++;
        return OnObterPorId(id);
    }

    public IList<LivroDTO> ObterTodos()
    {
        ObterTodosChamadas++;
        return OnObterTodos();
    }
}

internal sealed class PedidoDapperRepositoryFake : IPedidoDapperRepository
{
    public Func<IList<PedidoDto>> OnObterTodos { get; set; } = () => [];
    public Func<int, PedidoDto?> OnObterPorId { get; set; } = _ => null;

    public PedidoDto? ObterPorId(int id) => OnObterPorId(id);

    public IList<PedidoDto> ObterTodos() => OnObterTodos();
}
