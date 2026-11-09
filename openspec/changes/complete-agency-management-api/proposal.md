# Proposal

## Why

A aplicação já possui as entidades de domínio, o `AppDbContext`, migrations, conexão MySQL e Swagger, mas ainda não oferece uma API de negócio para administrar a operação de uma agência de intermediação entre influencers e agências de marketing. O único endpoint existente é o controller de exemplo `WeatherForecast`.

É necessário transformar o modelo persistido em uma API consistente, validada e organizada por casos de uso, permitindo o cadastro e a manutenção de todas as entidades envolvidas no ciclo comercial, financeiro e fiscal.

## What Changes

- Criar endpoints REST para todas as entidades do domínio:
  - Entidades comerciais, clientes, influencers, agências, intermediários e agentes.
  - Redes sociais e vínculos de influencers com suas redes.
  - Bancos, contas bancárias e vínculos de influencers com contas.
  - Contratos, despesas e pagamentos.
  - Notas fiscais, formatos, status e agendamentos de notas fiscais.
- Implementar operações de consulta, cadastro, atualização e exclusão conforme aplicável a cada entidade.
- Organizar a aplicação em camadas de repositories, services e use cases.
- Padronizar cada caso de uso com `Request`, `Response` e `Handler`, utilizando o nome do caso de uso como prefixo.
- Criar controllers finos, responsáveis apenas pelo protocolo HTTP e delegação aos handlers.
- Definir DTOs para evitar expor diretamente as entidades do EF Core na API.
- Adicionar validação de entrada, respostas HTTP consistentes, tratamento de recursos inexistentes e regras de integridade dos relacionamentos.
- Documentar os endpoints no Swagger.
- Adicionar testes para os casos de uso, repositories e endpoints principais.

A implementação deve preservar a estrutura atual de entidades e o banco MySQL, ajustando o modelo somente quando necessário para corrigir inconsistências que impeçam uma API coerente.

## Capabilities

### New Capabilities

- `agency-management-api`: Disponibiliza uma API REST completa para gerenciar entidades comerciais, influencers, agências, agentes, redes sociais, contas bancárias, contratos, despesas, pagamentos e documentos fiscais.
- `application-use-cases`: Organiza operações de negócio em use cases independentes, cada um com request, response e handler próprios.
- `persistence-repositories`: Define abstrações e implementações de repositories para acesso aos dados por meio do `AppDbContext`.

### Modified Capabilities

Nenhuma capability existente possui requisitos publicados no projeto. O diretório `openspec/specs` atualmente não contém especificações.

## Impact

- **API:** novos controllers, rotas REST, DTOs e contratos HTTP.
- **Application:** novas pastas de use cases, handlers, requests, responses e services.
- **Domain:** interfaces de repositories e possíveis ajustes nas entidades para explicitar regras de negócio e identificadores.
- **Infrastructure:** repositories concretos, consultas EF Core, transações e ajustes de configuração do `AppDbContext` quando necessários.
- **Persistência:** uso do schema MySQL existente e possível criação de novas migrations caso o modelo seja corrigido.
- **Documentação:** expansão do Swagger para todos os endpoints.
- **Testes:** criação de cobertura unitária e de integração para o fluxo completo da API.

A API usará `EntidadeComercialId` como identificador técnico único nas rotas para clientes, influencers, agências e intermediários. Os campos `Id` específicos atualmente existentes serão preservados e expostos nos DTOs como identificadores de negócio, sem substituir a chave técnica da hierarquia.
