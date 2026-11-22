# Proposal

## Why

The API currently exposes legacy resources through generic CRUD controllers under unversioned `/api/...` routes, with `JsonElement` payloads and reflection-based entity updates. That surface does not provide typed, use-case-specific contracts for the CRM domain (opportunities, campaigns, creator participation, deliveries, contracts, and financial obligations), making it easy for controllers and future Angular services to disagree about paths, request shapes, status transitions, and error responses.

## What Changes

- Define a versioned `/api/v1` HTTP contract for the CRM domain use cases, with explicit controller routes, typed request/response DTOs, stable identifiers, pagination/filtering, and OpenAPI documentation.
- Provide list, detail, create, and update operations where allowed for clients, opportunities, campaigns, creator participation, deliveries, contracts, invoices, receivables, payables, and their payment/repass records.
- Expose explicit commands for business transitions (such as winning/converting an opportunity, starting/closing/cancelling a campaign, approving/rejecting a delivery or contract, and recording/reconciling settlements); do not allow generic CRUD payloads to set lifecycle status or overwrite immutable financial/legal history.
- Keep controllers thin: delegate to typed application use-case handlers, use repositories/unit-of-work abstractions, and never query `AppDbContext` from controllers.
- Standardize validation, `201 Created`/`Location`, `204 No Content`, `400`, `404`, `409`, and sanitized Problem Details responses.
- Implement the `/api/v1` routes without authentication or authorization; credential validation, role policies, and sensitive-field access control are deferred to a future change.
- Preserve the existing unversioned routes during this additive change; define their compatibility boundary and avoid silently changing existing clients while the legacy API plan is in progress.
- Add controller, handler, and integration tests that verify routes and HTTP contracts against OpenAPI and persistence behavior without authentication or authorization setup.

This change complements, rather than replaces, `complete-agency-management-api`. That change is still in progress (8 of 35 tasks are checked): its completed work covers shared application results and paging, repository abstractions and EF implementations, reference checks, unit-of-work support, and current EF mappings. The repository already has unversioned generic CRUD routes for existing records such as clients, influencers, agencies, contracts, payments, and invoices. Those routes accept `JsonElement` and use reflection-based property mapping; their presence does not mean typed CRM contracts or lifecycle rules are implemented.

The `crm-domain-specification` change is complete as a specification and handoff, not as implementation of CRM aggregates. No CRM-domain code was added by that change. This API change therefore reuses existing records and persistence abstractions only where their semantics match, preserves their current identifiers and unversioned routes, and does not create duplicate tables or treat legacy payment/invoice records as receivable/payable aggregates. New CRM concepts remain dependent on their domain and persistence implementation.

## Capabilities

### New Capabilities
- `crm-use-case-api`: Versioned HTTP contracts and use-case endpoints for the CRM domain lifecycle, including typed CRUD, business transitions, and consistent errors; authentication and authorization are deferred.

### Modified Capabilities
- None. The repository has no published capabilities under `openspec/specs` yet; current CRM domain and legacy API requirements remain in their in-flight changes.

## Impact

- **API:** `mini-crm/Controllers`, route conventions, API DTOs, validation, error responses, and Swagger/OpenAPI.
- **Application:** typed requests, responses, handlers, validators, and explicit transition use cases.
- **Infrastructure:** reuse existing generic/per-entity repositories, unit of work, reference checks, and paging where appropriate; add CRM-specific queries only as required by typed contracts.
- **Existing records:** adapt compatible `Cliente`, `Influencer`, social-link, contract, expense, payment, invoice, and catalog records without duplicating their persistence model; keep existing `/api/...` endpoints and their contracts separate.
- **Domain/persistence dependencies:** CRM-only records absent from `AppDbContext` (including opportunities, campaigns, creator participation, deliveries, receivables, and payables) require their domain entities and mappings before their corresponding API slices; do not invent HTTP-only models.
- **Compatibility:** current unversioned `/api/...` endpoints remain available in this change; no Angular code or frontend services are modified.
- **Tests:** unit tests for handler/controller translation and integration tests for representative CRUD, transition, validation, and error behavior; no authentication or authorization tests are in scope.
