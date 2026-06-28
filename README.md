# InventarioComercialAPI

API para gerenciamento de inventário comercial construída com .NET 10.

## Visão geral
Aplicação Web API em ASP.NET Core que expõe endpoints via controllers. Usa Entity Framework Core com provedor Npgsql (PostgreSQL) para persistência, integra um mediator (Inproc) e publica documentação OpenAPI/Swagger em ambiente de desenvolvimento.

Principais pontos observados no código:
- Registro de controllers e Swagger: AddControllers(), AddEndpointsApiExplorer(), AddSwaggerGen().
- Registro de mediator: builder.Services.AddMediator(...).
- DbContext: ApplicationDbContext com Npgsql (connection string nomeada `DefaultConnection`), QuerySplittingBehavior = SplitQuery e retry em falhas (EnableRetryOnFailure(3)).
- Swagger/OpenAPI é mapeado em ambiente Development via `app.MapOpenApi()`.
- Rotas/endpoints: controllers mapeados com `app.MapControllers()`.
- Pipeline mínimo: HTTPS redirection e Authorization middleware.
- Arquivo de perfil de execução (launchSettings.json) com perfis locais, Docker e IIS Express com portas e variáveis de ambiente definidas.

## Tecnologias
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core (Npgsql)
- PostgreSQL
- Swagger / OpenAPI
- Mediator (registrado via AddMediator)
- Suporte para execução via Docker e IIS Express

## Requisitos
- .NET 10 SDK instalado
- PostgreSQL (ou acesso a um servidor PostgreSQL)
- Docker (opcional, para executar em container)

## Configuração (variáveis e connection string)
Crie/edite o arquivo appsettings.json ou configure variáveis de ambiente com a connection string chamada `DefaultConnection`.

Exemplo de appsettings.json (trecho):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=InventarioDb;Username=postgres;Password=SuaSenha"
  }
}
```

Variáveis úteis observadas em launchSettings.json:
- ASPNETCORE_ENVIRONMENT (por padrão `Development` nos perfis locais)
- Para execução em container (profiles): ASPNETCORE_HTTP_PORTS e ASPNETCORE_HTTPS_PORTS (ex.: 8080 e 8081)

Ajuste credenciais/host/porta conforme seu ambiente.

## Rodando localmente (dotnet CLI)
1. No diretório do projeto:
   - Restaurar dependências:
     dotnet restore
   - Build:
     dotnet build
   - Executar:
     dotnet run --project InventarioComercialAPI

Observações:
- Os perfis em launchSettings.json indicam URLs padrão locais:
  - HTTP: http://localhost:5260
  - HTTPS: https://localhost:7256
  - IIS Express (se usado via Visual Studio): http://localhost:57115/ com sslPort 44386

- Em ambiente Development, a UI do OpenAPI/Swagger é mapeada via `app.MapOpenApi()`; abra o browser no endpoint de documentação (por exemplo /swagger) quando o app estiver em Development.

## Executar em Docker
O launchSettings contém um perfil "Container (Dockerfile)" com:
- ASPNETCORE_HTTPS_PORTS = 8081
- ASPNETCORE_HTTP_PORTS = 8080
- publishAllPorts = true

Exemplo básico (ajuste nome do Dockerfile e imagem conforme existir):
- Construir imagem:
  docker build -t inventarioapi .
- Executar container:
  docker run -e ASPNETCORE_HTTP_PORTS=8080 -e ASPNETCORE_HTTPS_PORTS=8081 -p 8080:8080 -p 8081:8081 inventarioapi

Depois, acesse http://localhost:8080 ou https://localhost:8081 dependendo da configuração.

## Banco de dados e migrações (EF Core)
O projeto usa ApplicationDbContext e Npgsql. Para criar/atualizar o banco:

- Adicionar uma migration:
  dotnet ef migrations add InitialCreate --project <projeto_da_infraestrutura> --startup-project InventarioComercialAPI

- Aplicar migrações:
  dotnet ef database update --project <projeto_da_infraestrutura> --startup-project InventarioComercialAPI

Observação: ajuste os parâmetros `--project`/`--startup-project` de acordo com a estrutura do seu solution (se houver um projeto de infraestrutura específico, use-o como `--project`).

## Endpoints e documentação
- A API usa controllers (MapControllers).
- A geração da documentação OpenAPI/Swagger está registrada com AddSwaggerGen() e é ativada em ambiente Development (MapOpenApi()).
- Teste e explore endpoints pela UI do Swagger quando em Development.

## Observações sobre comportamento do EF Core
- QuerySplittingBehavior.SplitQuery foi configurado para evitar problemas de cartesian product em include de coleções.
- Retry (EnableRetryOnFailure(3)) para tolerância em falhas de conexão temporárias com o banco.

## Desenvolvimento e contribuições
- Abra issues descrevendo bugs ou features.
- Para PRs, inclua descrição, como reproduzir e testes.
- Mantenha o padrão de commits e mensagens claras.

## Troubleshooting
- Se a API não conectar ao banco, valide a `DefaultConnection` e se o PostgreSQL está acessível.
- Em Docker, verifique mapeamento de portas e variáveis ASPNETCORE_* definidas.
- Se Swagger não aparecer, confirme que ASPNETCORE_ENVIRONMENT=Development ao executar localmente.

