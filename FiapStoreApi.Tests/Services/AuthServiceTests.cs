using Core.Entity;
using Core.Input;
using FiapStoreApi.Services;
using FiapStoreApi.Tests.TestDoubles;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FiapStoreApi.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public void Login_DeveRetornarNull_QuandoUsuarioNaoExiste()
    {
        var usuarioRepository = new UsuarioRepositoryFake();
        var service = new AuthService(usuarioRepository, CriarConfiguracaoJwt());

        var resultado = service.Login(new LoginInput
        {
            Email = "inexistente@fiap.com",
            Senha = "123456"
        });

        Assert.Null(resultado);
    }

    [Fact]
    public void Login_DeveGerarToken_QuandoCredenciaisForemValidas()
    {
        var usuarioRepository = new UsuarioRepositoryFake
        {
            OnObterPorEmail = _ => new Usuario
            {
                Id = 7,
                Email = "admin@fiap.com",
                Role = "Admin",
                SenhaHash = GerarHash("123456"),
                DataCriacao = new DateTime(2026, 4, 1)
            }
        };

        var service = new AuthService(usuarioRepository, CriarConfiguracaoJwt());

        var token = service.Login(new LoginInput
        {
            Email = "admin@fiap.com",
            Senha = "123456"
        });

        Assert.NotNull(token);

        // Em teste de unidade, ler o token pronto e conferir suas claims
        // e uma forma objetiva de validar a regra sem subir middleware nem API.
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Equal("https://fiapstore.test", jwt.Issuer);
        Assert.Contains(jwt.Claims, claim => claim.Type == ClaimTypes.Email && claim.Value == "admin@fiap.com");
        Assert.Contains(jwt.Claims, claim => claim.Type == ClaimTypes.Role && claim.Value == "Admin");
        Assert.Contains(jwt.Claims, claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == "7");
    }

    [Fact]
    public void Register_DeveLancarExcecao_QuandoEmailJaExistir()
    {
        var usuarioRepository = new UsuarioRepositoryFake
        {
            OnObterPorEmail = _ => new Usuario
            {
                Id = 1,
                Email = "duplicado@fiap.com",
                Role = "User",
                SenhaHash = "HASH",
                DataCriacao = new DateTime(2026, 4, 1)
            }
        };

        var service = new AuthService(usuarioRepository, CriarConfiguracaoJwt());

        var excecao = Assert.Throws<InvalidOperationException>(() => service.Register(new RegisterInput
        {
            Email = "duplicado@fiap.com",
            Senha = "123456",
            Role = "User"
        }));

        Assert.Contains("já está em uso", excecao.Message);
    }

    [Fact]
    public void Register_DeveSalvarUsuarioComSenhaHasheadaERetornarDto()
    {
        Usuario? usuarioSalvo = null;

        var usuarioRepository = new UsuarioRepositoryFake
        {
            OnObterPorEmail = _ => null,
            OnCadastrar = usuario =>
            {
                usuario.Id = 10;
                usuario.DataCriacao = new DateTime(2026, 4, 2);
                usuarioSalvo = usuario;
            }
        };

        var service = new AuthService(usuarioRepository, CriarConfiguracaoJwt());

        var resultado = service.Register(new RegisterInput
        {
            Email = "novo@fiap.com",
            Senha = "123456",
            Role = "User"
        });

        Assert.NotNull(usuarioSalvo);
        Assert.Equal(1, usuarioRepository.CadastrarChamadas);
        Assert.Equal("novo@fiap.com", usuarioSalvo!.Email);
        Assert.Equal("User", usuarioSalvo.Role);
        Assert.NotEqual("123456", usuarioSalvo.SenhaHash);
        Assert.Equal(GerarHash("123456"), usuarioSalvo.SenhaHash);
        Assert.Equal(10, resultado.Id);
        Assert.Equal("novo@fiap.com", resultado.Email);
        Assert.Equal("User", resultado.Role);
    }

    private static IConfiguration CriarConfiguracaoJwt()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "12345678901234567890123456789012",
                ["Jwt:Issuer"] = "https://fiapstore.test",
                ["Jwt:Audience"] = "https://fiapstore.test/client"
            })
            .Build();
    }

    private static string GerarHash(string senha)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
        return Convert.ToHexString(bytes);
    }
}
