# Design

## Context

The backend targets .NET 10 and EF Core 10 with the existing MySQL provider. `AppDbContext` currently maps a TPH `EntidadeComercial` hierarchy plus social, banking, contract, expense, payment, invoice, and invoice-scheduling records in `OnModelCreating`. The domain has mixed legacy key types, generic and per-entity repository interfaces, a generic EF repository, and an `IUnitOfWork`; no CRM migrations or domain-specific model tests were found. See proposal.md for motivation and the active `crm-domain-specification` change for the behavioral contract.

## Goals / Non-Goals

**Goals:**
- Implement the defined opportunity, campaign, creator participation, delivery, and financial-obligation lifecycles in the domain and persist them without replacing the existing commercial hierarchy.
- Preserve independent lifecycle/history for campaigns, contracts, invoices, obligations, and settlements.
- Map and test the new model using the existing EF Core/MySQL stack and retain compatibility with existing records and unversioned routes.

**Non-Goals:**
- Controllers, `/api/v1` endpoints, Angular services/screens, or authentication/authorization.
- Audit fields, audit-event storage, or creator/updater attribution; the user explicitly deferred audit entirely.
- Department permission enforcement; the user explicitly deferred authentication and authorization.
- Replacing the existing MySQL provider, legacy keys, or current unversioned CRUD contract.

## Decisions

### 1. Extend the existing model without shadowing legacy records

Keep `Cliente`, `Contrato`, `NotaFiscal`, `Pagamento`, and their current keys. Add the campaign/opportunity links and missing CRM fields/relationships to compatible records only where the domain specification requires them. Create separate records for opportunities, campaigns, creator participation, deliveries, receivable/payable obligations, and their settlement history. A legacy `Pagamento`, `Despesa`, or `NotaFiscal` is not itself a receivable or payable.

**Alternative considered:** Replace the existing financial or commercial model with a new CRM-only schema; rejected because it would break existing relationships and duplicate records already used by the legacy API.

### 2. Choose keys by expected write volume

Preserve every existing primary key. Use database-generated `long` keys for low-volume, business-critical roots (`Oportunidade`, `Campanha`, `ContaReceber`, and `ContaPagar`). Use `Guid` keys for high-write detail/ledger records (`CampanhaCreator`, `Entrega`, delivery revisions/evidence, and settlement/repass entries). Treat this as the initial volume-based mapping; record types and key mapping must be explicit in EF configuration and tests.

**Alternative considered:** Use one key type for every table; rejected because the user requested long keys for important low-volume tables and GUIDs for tables expected to receive several inserts per day.

### 3. Keep aggregate invariants in domain operations and enforce storage constraints

Domain methods own allowed status transitions, cancellation/rejection reasons, immutable signed/settled history, partial balances, and completion rules. EF constraints enforce unique active client legal identity, one campaign per source opportunity, and one active creator participation per campaign. Cross-record opportunity conversion and multi-record settlements run through the existing unit-of-work transaction boundary; no controller or generic reflection CRUD may bypass the domain operations.

**Alternative considered:** Enforce lifecycle rules only in controllers; rejected because the same domain behavior must remain valid for other application entry points.

### 4. Store money and dates consistently

Use `decimal(18,2)` for monetary amounts and an ISO 4217 three-character currency code for CRM money values; do not extend the closed legacy `Moeda` enum as the CRM currency contract. Store timestamps as UTC. Model `Frente` as a campaign classification string until a controlled business taxonomy exists; represent the fee basis as fixed amount or percentage, and represent creator portions/repasses as separate obligation/settlement records. Overdue is derived from due date and outstanding balance, not written as a replacement for partial-settlement status.

**Alternative considered:** Reuse the legacy `Moeda` enum for all CRM values; rejected because its current values are limited to Real and Dolar and cannot represent ISO currency codes generally.

### 5. Keep owner references independent of authentication

Retain commercial/operational owner references as opaque `long` employee IDs without an EF foreign key until an employee/identity model exists. No authenticated actor is inferred from a request, and no audit actor fields are written in this change. The API change remains responsible for the separately approved unauthenticated behavior.

**Alternative considered:** Add a staff/identity subsystem as part of this domain change; rejected because authentication/authorization and employee identity were deferred and no such model exists in the current solution.

### 6. Follow current persistence conventions incrementally

Register new `DbSet`s and mappings in the existing `AppDbContext`, keeping legacy mapping behavior intact. Add typed repository interfaces only for queries/invariants that the generic repository cannot express; implement them under `Infrastructure/Repositories` and register them in infrastructure DI. Add an explicit migration for schema changes and validate the model and migration against the configured MySQL provider. Do not add a different database provider just for tests.

## Risks / Trade-offs

- **[Existing schema uses mixed IDs and TPH]** → Preserve current keys; test every new FK against the correct `EntidadeComercialId` or aggregate key.
- **[No authentication means there is no verified actor]** → Do not claim user attribution; keep audit and permission requirements deferred.
- **[Existing `Moeda` enum is narrower than CRM currencies]** → Use a separate ISO code field for new CRM financial records and preserve legacy enum storage.
- **[Many new lifecycle records increase schema and migration complexity]** → Implement vertical slices in dependency order and validate model relationships and migration diffs before applying them.
- **[Volume-based ID grouping is an estimate]** → Keep the selected type explicit and review it before the first production migration if actual insert volume assumptions change.

## Migration Plan

1. Add domain value/status types and aggregate records while preserving all current entity keys.
2. Add EF mappings, DbSets, indexes, delete rules, and targeted repositories for the new aggregates.
3. Generate and review MySQL migrations; verify relational model creation and existing mappings remain unchanged.
4. Exercise atomic conversion/settlement operations and lifecycle invariants with unit and persistence tests.
5. Apply the migration in development and verify the backend build/test suite; do not remove or rewrite legacy data.

## Open Questions

None that block the implementation plan. `Frente` will initially be an open classification string, fee basis is fixed or percentage, and repasse/settlement are distinct records; revisit those representations only if the business adopts a controlled vocabulary or conversion policy before migration.
