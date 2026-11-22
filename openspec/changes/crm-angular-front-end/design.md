# Design

## Context

See `proposal.md` for motivation and `specs/*` for observable behavior. The frontend is Angular 22 using the NgModule application style configured in `angular.json`. It currently contains only the starter `App` component and an empty routing module. The backend exposes unauthenticated `/api/v1` CRUD and command routes, but database migration and some integration verification remain separate work.

## Goals / Non-Goals

**Goals:**
- Replace the starter screen with a maintainable CRM shell and feature routes.
- Map every implemented `/api/v1` controller to one or more typed Angular services and pages.
- Reuse a common shell, table, form, dialog, status, pagination, and error system.
- Keep lifecycle actions explicit and prevent status fields from being submitted through generic updates.
- Provide deterministic routing and accessible loading/error/empty states.

**Non-Goals:**
- Authentication, authorization, JWT, login, route guards, role UI, or security-sensitive field projection.
- Backend controller, database, migration, or legacy `/api/...` redesign.
- A pixel-perfect copy of a commercial CRM or a new component library dependency.
- Offline-first storage or real-time WebSocket behavior.

## Decisions

### 1. Use feature-oriented NgModules and reactive forms

Keep the existing NgModule configuration but organize the application by feature: `shell`, `sales`, `campaigns`, `influencers`, `operations`, `finance`, and `workflow`. Each feature owns routing, pages, models, and services; shared code belongs in `core` or `shared`.

**Alternative considered:** A completely standalone Angular application. Rejected because it would force an unnecessary rewrite of the existing NgModule bootstrap and Angular CLI configuration.

### 2. Add one page per primary use case

Use dedicated routes and components for dashboard, customer list/detail/form, contact form, lead list/detail/form, opportunity list/detail/form, proposal/detail, campaign list/detail/form, campaign timeline/summary, influencer list/detail, curation request/candidate, contract, deliverable, receivable, payable, transaction, task, and notification pages.

Nested controller surfaces share a parent page when they are a section of the use case: customer contacts on customer detail, proposal items in proposal detail, campaign milestones/creators/timeline/summary in campaign detail, and influencer social/payment profiles in influencer detail.

**Alternative considered:** A generic JSON table that derives pages from controller metadata. Rejected because CRM workflows and lifecycle commands require purpose-built layouts and validation.

### 3. Use typed services mirroring controller ownership

Each route group receives a narrowly named HTTP service, such as `CustomersApi`, `OpportunitiesApi`, `CampaignsApi`, or `DeliverablesApi`. Services expose typed methods for collection, item, and command operations. DTO interfaces live under feature models and match server request/response names rather than EF entities.

The base API URL is environment/configuration-based and uses `/api/v1` by default. The development setup uses an Angular proxy or direct local backend origin, configurable by environment.

### Exact controller and DTO contract matrix

Every Angular service implementation SHALL be checked against this matrix. Property names use the exact JSON names emitted by ASP.NET Core with the project's default serialization settings. Request records are the controller contracts currently in `mini-crm/Application/Contracts`; domain response/navigation objects remain unacceptable in frontend models unless the controller explicitly returns a typed view.

| Service / route group | HTTP operations | Exact request contract |
|---|---|---|
| `CustomersApi` `/api/v1/customers` | `GET ?query&active&page&pageSize`, `GET /{id}`, `POST`, `PUT /{id}`, `PATCH /{id}/archive`, contacts `GET/POST /{id}/contacts`, `GET/PUT /{id}/contacts/{contactId}` | Create: `name`, `cnpj`, `segmento`, `preferenciaIdioma`, `codigo`; update: `name`, `cnpj`, `segmento`, `preferenciaIdioma`; contact create/update: `name`, `cargo`, `email`, `telefone`, `principal` |
| `LeadsApi` `/api/v1/leads` | `GET`, `GET /{id}`, `POST`, `PUT /{id}`, `POST /{id}/qualify`, `POST /{id}/lose` | `nome`, `empresa`, `email`, `telefone`, `responsavelId` |
| `OpportunitiesApi` `/api/v1/opportunities` | `GET`, `GET /{id}`, `POST`, win/lose, `POST /{id}/convert-to-campaign` | `clienteEntidadeComercialId`, `responsavelId`, `briefing`, `orcamento`, `moeda` |
| `ProposalsApi` `/api/v1/proposals` | `GET`, `GET /{id}`, `POST`, `POST /{id}/send`, `/approve`, `/reject` | `oportunidadeId`, `valor`, `moeda`; items: `descricao`, `quantidade`, `valorUnitario`, `moeda` |
| `CampaignsApi` `/api/v1/campaigns` | `GET`, `GET /{id}`, `POST`, start/complete/cancel, `GET /{id}/milestones`, timeline/summary routes | `oportunidadeId`, `clienteEntidadeComercialId`, `responsavelId`, `frente`, `dataInicio`, `dataFim`, `totalAprovado`, `moeda`, `tipoTaxa`, `taxa` |
| `CampaignCreatorsApi` `/api/v1/campaigns/{campaignId}/creators` | `GET`, `POST`, approve/cancel by `participationId` GUID | `influencerEntidadeComercialId`, `compensacao`, `moeda`, `termos` |
| `InfluencersApi` `/api/v1/influencers` | `GET`, `GET /{id}`, social-profiles and payment-profiles nested GET routes | No create/update DTO is currently implemented for the top-level influencer controller |
| `CurationRequestsApi` `/api/v1/curation-requests` | `GET`, `GET /{id}`, `POST`, `GET /{id}/candidates` | `campanhaId`, `briefing` |
| `CurationCandidatesApi` `/api/v1/curation-candidates` | `GET /{id}`, `POST`, `POST /{id}/approve`, `/reject` | `solicitacaoCuradoriaId`, `influencerEntidadeComercialId`, `avaliacao` |
| `ContractsApi` `/api/v1/contracts` | `GET`, `GET /{id}`, `POST`, send-for-signature/register-signature/cancel | `clienteEntidadeComercialId`, `agenciaEntidadeComercialId`, `agenteId`, `influencerEntidadeComercialId`, `conteudoContrato` |
| `DeliverablesApi` `/api/v1/deliverables` | `GET`, `GET /{id}`, `POST`, submit/approve/reject/publish | `campanhaId`, `participacaoCampanhaId`, `titulo`, `dataLimiteUtc`; publish: `evidencia`, `publicadoEmUtc` |
| `AccountsReceivableApi` `/api/v1/accounts-receivable` | `GET`, `GET /{id}`, `POST`, `POST /{id}/payments` | `campanhaId`, `participacaoCampanhaId`, `valor`, `valorAprovado`, `moeda`, `dataVencimentoUtc`; settlement: `valor`, `moeda`, `referencia` |
| `AccountsPayableApi` `/api/v1/accounts-payable` | `GET`, `GET /{id}`, `POST`, `POST /{id}/payments` | Same financial and settlement fields as receivable, with `participacaoCampanhaId` when applicable |
| `FinancialTransactionsApi` `/api/v1/financial-transactions` | `GET`, `GET /{id}`, `POST` | `contaReceberId`, `contaPagarId`, `repasseCreatorId`, `valor`, `moeda`, `referencia` |
| `TasksApi` `/api/v1/tasks` | `GET`, `GET /{id}`, `POST`, assign/start/complete/cancel | `titulo`, `descricao`, `responsavelId`, `prazoUtc` |
| `NotificationsApi` `/api/v1/notifications` | `GET`, `GET /{id}`, `POST`, `POST /{id}/read` | `titulo`, `mensagem` |

The Angular service SHALL use the public route identifier type declared by the controller: `long` for commercial and campaign IDs, `Guid` for participation/delivery/curation/task/notification/transaction IDs, and composite IDs only for nested influencer profile read routes that have no item GET route. The frontend SHALL never substitute `Cliente.Id`, `Influencer.Id`, or any other business GUID for `EntidadeComercialId` in route URLs.

For routes currently absent or only partially implemented by the backend, the frontend SHALL mark the operation unavailable until a controller and typed DTO exist. In particular: proposal items use the dedicated nested controller, milestone CRUD beyond `GET /campaigns/{id}/milestones` is not currently implementable, and campaign reopen/revision/contract-document routes are not currently controller-backed.

**Alternative considered:** One generic `CrmApiService`. Rejected because it obscures route ownership and makes lifecycle command contracts harder to type and test.

### 4. Translate HTTP failures through one interceptor plus page-level state

A core HTTP interceptor normalizes Problem Details and network failures. Reactive components use typed loading/error states and reusable 400/404/409/500 presentation. Form controls receive field errors returned by the API or by client validation.

**Alternative considered:** Handling errors independently in every service. Rejected because route parity and sanitized messaging would drift.

### 5. Use signals for local UI state and RxJS for HTTP streams

Services expose `Observable` results for HTTP compatibility with Angular. Feature components use signals or reactive form state for local loading, dialogs, sorting, and selection. Shared components remain presentation-oriented and receive inputs/outputs instead of injecting API services.

**Alternative considered:** NgRx or another state library. Rejected for the initial implementation because backend CRUD state does not justify a store and can be delivered with feature-local state.

### 6. Standardize collection and command UX

Collection pages use a shared table, page/pageSize controls, filters, deterministic sort, loading, empty, and retry states. Create/edit dialogs or pages use typed reactive forms. Lifecycle commands use confirmation dialogs that require reasons or evidence when the contract requires them and disable the command only when the server state makes it known to be invalid.

**Alternative considered:** Allowing status fields in every create/update form. Rejected because lifecycle transitions must remain explicit and conflict-protected.

### 7. Use a generic CRM visual system

The shell uses a neutral CRM layout: deep sidebar, off-white content canvas, white cards, restrained primary color, status badges, compact tables, and responsive detail panels. There is no visual dependency on the Angular starter and no requirement to reproduce an existing brand system.

**Alternative considered:** A feature-heavy design system or third-party UI kit. Rejected to keep the change focused and dependencies minimal.

## Risks / Trade-offs

- **[API DTO mismatches]** → Generate or manually maintain typed interfaces, then add HTTP contract tests and review them against controller records and OpenAPI.
- **[Backend route availability is uneven]** → Feature tasks group implemented routes first and mark planned or absent contract surfaces as blocked/deferred.
- **[A single campaign/finance page becomes too dense]** → Split detail views into route tabs/sections: overview, milestones, creators, timeline, summary, deliverables, and finance.
- **[No authentication is implemented]** → Do not build login or role conditions; document that routes remain unauthenticated and that future access control will require guards and data projections.
- **[Angular starter configuration is minimal]** → Add routing and `provideHttpClient` incrementally and preserve the existing NgModule bootstrap.

## Migration Plan

1. Replace the starter shell and establish routes and shared patterns.
2. Add typed API models/services and HTTP error normalization.
3. Implement sales, influencer/curation, campaign, operations, finance, and workflow feature pages in vertical slices.
4. Add component/service tests and run Angular build/test.
5. Run against the backend after database configuration and verify route, payload, pagination, and 400/404/409/500 behavior.
6. Roll back to the Angular starter by routing removal and shell rollback only; no backend or persisted data rollback is required.

## Open Questions

None. Authentication, role UI, and backend availability/deployment restrictions remain explicit non-goals for this change.
