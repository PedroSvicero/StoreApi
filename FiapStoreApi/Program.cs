using Core.Repository;
using Core.Service;
using FiapStoreApi.Services;
using FiapStoreApi.Swagger;
using Infrastructure.Repository.Dapper;
using Infrastructure.Repository.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

// Program.cs e o ponto de entrada da aplicacao.
// Aqui sao configurados servicos, autenticacao, infraestrutura e o pipeline HTTP.
var builder = WebApplication.CreateBuilder(args);

// Registra os controllers que atenderao as rotas da API.
builder.Services.AddControllers();

// Gera metadados dos endpoints para ferramentas como Swagger.
builder.Services.AddEndpointsApiExplorer();

// Caminhos dos arquivos XML gerados pelo compilador.
// Esses arquivos alimentam o Swagger com textos das classes, metodos e DTOs.
var apiXmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
var coreXmlPath = Path.Combine(AppContext.BaseDirectory, "Core.xml");

// Configura a documentacao interativa da API.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FiapStoreApi | Guia Interativo",
        Version = "v1",
        Description = """
            API didatica para estudo de ASP.NET Core, Entity Framework Core, Dapper e autenticacao JWT.

            ### Como usar esta documentacao
            1. Use `POST /Auth/register` para criar um usuario.
            2. Use `POST /Auth/login` para obter o token JWT.
            3. Clique em `Authorize` e informe `Bearer {seu_token}`.
            4. Teste os endpoints protegidos diretamente pela interface.

            ### O que este projeto demonstra
            - Controllers, services e repositorios
            - EF Core para escrita e Dapper para leituras selecionadas
            - Autenticacao e autorizacao com JWT
            - Separacao entre entidades, DTOs e inputs
            """
    });

    options.IncludeXmlComments(apiXmlPath, includeControllerXmlComments: true);

    if (File.Exists(coreXmlPath))
    {
        options.IncludeXmlComments(coreXmlPath);
    }

    options.CustomSchemaIds(type => type.FullName?.Replace('+', '.') ?? type.Name);
    options.OrderActionsBy(api => $"{api.ActionDescriptor.RouteValues["controller"]}_{api.HttpMethod}_{api.RelativePath}");

    var bearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token no formato: Bearer {token}"
    };

    options.AddSecurityDefinition("Bearer", bearerScheme);
    options.OperationFilter<SwaggerDocumentationOperationFilter>();
    options.DocumentFilter<SwaggerTagDescriptionsDocumentFilter>();
});

// Registra o contexto do banco.
// Scoped = uma instancia por request, padrao recomendado para EF Core.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    // Lazy loading permite carregar propriedades de navegacao sob demanda.
    options.UseLazyLoadingProxies();
}, ServiceLifetime.Scoped);

// Contexto de conexao do Dapper.
// Neste projeto ele convive com o EF Core para mostrar um modelo hibrido.
builder.Services.AddScoped<DapperContext>();

// Mapeia interfaces de repositorio para suas implementacoes concretas.
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ILivroDapperRepository, LivroDapperRepository>();
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IPedidoDapperRepository, PedidoDapperRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Registra a camada de servicos, onde ficam regras de negocio e mapeamentos.
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Carrega as configuracoes do JWT que estao no appsettings.
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection.GetValue<string>("Key")
    ?? throw new InvalidOperationException("Jwt:Key nao configurada.");
var jwtIssuer = jwtSection.GetValue<string>("Issuer");
var jwtAudience = jwtSection.GetValue<string>("Audience");

// Define como a API vai autenticar tokens enviados no header Authorization.
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Define politicas de autorizacao reutilizaveis.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));
});

var app = builder.Build();

// Habilita o servimento de arquivos estaticos usados na customizacao do Swagger.
app.UseStaticFiles();

// Em desenvolvimento, a documentacao fica disponivel ja na raiz da aplicacao.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FiapStoreApi v1");
        options.RoutePrefix = string.Empty;
        options.DocumentTitle = "FiapStoreApi | Swagger";
        options.DocExpansion(DocExpansion.List);
        options.DefaultModelRendering(ModelRendering.Model);
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableFilter();
        options.EnablePersistAuthorization();
        options.InjectStylesheet("/swagger-ui/custom.css");
        options.InjectJavascript("/swagger-ui/custom.js");
    });
}

// Middleware global de excecao para padronizar erros nao tratados.
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    try
    {
        await next();
    }
    catch (Exception exception)
    {
        logger.LogError(exception, "Erro global capturado");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            message = "Erro interno no servidor"
        });
    }
});

// Middleware de log basico para observar request, response e tempo de execucao.
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var inicio = DateTime.UtcNow;

    logger.LogInformation("Request: {Metodo} {Url}", context.Request.Method, context.Request.Path);

    await next();

    var tempo = DateTime.UtcNow - inicio;

    logger.LogInformation(
        "Response: {StatusCode} - Tempo: {Tempo}ms",
        context.Response.StatusCode,
        tempo.TotalMilliseconds.ToString("F0"));
});

// Daqui para baixo comeca o pipeline padrao das requisicoes.
app.UseHttpsRedirection();

// Primeiro identifica quem e o usuario...
app.UseAuthentication();

// ...depois aplica as permissoes exigidas pelo endpoint.
app.UseAuthorization();

// Faz o binding final entre rotas HTTP e controllers.
app.MapControllers();

// Inicializa a aplicacao.
app.Run();
