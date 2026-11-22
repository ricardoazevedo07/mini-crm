# Design

## Context

The project targets .NET 10 / ASP.NET Core, EF Core 10, and MySQL. The active `complete-agency-management-api` change has 8 of 35 tasks checked. Its completed work is present in the code: shared operation/paging types, generic and per-entity repository contracts and EF implementations, reference checks, a unit of work, and EF mappings for the current model. `ResourceControllers.cs` exposes unversioned generic CRUD routes for the existing client, influencer, agency, social, banking, contract, expense, payment, and fiscal entities. `EntityController<TEntity>` accepts `JsonElement`, while `GenericCrudHandler<TEntity>` constructs entities and assigns writable properties reflectively; these routes are not typed CRM use cases and do not implement CRM lifecycle transitions.

The application DI registers only `GenericCrudHandler<>`; infrastructure DI registers the generic and current per-entity repositories, reference checker, and unit of work. Existing repository paging orders by the primary key and returns count metadata, but `PageRequest` silently normalizes invalid page values and current list routes provide no resource filters. The exception middleware sanitizes unexpected failures as 500 only; it does not yet provide the complete stable error contract. `Program.cs` calls `UseAuthorization()` but configures no authentication scheme and does not call `UseAuthentication()`.

The current domain and `AppDbContext` contain compatible existing records including `Cliente`, `Influencer`, social/banking associations, `Contrato`, `Despesa`, `Pagamento`, `NotaFiscal`, and fiscal catalogs/schedules. They do not contain CRM concepts such as `Oportunidade`, `Campanha`, `CampanhaCreator`, `Entrega`, `ContaReceber`, or `ContaPagar`. `crm-domain-specification` completed its planning tasks only; it did not implement these aggregates. Reuse an existing record only when its business semantics match, and preserve its identity; do not create shadow records. See proposal.md and `specs/crm-use-case-api/spec.md` for the motivation and observable behavior.

## Goals / Non-Goals

**Goals:**
- Define one exact, versioned endpoint and DTO contract for CRM-specific Angular/API clients.
- Keep HTTP controllers thin and route business actions through typed use cases and persistence abstractions.
- Preserve current legacy routes during rollout while ensuring new CRM endpoints use the domain model and its invariants.
- Use consistent, documented HTTP errors; authentication and authorization are explicitly deferred.

**Non-Goals:**
- Replacing or silently changing legacy `/api/...` routes and their contracts.
- Building Angular screens, client services, or a frontend authentication flow in this change.
- Authentication, authorization, JWT validation, role policies, and verified user attribution; these will be handled in a separate future change.
- Implementing domain entities, migrations, or repositories already owned by the domain/persistence changes; this API work depends on those contracts being available.
- Exposing arbitrary generic CRUD over EF entities or allowing status changes via mass assignment.

## Decisions

### 1. Separate versioned CRM controllers from legacy CRUD controllers

New controllers are organized under `Controllers/Api/V1` (or an equivalent consistent namespace) with explicit `[ApiController]` and `/api/v1` routes. The existing controllers in `ResourceControllers.cs` remain on `/api/...` while consumers migrate. The current legacy route inventory includes clients, influencers, social networks and links, agencies, agents and agency-agent links, intermediaries, banks and bank accounts, contracts, expenses, payments, invoices, invoice formats/statuses, and invoice schedules. Do not derive the new routes from legacy CLR entity names or count a generic legacy route as fulfillment of a typed CRM endpoint.

**Why:** The existing API is an unversioned generic CRUD surface. `complete-agency-management-api` has implemented shared persistence/application foundations but its resource-specific CRUD, controllers, validation, and tests remain in progress. A separate versioned contract avoids route and response collisions and provides a stable source for Angular services.

**Alternative considered:** Re-route all existing controllers under `/api/v1` now; rejected because it breaks existing clients and blends unrelated current-schema CRUD with the domain-first CRM contract.

### Existing resource and identifier compatibility

The inventory below distinguishes records that can back a typed CRM endpoint from concepts that have no matching domain/persistence record. These mappings reuse the current entities and repositories; they do not imply that typed `/api/v1` handlers or lifecycle behavior already exist.

| CRM resource | Existing compatible records and repository abstractions | Public identifier for an eventual typed route | Gaps / boundary |
|---|---|---|---|
| Customer | `Cliente`, `IClienteRepository` | `EntidadeComercialId` as the route ID; `Cliente.Id` remains a business identifier | No separate contact entity/repository; lead, opportunity, and proposal are not represented. |
| Influencer | `Influencer`, `IInfluencerRepository` | `EntidadeComercialId`; `Influencer.Id` remains a business identifier | Curation requests/candidates, niches, and metric snapshots are absent. |
| Influencer social profile | `RedeSocial`, `InfluencerRedeSocial` and their repositories | Composite key `(InfluencerEntidadeComercialId, RedeSocialId)` | The existing relationship contains a link; it is not a metric/profile history model. |
| Influencer payment/bank profile | `ContaBancaria`, `InfluencerContaBancaria` and their repositories | `ContaBancaria.Id` for the account; association key `(InfluencerEntidadeComercialId, ContaBancariaId)` | No dedicated CRM payment-profile contract exists; bank records remain distinct from financial obligations. |
| Agency and intermediary parties | `Agencia`, `Intermediario`, respective repositories | `EntidadeComercialId`; subtype `Id` remains a business identifier | Do not use the subtype ID as the commercial-entity route key. |
| Agency/agent relationship | `Agente`, `AgenciaAgente` and repositories | `Agente.Id`; association composite key `(AgenciaEntidadeComercialId, AgenteId)` | Agents are a legacy commercial model, not campaign owners unless domain rules establish that mapping. |
| Contract | `Contrato`, `IContratoRepository` | `Contrato.Id` | Campaign-scoped documents, clauses, approvals, and version history are not modeled. |
| Expense and payment records | `Despesa`, `Pagamento` and repositories | Each record's `Id` | These do not replace campaign receivable/payable obligations, immutable settlement ledgers, or reconciliation aggregates. |
| Invoice and fiscal scheduling | `NotaFiscal`, `FormatoNotaFiscal`, `StatusNotaFiscal`, `AgendamentoNotaFiscal` and repositories | Each record's `Id` | No campaign link or receivable/payable lifecycle is modeled. |
| Opportunity, proposal, campaign, creator participation, deliverable, curation, receivable/payable, task, notification | None | Defer public-ID choice to the domain/persistence definition; route IDs must use that record's agreed primary key | These vertical slices are blocked on the missing domain records and mappings; do not create API-only shadow entities or infer a key type. |

For existing entities, the public route key follows the configured primary key: commercial subtypes use `EntidadeComercialId`, independent entities use their configured key, and join entities use their composite keys. Subtype `Id` fields such as `Cliente.Id` and `Influencer.Id` remain business data and are not substituted for `EntidadeComercialId`. For CRM aggregates not yet modeled, the key type and route representation remain intentionally undecided until the domain and persistence mapping are available.

### 2. Use explicit routes for resources and commands

Use the following canonical resource routes, with route IDs represented by the agreed public identifier for each aggregate:

| Resource / use case | Collection and item routes | Business-command routes |
|---|---|---|
| Customers and contacts | `/customers`, `/customers/{id}`, `/customers/{id}/contacts`, `/customers/{id}/contacts/{contactId}` | `PATCH /customers/{id}/archive` |
| Leads | `/leads`, `/leads/{id}`, `/leads/{id}/interactions` | `/leads/{id}/qualify`, `/leads/{id}/convert`, `/leads/{id}/lose` |
| Opportunities and proposals | `/opportunities`, `/opportunities/{id}`, `/opportunities/{id}/proposals`, `/proposals/{id}`, `/proposals/{id}/items` | `/opportunities/{id}/win`, `/opportunities/{id}/lose`, `/opportunities/{id}/convert-to-campaign`; `/proposals/{id}/send`, `/approve`, `/reject`, `/versions` |
| Campaigns and assignments | `/campaigns`, `/campaigns/{id}`, `/campaigns/{id}/milestones`, `/campaigns/{id}/creators`, `/campaigns/{id}/timeline`, `/campaigns/{id}/summary` | `/campaigns/{id}/start`, `/complete`, `/cancel`, `/reopen`; creator participation has explicit approve/cancel operations |
| Influencers and curation | `/influencers`, `/influencers/{id}`, `/influencers/{id}/social-profiles`, `/influencers/{id}/metrics`, `/influencers/{id}/payment-profiles`, `/curation-requests`, `/curation-requests/{id}`, `/curation-requests/{id}/candidates`, `/curation-candidates/{id}` | Curation request assign/start/complete; candidate evaluate/recommend/submit/approve/reject/select |
| Contracts | `/contracts`, `/contracts/{id}`, `/contracts/{id}/documents`, `/contracts/{id}/approvals` | `/review`, `/approve`, `/send-for-signature`, `/register-signature`, `/complete`, `/cancel`, `/amendments` |
| Deliverables | `/deliverables`, `/deliverables/{id}`, `/deliverables/{id}/approvals`, `/deliverables/{id}/revisions` | `/submit`, `/request-revision`, `/approve`, `/reject`, `/schedule`, `/publish`, `/complete` |
| Finance | `/accounts-receivable`, `/accounts-receivable/{id}/payments`, `/accounts-payable`, `/accounts-payable/{id}/payments`, `/financial-transactions`, `/financial-transactions/{id}` | `/accounts-payable/{id}/approval`, `/financial-transactions/{id}/reconcile`, campaign financial summary and finance reports |
| Tasks and notifications | `/tasks`, `/tasks/{id}`, `/notifications`, `/notifications/{id}` | `/tasks/{id}/assign`, `/start`, `/complete`, `/cancel`; notification read operations |

Resource collections support `GET` with `page`, `pageSize`, defined filters and deterministic sort; item `GET` returns 404 when absent. `POST` returns `201` and a resolvable `Location`. `PUT` updates editable metadata only. Delete is exposed only for uncommitted records with no protected history; otherwise use archive/cancel commands or return `409`. Use `PATCH` only for explicit commands or partial updates whose contract is defined.

**Alternative considered:** A `PUT` body that includes `Status` for all changes; rejected because it bypasses transition validation, approval records, and audit history.

### 3. Typed application contracts are the controller boundary

For each operation, define a dedicated request and response DTO and a handler in the existing application organization (`Application/UseCases/<Module>/<UseCase>`). Controllers bind/validate the HTTP contract, invoke the handler, and translate `OperationResult` to HTTP. Handlers use repository interfaces and `IUnitOfWork`; controllers never inject `AppDbContext`. Mapping is explicit, excludes EF navigation properties, and never accepts server-owned IDs or fields not editable in the current state.

Reuse common pagination and error abstractions where they fit, but do not reuse `GenericCrudHandler<TEntity>` for domain lifecycle endpoints: its empty construction and reflection-based field assignment cannot enforce typed invariants.

**Alternative considered:** Continue using `JsonElement` with a generic controller and add exclusions; rejected because it remains unable to express per-use-case validation or safe property allowlists.

### 4. Defer authentication and authorization

The `/api/v1` endpoints in this change do not require credentials and do not apply authentication, department/role policies, or sensitive-field authorization. Financial and banking endpoints are included without access-control checks, as decided for this phase. Do not configure JWT validation, an authentication middleware, or authorization policies as part of this change; the application remains usable without an authenticated principal.

Response contracts are typed and explicit, but they are not projected by user role. Secure identity attribution and role-based visibility are deferred to a separate future change. The OpenAPI document must not declare a security scheme or operation-level security requirement. No deployment restriction is added to this change.

**Alternative considered:** Adding a temporary authentication scheme or restricting sensitive endpoints until authorization exists; rejected for this phase so implementation can proceed without access-control dependencies.

### 5. Centralize validation and Problem Details translation

Use request validation before invoking handlers, application error codes for expected failures, and centralized exception handling for unexpected failures. Map validation to 400, missing resources to 404, uniqueness/lifecycle/balance conflicts to 409, and unexpected errors to sanitized 500. Authentication and authorization responses (401/403) are out of scope because routes do not require credentials. Keep error codes stable and do not include SQL or stack traces. Add response metadata to OpenAPI.

### 6. Reuse current persistence selectively and gate only dependent slices

Reuse the existing repository, paging, reference-check, unit-of-work, and EF mapping foundations rather than implementing them again. For existing business records, add typed `/api/v1` DTOs, handlers, and queries over the current entity/repository when its semantics satisfy the CRM contract; preserve the existing identifiers and do not alter the legacy route payloads. The current public legacy key is generally `EntidadeComercialId` for commercial-entity routes, while some records use other or composite keys; agree the versioned public-ID mapping explicitly instead of assuming one universal key.

For concepts absent from the domain and database—such as opportunities, campaigns, creator participation, deliveries, receivables, and payables—implement their domain and persistence contracts before the corresponding API slice. Do not create API-only shadow tables/entities, map a receivable/payable to a generic legacy `Pagamento`, or block independently implementable slices over compatible existing records. Use the existing MySQL provider unless a separate approved change selects a different provider.

`crm-domain-specification` currently supplies the domain contract and implementation handoff, not those entities. `complete-agency-management-api` supplies the current-model persistence foundations and legacy generic routes, but does not satisfy the typed CRM endpoint requirements.

### 7. Test contract alignment at the HTTP boundary

Unit-test handler validation, transitions, and result mapping. Test controllers through ASP.NET Core integration tests using an isolated database/test host without authentication setup. Assert verbs, route templates, DTO serialization, `Location`, pagination/filtering, status codes, Problem Details, requests without credentials, no DbContext access in controllers, and OpenAPI parity. Do not test only that a route exists.

## Risks / Trade-offs

- **[Domain and persistence are not yet present for several API resources]** → Mitigation: make their completion a prerequisite and deliver controllers in vertical slices; do not fake missing aggregates.
- **[Existing legacy API uses unversioned routes and generic handlers]** → Mitigation: preserve legacy routes and define a separate `/api/v1` contract; document any later migration in a dedicated change.
- **[Authentication and authorization are intentionally deferred]** → Mitigation: keep the API contract explicit that routes do not require credentials or enforce roles; add verified identity and sensitive-field policies in a separate future change.
- **[Role-level visibility details are not fully approved]** → Mitigation: defer role-based field projection and permission decisions to the future authentication/authorization change.
- **[Broad CRUD could conflict with lifecycle invariants]** → Mitigation: limit update/delete to editable states and expose status transitions as explicit commands.
- **[Controller DTOs may drift from Angular services]** → Mitigation: publish OpenAPI as the single HTTP contract and require frontend services to consume the same versioned paths and schemas.

## Migration Plan

1. Confirm domain aggregates, public identifier strategy, and persistence/repository prerequisites; no authentication or authorization configuration is required in this change.
2. Add `/api/v1` CRM route groups and typed contracts without modifying existing `/api/...` routes.
3. Implement each resource as a vertical slice: handler/validator, repository query, controller, OpenAPI definition, and tests.
4. Exercise new routes in development and compare generated OpenAPI to the specified route inventory; existing clients continue using legacy routes.
5. Roll back by disabling/unregistering only the new versioned route group; do not remove or alter legacy routes or persisted business history.

## Future Authentication and Authorization

Authentication, authorization, JWT validation, department/role policies, verified actor attribution, and sensitive-field access rules are deferred to a separate future change. They are not prerequisites for implementing the routes in this change.
