# Proposal

## Why

The existing backend has basic commercial entities and EF Core persistence, but it does not represent the opportunity-to-campaign lifecycle, creator participation, deliveries, or campaign financial obligations specified by `crm-domain-specification`. Implementing those domain and persistence records is a prerequisite for the typed CRM API; exposing generic legacy entities in their place would conflate distinct business lifecycles.

## What Changes

- Implement the missing CRM aggregates and lifecycle invariants defined by `crm-domain-specification`, while extending compatible existing client, contract, invoice, and payment records rather than duplicating them.
- Add EF Core mappings, repositories, transaction support, and MySQL migrations for the CRM aggregates and their references to existing records.
- Preserve existing entity keys and relationships. For new tables, use `long` generated identifiers for low-volume core records and `Guid` identifiers for high-write detail/ledger records, based on the expected daily insert volume.
- Keep campaign, creator participation, delivery, invoice, receivable/payable, and settlement lifecycles distinct; do not model receivables or payables as generic legacy payments.
- Defer audit fields/events and authentication, authorization, and department permission enforcement entirely to future changes, as requested.

## Capabilities

### New Capabilities

None. This change implements the domain behavior already specified by the active `crm-domain-specification` change and does not introduce a separate behavioral contract. `.openspec.yaml` sets `skip_specs: true` to avoid duplicating that specification.

### Modified Capabilities

None.

## Impact

- **Domain:** `mini-crm/Domain/Entities` and repository contracts for CRM lifecycle records.
- **Persistence:** `AppDbContext`, EF Core configurations, `Infrastructure/Repositories`, dependency injection, and MySQL migrations.
- **Tests:** domain invariant, EF model/mapping, repository, transaction, and migration verification tests in the existing xUnit project.
- **Compatibility:** current commercial entity keys and legacy `/api/...` contracts remain unchanged; no controllers or Angular files are in scope.
- **Deferred:** audit metadata/history and all authentication, authorization, and permission enforcement are not implemented by this change.
