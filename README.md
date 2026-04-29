# FiapStoreApi

Projeto didatico em ASP.NET Core organizado em tres camadas principais:

- `Core`: onde ficam as entidades, DTOs, contratos de repositorio e contratos de servico.
- `Infrastructure`: onde ficam Entity Framework, `DbContext`, mapeamentos e repositorios concretos.
- `FiapStoreApi`: onde ficam `Program.cs`, controllers e services expostos pela API.

## Como ler o projeto

Uma boa ordem de leitura para entender o sistema e:

1. `FiapStoreApi/Program.cs`
2. `Core/Entity`
3. `Core/Input` e `Core/DTO`
4. `Core/Repository` e `Core/Service`
5. `Infrastructure/Repository`
6. `FiapStoreApi/Services`
7. `FiapStoreApi/Controllers`

## Papel de cada camada

### 1. Core

O `Core` representa o centro da regra de negocio. Ele nao deveria depender de HTTP, banco ou detalhes do ASP.NET.

- `Entity`: modela o dominio real do sistema.
- `Input` e `DTO`: definem o formato de entrada e saida da API.
- `Repository`: declara contratos de persistencia.
- `Service`: declara contratos das regras de negocio.

### 2. Infrastructure

Essa camada implementa a persistencia.

- `Repository/EntityFramework`: concentra `ApplicationDbContext`, `EFRepository<T>` e os repositorios que escrevem/leem via EF Core.
- `Repository/Dapper`: concentra `DapperContext` e os repositorios de leitura com SQL manual.
- `Repository/Configurations`: dizem ao EF Core como cada entidade vira tabela/coluna/relacao.
- `Migrations`: guarda o historico de evolucao do banco gerado pelo EF Core.

### 3. API

Essa camada recebe requests HTTP e devolve responses HTTP.

- `Program.cs`: configura DI, Swagger, autenticacao JWT, autorizacao, banco e middlewares.
- `Controllers`: expoem as rotas.
- `Services`: concentram regras de negocio, validacoes e mapeamentos.

## Fluxo de uma requisicao

Quando uma chamada chega na API, o fluxo geral e:

1. O request entra no pipeline configurado em `Program.cs`.
2. Middlewares de log, autenticacao e autorizacao sao executados.
3. O controller correspondente recebe a chamada.
4. O controller usa um service ou repositorio.
5. O repositorio usa o `ApplicationDbContext` ou o `DapperContext`, dependendo do tipo de acesso a dados.
6. O resultado volta como DTO para o controller.
7. O controller devolve a resposta HTTP final.

## Pontos didaticos importantes

- `Entity` nao e a mesma coisa que `DTO`.
  `Entity` representa como o sistema pensa os dados.
  `DTO` representa como a API troca dados com o mundo externo.

- `Repository` nao e a mesma coisa que `Service`.
  `Repository` cuida de persistencia.
  `Service` cuida de regra de negocio.

- `Controller` nao deveria concentrar regra complexa.
  O ideal e ele apenas orquestrar HTTP.

- JWT resolve autenticacao e autorizacao.
  O login gera um token com claims, e depois os atributos `[Authorize]` usam essas claims para permitir ou negar acesso.

- `Configuration` do EF evita poluir a entidade com detalhes de banco.

## Observacoes sobre este projeto

- Os controllers agora seguem um padrao mais consistente:
  eles falam com a camada de `service`, enquanto os detalhes de leitura e escrita ficam escondidos atras da regra de negocio.

- Agora o projeto tambem mostra um modelo hibrido de acesso a dados:
  EF Core continua sendo usado para escrita e migrations.
  Dapper passa a ser usado em leituras selecionadas, onde o SQL manual ajuda a estudar consultas e mapeamento.

- A estrutura foi organizada para deixar mais clara a separacao entre `DTO`, `Input`, `Service` e `Repository`.

- Os arquivos de migration em `Infrastructure/Migrations` sao gerados pelo EF Core.
  Eles sao importantes para versionar o banco, mas nao costumam ser o melhor ponto de entrada para entender a regra de negocio.

## Testes de unidade

O projeto agora possui um projeto separado de testes chamado `FiapStoreApi.Tests`.

- A ideia dos testes de unidade e validar a regra de negocio sem depender de banco, Swagger ou API rodando.
- Por isso, os testes focam principalmente na camada de `Services`, que e onde estao as decisoes mais importantes do sistema.
- Em vez de usar um framework de mock, os testes usam dublês simples em `FiapStoreApi.Tests/TestDoubles`.
  Isso deixa mais facil enxergar como cada dependencia foi simulada.

### O que os testes mostram

- `AuthServiceTests`: validam login, geracao de token JWT, hash de senha e bloqueio de email duplicado.
- `ClienteServiceTests`: validam mapeamento de input para entidade/DTO e a regra de pedidos dos ultimos 6 meses.
- `LivroServiceTests`: mostram o padrao hibrido do projeto, com leitura via Dapper e escrita via EF Core.
- `PedidoServiceTests`: validam a criacao de pedido, incluindo os cenarios de cliente/livro inexistentes.

### Como executar

Use o comando abaixo na raiz da solucao:

```powershell
dotnet test FiapStoreApi.sln
```

Se voce quiser estudar a estrutura de um teste, uma boa ordem de leitura e:

1. `FiapStoreApi.Tests/TestDoubles/RepositoryFakes.cs`
2. `FiapStoreApi.Tests/Services/AuthServiceTests.cs`
3. `FiapStoreApi.Tests/Services/ClienteServiceTests.cs`
4. `FiapStoreApi.Tests/Services/LivroServiceTests.cs`
5. `FiapStoreApi.Tests/Services/PedidoServiceTests.cs`
