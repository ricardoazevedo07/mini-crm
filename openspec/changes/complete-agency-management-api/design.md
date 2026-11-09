# Design

## Context

O projeto é uma aplicação ASP.NET Core em .NET 10 com controllers, Swagger, Entity Framework Core, MySQL e um `AppDbContext` que modela uma hierarquia TPH de `EntidadeComercial`. Ainda não existem camadas de aplicação ou persistência separadas. A motivação e o escopo funcional estão descritos em `proposal.md`; os contratos comportamentais estão nas especificações de `agency-management-api`, `application-use-cases` e `persistence-repositories`.

## Goals / Non-Goals

**Goals:**

- Criar uma arquitetura por camadas com controllers, handlers, services e repositories.
- Expor contratos HTTP estáveis por meio de requests e responses próprios.
- Gerenciar todas as entidades existentes e seus relacionamentos sem retornar grafos de navegação do EF Core.
- Preservar consistência transacional em operações financeiras e relacionais.
- Permitir evolução incremental dos casos de uso mantendo handlers pequenos e testáveis.

**Non-Goals:**

- Implementar autenticação, autorização, multi-tenancy ou auditoria nesta mudança.
- Criar uma aplicação web ou interface administrativa.
- Alterar o provedor MySQL ou substituir o EF Core.
- Introduzir eventos distribuídos, mensageria ou processamento assíncrono externo.

## Decisions

### Organização por camadas e casos de uso

Os controllers ficarão em `Controllers` e terão apenas responsabilidades HTTP. Cada operação será organizada em `Application/UseCases/<Recurso>/<Operacao>/`, contendo `<Operacao><Recurso>Handler`, `<Operacao><Recurso>Request` e `<Operacao><Recurso>Response`. Services concentrarão regras compartilhadas entre casos de uso, enquanto repositories serão responsáveis por consultas e persistência.

Essa abordagem foi escolhida em vez de colocar CRUD diretamente nos controllers porque mantém regras de negócio testáveis e permite que a mesma operação seja reutilizada por outros adaptadores. O uso de um único service genérico de CRUD foi evitado porque os relacionamentos e regras de contratos, pagamentos e documentos fiscais são diferentes.

### Identificadores públicos e internos

`EntidadeComercialId` será o identificador técnico usado nas rotas e nas relações internas da hierarquia de `EntidadeComercial`. Os campos específicos `Cliente.Id`, `Influencer.Id`, `Agencia.Id` e `Intermediario.Id` serão preservados como identificadores de negócio e aparecerão nos responses quando aplicável.

Essa decisão mantém compatibilidade com a chave atualmente configurada no EF Core e evita chaves estrangeiras incompatíveis nos vínculos de influencers, contratos e pagamentos. Requests de criação e atualização não poderão alterar identificadores técnicos; identificadores de negócio terão validação de unicidade quando a regra for definida.

### Contratos HTTP e resultados

Os endpoints usarão rotas no formato `/api/{recursos}` e seguirão convenções REST: `POST` para criação, `GET` para consulta, `PUT` ou `PATCH` conforme a semântica definida no caso de uso e `DELETE` para remoção quando não houver dependências impeditivas. Respostas serão DTOs específicos por recurso, com coleções paginadas contendo itens e metadados.

Falhas serão representadas por `ProblemDetails`, com tratamento centralizado para validação, recurso inexistente, conflito de unicidade e erros inesperados. Isso foi preferido a respostas de erro ad hoc em cada controller para manter o contrato uniforme.

### Persistência e transações

Interfaces de repositories ficarão em `Domain/IRepositories` e implementações em `Infrastructure/Repositories`. Consultas de leitura serão projetadas para DTOs ou modelos de consulta quando isso evitar carregar navegações desnecessárias. Escritas usarão uma unidade de trabalho baseada no `AppDbContext` e transações explícitas somente nos casos que modificarem múltiplos agregados ou associações.

O EF Core continuará sendo usado em vez de adicionar outro micro-ORM, pois o contexto e as migrations já existem e os relacionamentos complexos dependem do mapeamento atual. Queries paginadas aplicarão ordenação estável, `Skip`/`Take` e contagem no banco.

### Validação e mapeamento

Requests serão validados antes da execução do caso de uso, incluindo campos obrigatórios, valores monetários, enums, existência de referências e duplicidade de associações. O mapeamento entre requests/responses e entidades ficará explícito, evitando serialização automática das propriedades de navegação.

A implementação deve revisar as inconsistências do modelo atual apenas quando elas impedirem o fluxo da API, gerando migration quando houver alteração persistida. O identificador técnico da hierarquia não será duplicado nas entidades derivadas.

### Testes

Serão adicionados testes unitários para handlers e services, testes de repository para consultas e relacionamentos críticos e testes de integração para endpoints representativos de cada grupo funcional. Os testes de integração deverão usar uma configuração de banco isolada e não depender da connection string de produção.

## Risks / Trade-offs

- **[Risco]** A hierarquia TPH e os IDs específicos podem gerar confusão entre identificador técnico e de negócio. **Mitigação:** documentar ambos nos DTOs e usar nomes explícitos nos contratos.
- **[Risco]** Expor CRUD para todas as entidades pode gerar uma superfície grande e inconsistente. **Mitigação:** implementar por grupos funcionais, mantendo os mesmos templates de use case e critérios de resposta.
- **[Risco]** Exclusões podem violar relacionamentos existentes. **Mitigação:** validar dependências e retornar conflito quando a remoção física não for segura; avaliar soft delete em uma mudança posterior.
- **[Risco]** MySQL pode apresentar diferenças em transações, tipos decimais e migrations. **Mitigação:** executar testes de integração contra MySQL compatível e revisar a migration gerada antes da aplicação.
- **[Risco]** A ausência de autenticação deixa os endpoints sem proteção. **Mitigação:** declarar autenticação e autorização como escopo posterior e não publicar a API em ambiente externo sem essa camada.

## Migration Plan

1. Implementar as camadas e os endpoints sem alterar o schema quando o modelo atual for suficiente.
2. Gerar uma nova migration somente para correções necessárias ao contrato da API.
3. Aplicar a migration em ambiente de desenvolvimento e executar testes de integração.
4. Aplicar em homologação após revisar chaves, índices e comportamento de exclusão.
5. Em caso de falha, reverter a aplicação para a versão anterior e reverter a migration somente após avaliar perda de dados.

## Open Questions

Nenhuma questão bloqueante permanece para o desenho atual. Autenticação, autorização, auditoria, soft delete e versionamento formal da API podem ser tratados em mudanças futuras sem alterar este desenho base.
