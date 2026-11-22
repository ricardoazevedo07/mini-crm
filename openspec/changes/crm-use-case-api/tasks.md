# Tasks

## Existing implementation baseline

The legacy `complete-agency-management-api` change is 8/35 tasks complete. Reuse the implemented `OperationResult`/paging contracts, repository interfaces and EF implementations, reference checker, `IUnitOfWork`, current-model mappings, and existing `AppDbContext` entities where their semantics fit. Legacy `/api/...` controllers and the reflection-based `GenericCrudHandler<TEntity>` are generic CRUD, not typed CRM contracts; do not duplicate compatible entities, count those routes as `/api/v1` completion, or use the generic handler for CRM transitions. The completed `crm-domain-specification` tasks are specification/handoff work only; CRM aggregate code is still absent.

## 1. Prerequisites and API scope

- [x] 1.1 Inventory which existing entities/repositories can support each CRM resource, agree their public-ID mapping, and identify missing aggregates/mappings per vertical slice; verify compatible slices reuse existing records and only block slices that depend on absent CRM concepts.
- [x] 1.2 Keep authentication and authorization out of this implementation: do not configure JWT, credentials, middleware, or role policies; verify `/api/v1` operations do not require credentials. Defer verified identity and access control to a separate future change.
- [x] 1.3 Define the public-ID mapping and stable `/api/v1` route inventory against the domain and legacy `/api/...` endpoints; verify each resource route has one owning controller and no route collision.

## 2. Shared HTTP contracts and error handling

- [x] 2.1 Reuse the existing `PageRequest`, `PagedResult`, and application result types where suitable; add typed CRM request/response and filter contracts, validating serialization and rejecting server-owned identifier/status fields.
- [x] 2.2 Extend the current exception middleware/error abstractions beyond their existing sanitized 500 behavior to map validation, not-found, conflict, and unexpected failures to stable Problem Details; test 400, 404, 409, and 500 without leaking details. Do not add 401/403 contracts in this change.
- [x] 2.3 Reuse primary-key ordering and database-level paging in `EfRepository`; reject invalid paging values instead of silently normalizing them, add resource filters through targeted queries, and test empty pages, limits, count, and deterministic ordering.

## 3. Customers and sales controllers

- [x] 3.1 Implement typed customer/contact and lead CRUD handlers and `/api/v1` controllers with search filters; verify create/read/update/list behavior, duplicate-client conflicts, and field-level validation in handler and HTTP tests.
- [x] 3.2 Implement opportunity and proposal/item CRUD handlers and controllers, including explicit win/loss, proposal send/approve/reject/version, and opportunity-to-campaign commands; verify invalid transitions return 409 and approved proposals cannot be edited in place.
- [ ] 3.3 Publish OpenAPI contracts for customer and sales endpoints and add integration tests for routes, DTOs, `Location`, pagination, and errors; verify schemas match actual responses and declare no authentication requirement.

## 4. Influencers and curation controllers

- [x] 4.1 Add typed versioned use cases/controllers over the existing `Influencer`, `RedeSocial`, `InfluencerRedeSocial`, and bank-account association records where semantics match; implement only missing curation/metric/payment-profile concepts after their domain contracts exist, and verify DTOs match the documented response shapes.
- [x] 4.2 Implement curation request and candidate CRUD/query handlers plus assign/start/evaluate/recommend/submit/approve/reject/select/complete commands; verify related IDs are validated, duplicate candidates are rejected, and lifecycle conflicts return 409.
- [ ] 4.3 Add OpenAPI and integration tests for influencer and curation routes; verify response contracts, filtering, pagination, unauthenticated access, and transition history.

## 5. Campaign and creator-participation controllers

- [ ] 5.1 Implement campaign and milestone CRUD/query handlers and controllers, including timeline, summary, start, complete, cancel, and reopen operations; verify status cannot be set by generic create/update and invalid transitions preserve state.
- [x] 5.2 Implement creator participation CRUD and approve/cancel operations with campaign scoping; verify duplicate active participation is rejected and cancellation retains existing contracts, deliverables, and financial history.
- [ ] 5.3 Add campaign OpenAPI definitions and integration tests for response contracts, filters, pagination, unauthenticated access, and status commands; verify outputs match the campaign response contract.

## 6. Legal and deliverable controllers

- [ ] 6.1 Add typed versioned use cases/controllers over the existing `Contrato` record and preserve its current relationships; implement campaign-scoped documents, clauses, approvals, and lifecycle/version behavior only after the required CRM domain/persistence contracts exist, verifying signed versions are immutable.
- [x] 6.2 Implement deliverable, approval, and revision use cases/controllers with submit, review, revision, approve/reject, schedule, publish, and complete operations; verify rejection reason and publication evidence/time are required.
- [ ] 6.3 Add integration and OpenAPI tests for legal and deliverable routes; verify DTOs exclude persistence navigation graphs and lifecycle failures return consistent Problem Details.

## 7. Financial controllers

- [x] 7.1 Implement receivable/payable handlers/controllers only after their domain and persistence records exist; do not represent obligations as legacy `Pagamento`, `Despesa`, or `NotaFiscal` records, and verify currency, due-date, balance, and unauthenticated access behavior.
- [ ] 7.2 Add typed CRM settlement/approval/reconciliation operations while preserving the existing legacy `Pagamento` contract; model immutable settlement history and partial balances in the CRM financial domain, verifying currency mismatch and over-balance. Defer authenticated actor attribution and permission-based approval segregation.
- [ ] 7.3 Add integration tests and OpenAPI contracts for financial endpoints; verify their typed response contracts, unauthenticated access, and application-use-case delegation.

## 8. Tasks, notifications, and API documentation

- [x] 8.1 Implement task and notification query/update use cases and controllers, including task assignment/status commands and mark-read operations; do not scope access by user identity or role in this change.
- [ ] 8.2 Publish OpenAPI documentation for every implemented `/api/v1` resource, command, query parameter, DTO, and error response; verify generated route/schema inventory against the design endpoint matrix and declare no authentication or authorization requirements.
- [ ] 8.3 Add integration tests for tasks, notifications, and OpenAPI discovery; verify errors, access without credentials, pagination, and response schemas.

## 9. Integration verification

- [ ] 9.1 Run the full backend build and test suites, including MySQL integration tests for the CRM API vertical slices; verify the new controllers resolve through dependency injection and do not alter existing unversioned endpoints.
- [ ] 9.2 Exercise the generated OpenAPI document and representative end-to-end flows from customer/opportunity conversion through campaign, deliverable, contract, and financial settlement; verify behavior and HTTP responses against the spec.

## Workflow follow-up

- Review and archive the CRM domain and persistence prerequisites before applying API tasks that depend on their entities and mappings.
- Keep `complete-agency-management-api`'s legacy CRUD implementation separate; review route compatibility before any future migration from unversioned `/api/...` endpoints.
