using Core.DTO;
using Core.Entity;
using Core.Input;
using Core.Repository;
using Core.Service;

namespace FiapStoreApi.Services
{
    // Service de livros.
    // Ele concentra toda a regra de negocio de livros.
    // A organizacao melhora quando o controller fala com service, e nao direto com repositorio.
    public class LivroService : ILivroService
    {
        private readonly ILivroDapperRepository _livroDapperRepository;
        private readonly ILivroRepository _livroRepository;

        public LivroService(ILivroDapperRepository livroDapperRepository, ILivroRepository livroRepository)
        {
            _livroDapperRepository = livroDapperRepository;
            _livroRepository = livroRepository;
        }

        public IList<LivroDTO> ObterTodos()
        {
            // Leitura via Dapper: consulta enxuta e foco em performance/clareza do SQL.
            return _livroDapperRepository.ObterTodos();
        }

        public LivroDTO? ObterPorId(int id)
        {
            return _livroDapperRepository.ObterPorId(id);
        }

        public LivroDTO Cadastrar(LivroInput input)
        {
            // Converte o DTO recebido em entidade persistivel.
            var livro = new Livro
            {
                Nome = input.Nome,
                Editora = input.Editora
            };

            _livroRepository.Cadastrar(livro);

            return MapearParaDto(livro);
        }

        public bool Alterar(int id, LivroUpdateInput input)
        {
            var livro = _livroRepository.ObterPorId(id);
            if (livro is null)
            {
                return false;
            }

            livro.Nome = input.Nome;
            livro.Editora = input.Editora;

            _livroRepository.Alterar(livro);

            return true;
        }

        public bool Deletar(int id)
        {
            var livro = _livroRepository.ObterPorId(id);
            if (livro is null)
            {
                return false;
            }

            _livroRepository.Deletar(id);

            return true;
        }

        private static LivroDTO MapearParaDto(Livro livro)
        {
            // DTO evita expor diretamente a entidade do EF Core na resposta HTTP.
            return new LivroDTO
            {
                Id = livro.Id,
                DataCriacao = livro.DataCriacao,
                Nome = livro.Nome,
                Editora = livro.Editora,
                Pedidos = livro.Pedidos
                    .Select(p => new PedidoDto
                    {
                        Id = p.Id,
                        DataCriacao = p.DataCriacao,
                        ClienteId = p.ClienteId,
                        LivroId = p.LivroId
                    })
                    .ToList()
            };
        }
    }
}
