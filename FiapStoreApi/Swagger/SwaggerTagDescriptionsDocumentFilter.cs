using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FiapStoreApi.Swagger
{
    // DocumentFilter atua no documento inteiro.
    // Aqui definimos descricoes amigaveis para as tags exibidas no Swagger.
    public class SwaggerTagDescriptionsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new HashSet<OpenApiTag>
            {
                new OpenApiTag
                {
                    Name = "Auth",
                    Description = "Fluxo de autenticacao e registro de usuarios da API."
                },
                new OpenApiTag
                {
                    Name = "Cliente",
                    Description = "Operacoes de consulta e administracao de clientes."
                },
                new OpenApiTag
                {
                    Name = "Livro",
                    Description = "Operacoes de leitura e manutencao do catalogo de livros."
                },
                new OpenApiTag
                {
                    Name = "Pedido",
                    Description = "Endpoints que ligam cliente e livro por meio de pedidos."
                },
                new OpenApiTag
                {
                    Name = "Log",
                    Description = "Endpoint auxiliar para demonstrar a geracao de logs."
                }
            };
        }
    }
}
