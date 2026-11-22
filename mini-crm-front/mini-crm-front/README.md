# Mini CRM Front

Frontend Angular 22 da aplicação Influence CRM. O projeto oferece um shell CRM responsivo e páginas conectadas aos endpoints versionados `/api/v1` da API .NET.

## Pré-requisitos

- Node.js compatível com Angular 22
- npm
- API backend disponível em `https://localhost:5001` ou em outra URL configurada

## Instalação e execução

A partir da pasta `mini-crm-front/mini-crm-front`:

```bash
npm install
npm start
```

O servidor de desenvolvimento usa a porta configurada em `angular.json`:

- URL: `http://127.0.0.1:51751`
- Para executar em outra porta: `npm start -- --port 4200`

## Configuração da API

A URL base fica em `src/environments/environment.ts`:

```ts
export const environment = {
  apiUrl: 'https://localhost:5001',
};
```

Use `src/environments/environment.development.ts` para substituições locais. O frontend chama:

```text
${apiUrl}/api/v1/<recurso>
```

Os endpoints foram definidos sem autenticação e não exigem login, JWT, guards ou checagem de roles nesta fase.

## O que já está disponível

### Shell da aplicação

- Sidebar responsiva com navegação CRM.
- Barra superior com busca e acesso a notificações.
- Menu móvel para telas menores que 1024 px.
- Página inicial em `/dashboard`.
- Página 404 para rotas desconhecidas.
- Layout em cards, métricas, tabelas e painéis.

### CRUD reutilizável

A `ResourcePage` fornece listagem, detalhe lateral, criação, edição e estados de carregamento/erro para os recursos conectados à API. Os dados são enviados em JSON conforme os DTOs tipados em `src/app/core/api-models.ts`.

Para recursos sem suporte completo de edição na API, a página permanece limitada às rotas disponíveis no backend.

### Fluxos implementados

| Área | Tela ou rota | Operações disponíveis |
|---|---|---|
| Dashboard | `/dashboard` | Visão geral e atalhos |
| Clientes | `/customers` | Listar, detalhar, criar, editar e arquivar |
| Contatos | `/customers/:id/contacts` | Listar, criar e editar contatos |
| Leads | `/leads` | Listar, detalhar, criar, editar, qualificar e marcar perdido |
| Oportunidades | `/opportunities` | Listar, detalhar, criar, ganhar, perder e converter em campanha |
| Propostas | `/proposals` | Listar, detalhar, criar, enviar, aprovar e rejeitar |
| Itens da proposta | `/proposals/:proposalId/items` | Listar e criar itens |
| Campanhas | `/campaigns` | Listar, detalhar, criar, editar, iniciar, concluir e cancelar |
| Marcos | `/campaigns/:id/milestones` | Listar marcos da campanha |
| Criadores | `/campaigns/:campaignId/creators` | Listar, adicionar, aprovar e cancelar participação |
| Timeline | `/campaigns/:id/timeline` | Consultar linha do tempo |
| Resumo da campanha | `/campaigns/:id/summary` | Consultar resumo operacional e financeiro |
| Influenciadores | `/influencers` | Listar e detalhar |
| Perfis sociais | `/influencers/:influencerId/social-profiles` | Listar perfis sociais |
| Perfis de pagamento | `/influencers/:influencerId/payment-profiles` | Listar perfis de pagamento |
| Curadoria | `/curation-requests` | Listar, detalhar e criar solicitações |
| Candidatos | `/curation-candidates` | Listar, detalhar, criar, aprovar e rejeitar |
| Contratos | `/contracts` | Listar, detalhar, criar, enviar para assinatura, assinar e cancelar |
| Entregáveis | `/deliverables` | Listar, detalhar, criar, enviar, aprovar, rejeitar e publicar |
| Contas a receber | `/accounts-receivable` | Listar, detalhar, criar e registrar recebimento |
| Contas a pagar | `/accounts-payable` | Listar, detalhar, criar e registrar pagamento |
| Financeiro | `/finance` | Redireciona para contas a receber |
| Transações | `/financial-transactions` | Listar, detalhar e criar |
| Tarefas | `/tasks` | Listar, detalhar, criar, atribuir, iniciar, concluir e cancelar |
| Notificações | `/notifications` | Listar, detalhar, criar e marcar como lida |

## Comandos de transição

Os recursos que possuem operações de ciclo de vida mostram botões explícitos em vez de permitir alteração arbitrária do campo `status`:

- Leads: `Qualificar`, `Marcar perdido`.
- Oportunidades: `Marcar ganha`, `Marcar perdida`, `Converter em campanha`.
- Propostas: `Enviar`, `Aprovar`, `Rejeitar`.
- Campanhas: `Iniciar`, `Concluir`, `Cancelar`.
- Entregáveis: `Enviar`, `Aprovar`, `Rejeitar`.
- Tarefas: `Iniciar`, `Concluir`, `Cancelar`.
- Notificações: `Marcar lida`.

## Estrutura principal

```text
src/app/
├── app-module.ts
├── app-routing-module.ts
├── core/
│   ├── api-models.ts
│   ├── api-error.interceptor.ts
│   └── crm-api.service.ts
├── dashboard/
├── resource-page/
├── not-found/
├── sales/
├── campaigns/
├── influencers/
├── operations/
├── finance/
└── workflow/
```

`CrmApiService` concentra as chamadas HTTP e `apiErrorInterceptor` transforma respostas HTTP em `Problem Details` tipados.

## Erros da API

O frontend trata os casos principais:

- `400`: dados inválidos ou campos ausentes.
- `404`: recurso não encontrado.
- `409`: conflito de unicidade ou transição de estado inválida.
- `500`: falha inesperada sem exposição de detalhes internos.
- Status `0`: backend indisponível ou CORS/conexão bloqueada.

O JSON informado nas páginas de cadastro deve respeitar os nomes e tipos definidos em `src/app/core/api-models.ts`.

## Build e testes

```bash
npm run build
npm test -- --watch=false
```

O build de produção gera a aplicação em `dist/mini-crm-front`.

Os testes atuais verificam a criação do componente raiz e os formatos TypeScript de paginação, cliente e `Problem Details`.

## Limitações atuais

- O formulário reutilizável usa JSON até a criação dos formulários reativos dedicados por recurso.
- O dashboard utiliza dados de apresentação iniciais e ainda não agrega todos os endpoints.
- Nem toda ação planejada possui controller específico no backend, como edição aberta de marcos e documentos de contratos.
- Login, JWT, roles e autorização não foram implementados.
- Os testes de integração e E2E dependem da API e do banco configurados.

## Referências

- Angular 22
- Configuração em `angular.json`
- Modelos da API em `src/app/core/api-models.ts`
- Rotas em `src/app/app-routing-module.ts`
