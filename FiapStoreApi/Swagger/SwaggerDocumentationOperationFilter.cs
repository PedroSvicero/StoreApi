using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FiapStoreApi.Swagger
{
    // OperationFilter permite enriquecer cada endpoint individualmente.
    // Aqui adicionamos informacoes didaticas sobre autenticacao e respostas padrao.
    public class SwaggerDocumentationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses ??= [];
            operation.Responses.TryAdd("500", new OpenApiResponse
            {
                Description = "Erro interno nao tratado pela API."
            });

            var hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
            var controllerAuthorizeAttributes = context.MethodInfo.DeclaringType is null
                ? Enumerable.Empty<AuthorizeAttribute>()
                : context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>();

            var authorizeAttributes = context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>()
                .Concat(controllerAuthorizeAttributes)
                .ToList();

            if (hasAllowAnonymous || authorizeAttributes.Count == 0)
            {
                operation.Description = AppendBlock(operation.Description, "Acesso", "Endpoint publico. Nao exige JWT.");
                return;
            }

            var roles = authorizeAttributes
                .Where(attribute => !string.IsNullOrWhiteSpace(attribute.Roles))
                .SelectMany(attribute => attribute.Roles!.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            operation.Responses.TryAdd("401", new OpenApiResponse
            {
                Description = "Token ausente, invalido ou expirado."
            });

            operation.Responses.TryAdd("403", new OpenApiResponse
            {
                Description = "Usuario autenticado, mas sem permissao suficiente para executar a acao."
            });

            operation.Security ??= [];
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", null, null)] = []
            });

            var accessText = roles.Count == 0
                ? "Endpoint protegido. Qualquer usuario autenticado pode acessar."
                : $"Endpoint protegido. Perfis permitidos: {string.Join(", ", roles)}.";

            operation.Description = AppendBlock(operation.Description, "Acesso", accessText);
        }

        private static string AppendBlock(string? currentDescription, string title, string content)
        {
            if (string.IsNullOrWhiteSpace(currentDescription))
            {
                return $"**{title}:** {content}";
            }

            return $"{currentDescription}{Environment.NewLine}{Environment.NewLine}**{title}:** {content}";
        }
    }
}
