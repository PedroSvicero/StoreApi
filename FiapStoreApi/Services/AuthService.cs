using Core.DTO;
using Core.Entity;
using Core.Input;
using Core.Repository;
using Core.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FiapStoreApi.Services
{
    // Service de autenticacao.
    // Centraliza cadastro, validacao de credenciais e emissao de JWT.
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public string? Login(LoginInput input)
        {
            // Primeiro localizamos o usuario pelo email informado no login.
            var usuario = _usuarioRepository.ObterPorEmail(input.Email);

            // Usuário não existe ou senha incorreta — mesmo erro para não revelar
            // qual dos dois falhou (segurança contra enumeração de usuários)
            if (usuario is null || !VerificarSenha(input.Senha, usuario.SenhaHash))
                return null;

            return GerarToken(usuario);
        }

        public UsuarioDto Register(RegisterInput input)
        {
            // Regra de negocio: o email precisa ser unico.
            var existente = _usuarioRepository.ObterPorEmail(input.Email);
            if (existente is not null)
                throw new InvalidOperationException($"Email '{input.Email}' já está em uso.");

            // A entidade salva o hash, nunca a senha em texto puro.
            var usuario = new Usuario
            {
                Email = input.Email,
                SenhaHash = HashSenha(input.Senha),
                Role = input.Role
            };

            _usuarioRepository.Cadastrar(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Role = usuario.Role
            };
        }

        // -----------------------------------------------------------------------
        // Geração do token JWT
        // -----------------------------------------------------------------------
        // Gera o token que o cliente vai enviar nas proximas requisicoes.
        private string GerarToken(Usuario usuario)
        {
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key não configurada no appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims são as informações embutidas no token
            // O claim de Role é o que o [Authorize(Roles = "Admin")] vai ler
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // -----------------------------------------------------------------------
        // Hash de senha com SHA256
        // Em produção, prefira BCrypt.Net-Next (NuGet) que é mais seguro.
        // BCrypt.Net.BCrypt.HashPassword(senha) / BCrypt.Net.BCrypt.Verify(senha, hash)
        // -----------------------------------------------------------------------
        private static string HashSenha(string senha)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
            return Convert.ToHexString(bytes);
        }

        // Recalcula o hash da senha recebida e compara com o valor salvo no banco.
        private static bool VerificarSenha(string senha, string hash)
        {
            var hashTentativa = HashSenha(senha);
            return hashTentativa.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
