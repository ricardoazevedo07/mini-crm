# Design

## Context

The current domain contains a generic base model (`EntidadeComercial`) and several entities such as `Cliente`, `Agencia`, `Contrato`, `Pagamento`, `NotaFiscal`, and `Despesa`, but it does not yet represent the full business flow described in the Remo use-case. The project already has a foundational concept of commercial entities, but it lacks explicit lifecycle, sector-based visibility, and audit metadata expected by the CRM.

See proposal.md for the motivation and the capability scope.

## Goals / Non-Goals

**Goals:**
- Model the end-to-end CRM lifecycle from client opportunity to campaign, financial settlement, and management reporting.
- Make policy boundaries explicit: commercial, curadoria, planejamento, atendimento, jurídico, financeiro, and gestão.
- Define status and ownership metadata so the system can trace who changed what and when.
- Establish a durable structure that future persistence and API layers can build on without reworking the domain model.

**Non-Goals:**
- Implementing the application UI or business workflow screens.
- Choosing a specific database vendor or infrastructure provider beyond the domain contract.
- Defining exact controller actions or repository implementations in this planning artifact.

## Decisions

### 1. Domain-first modeling before API and persistence
The design will prioritize the domain model as the source of truth for the CRM behavior. Entities and status types will be modeled before repository or controller implementation so that downstream layers inherit the correct semantics.

**Why this choice:** The current project already shows a domain layer but not the full operative business flow; the risk is to build controllers or repositories around inconsistent assumptions.

**Alternative considered:** Starting from controllers and persistence would lock in incomplete assumptions and create rework. The domain-driven approach reduces that risk.

### 2. Use a campaign-centered aggregate model
The design will center the domain around a `Campanha` record, with related subordinate records for `CampanhaCreator`, `Entrega`, `Contrato`, `Pagamento`, `NotaFiscal`, `ContaReceber`, `ContaPagar`, and `Repasse`.

**Why this choice:** The use-case repeatedly describes campaigns as the operational unit and indicates that each sector acts on a campaign after conversion from opportunity.

**Alternative considered:** A purely relational model without a core campaign aggregate would obscure ownership, stage transitions, and cross-sector coordination. A campaign-centric aggregate keeps those responsibilities explicit.

### 3. Domain includes role-derived visibility rules, not just data tables
The design will include explicit authorization semantics and visibility policies in the domain specification, rather than treating permissions as purely UI concerns.

**Why this choice:** The case study explicitly states that each sector has different visibility into values and that finance and management access sensitive balances.

**Alternative considered:** Omitting these rules would leave access control undefined and would push security decisions into the UI or later infrastructure work.

### 4. Add lifecycle and audit metadata to core entities
Core entities will include status, owner, creator, updater, and timestamps to match the requirement that each record has a responsible person and time of change.

**Why this choice:** The use-case highlights that information is recorded once, has a responsible owner, and is tracked over time.

**Alternative considered:** Storing these properties only in application code would be fragile and would not survive future integrations or auditing demands.

## Domain Boundaries and Repository Findings

The existing model already has `Cliente`, `Influencer`, `Agencia`, `Contrato`, `Despesa`, `Pagamento`, `NotaFiscal`, `AgendamentoNotaFiscal`, and supporting social-profile and bank-account entities. `EntidadeComercial` is a shared base and currently owns payment navigation. These are persistence-shaped entities with few lifecycle rules; `Cliente` has a `Guid Id` while the shared commercial key and most relationships use `long`. The model has a Boolean invoice flag and a separate invoice-schedule status lookup, but no common creation/update audit fields or recorded actor.

The following use-case concepts are not explicit domain entities yet: `Oportunidade`, `Campanha`, `CampanhaCreator`, `Entrega`, accounts receivable/payable, payment portions/repasses, and role/permission policy. Existing contracts and payments are not linked to campaigns because campaigns do not yet exist. Treat this as a gap inventory, not as a deletion or replacement plan for the current model.

### Aggregate boundaries

| Aggregate/root | Owns and protects | References and constraints |
|---|---|---|
| `Cliente` | Legal identity, commercial profile, contacts, active/archive state | Unique legal identity for active records; archival is required instead of deletion while opportunities or campaigns remain active. |
| `Oportunidade` | Intake/briefing, commercial owner, estimated value, stage, close/loss details | References `Cliente` and owner by ID; conversion to campaign is one-time and only from a won state. |
| `Campanha` | Operational owner, front, schedule, currency, fee structure, lifecycle | References customer and originating opportunity; coordinates related records but does not own their independent financial or legal lifecycles. |
| `CampanhaCreator` | One creator's campaign participation, negotiated compensation, terms, approval and repasse linkage | Unique active participation per creator/campaign; cancellation preserves existing obligations and payment history. |
| `Entrega` | One deliverable's due date, submission, review, evidence, revisions and publication state | References campaign and creator participation; approval/revision history is retained. |
| `Contrato` | Parties, campaign/creator scope, version, legal status, signature and documents | Signed versions are immutable; amendments create a new version/record. |
| `ContaReceber` / `ContaPagar` | One financial obligation, amount, currency, due date, settlement balance and status | References campaign and customer, creator, contract, or participation as applicable; payment entries cannot exceed remaining balance. |
| `NotaFiscal` | Invoice identity, party, amount, issue/status and link to a financial obligation | Invoice and receivable/payable are related but distinct records; invoice status cannot substitute for payment history. |
| `Pagamento` / `Repasse` | Individual receipt or disbursement, date, method, reference and responsible actor | Append-only financial history associated with its obligation; corrections use a compensating entry rather than deleting a settled entry. |

Only invariants that require atomic consistency belong within an aggregate boundary. Cross-aggregate workflows (for example, winning an opportunity and creating a campaign) are coordinated by an application service and must be idempotent; they must not be implemented as direct navigation-graph mutation.

## Role and Visibility Model

Permissions are enforced by the API/application boundary and are never replaced by hiding fields in the Angular UI. The matrix is the initial least-privilege domain contract; the exact department-specific edit rights and the handling of agency-margin visibility remain policy decisions to confirm with the business owner.

| Department/role | May view | May create/update | Restricted data/actions |
|---|---|---|---|
| Comercial | Clients, opportunities, briefings, proposal/campaign commercial summary | Client intake, contacts, opportunities, briefing and commercial stage before handoff | No bank details, payment receipts or reconciliation; access to creator compensation only when explicitly granted. |
| Curadoria | Campaign brief and selection criteria; creator profiles and permitted metrics | Creator candidates, evaluations, recommendations and selection status | No client receivables, bank details, payment records or agency margin. |
| Planejamento | Campaign plan, creator assignments, schedules and deliverables | Planning data, assignment plan and target dates within authorized stages | Cannot approve/sign contracts or post/alter financial transactions. |
| Atendimento | Campaign operational data, contacts, delivery status and evidence | Operational brief, delivery tracking, approval requests and evidence | Financial actuals, bank details and payment operations are restricted; budget visibility is policy-controlled. |
| Jurídico | Campaign parties, contract scope, clauses, documents, approvals and signatures | Contract drafts, legal review, versions, signature and cancellation records | Cannot register financial settlement or view bank credentials; financial amounts only as needed for contract terms. |
| Financeiro | Invoices, receivables, payables, fees, balances, receipts, repasses and reconciliation | Financial obligations, approvals, settlements and reconciliation | Cannot alter commercial briefing, curation evaluation or signed legal content. Bank details are restricted to payment workflows. |
| Gestão | Cross-department operational dashboards, audit trail and authorized financial summaries | Management-level decisions and explicitly delegated overrides | Sensitive detail access is audited; overrides require a reason and cannot erase history. |

Each entity also has a responsible owner. Read, write, approve, and sensitive-field permissions are separate decisions; the same user must not gain approval rights merely because they can edit a record.

## Lifecycle and Status Semantics

Status changes are explicit domain operations with allowed transitions; clients cannot set an arbitrary status through a generic update. The names below are the proposed canonical lifecycle, while localized labels may be presented by the UI.

| Record | Lifecycle statuses | Delay/terminal rules |
|---|---|---|
| `Oportunidade` | New, Qualified, Briefing, Proposal, Negotiation, Won, Lost | Won/Lost are terminal; Lost requires a reason. Won can produce at most one campaign. |
| `Campanha` | Draft, Planning, Curation, Negotiation, Contracting, ReadyToStart, InProgress, PendingApproval, Completed, Cancelled | Completed requires operational close validation; Cancelled requires a reason; reopening is explicit and audited. |
| `Entrega` | Planned, InProduction, Submitted, UnderReview, RevisionRequested, Approved, Scheduled, Published, Completed, Cancelled | A rejection retains a reason and revision history; Published requires publication time/evidence. |
| `Contrato` | Draft, UnderReview, Approved, SentForSignature, Signed, Active, Completed, Rejected, Cancelled | Signed content is immutable; cancellation requires a reason; approval is not signature. |
| `Fatura` | Draft, Issued, Sent, PartiallyPaid, Paid, Cancelled | Overdue is derived when the due date passes with a positive balance; it does not overwrite payment history. |
| `ContaReceber` / `ContaPagar` | Planned, PendingApproval, Approved, PartiallySettled, Settled, Cancelled | Overdue is derived from due date and outstanding balance; partial settlement remains visible. |
| `Repasse` | Planned, PendingApproval, Approved, Scheduled, PartiallyPaid, Paid, Cancelled | Overdue is derived from due date and remaining balance; paid/cancelled records cannot accept ordinary edits. |

The workflow must preserve the difference between operational campaign completion and financial settlement. Campaign closure requires the operational close checklist and completed financial reconciliation; reconciliation does not require every obligation to be paid. Any outstanding balance must be explicitly dispositioned and surfaced in the close summary.

## Audit and Financial Data Contract

Core records carry `CriadoPor`, `DataCriacao`, `AtualizadoPor`, and `DataAtualizacao`; timestamps are UTC. Creation metadata is required, update metadata is set on each successful change, and an audit record captures entity, identifier, actor, action, timestamp, and the changed fields. Sensitive values (bank credentials and personal identifiers) must be excluded or masked in audit payloads. Audit data is append-only for ordinary users.

Financial records use `decimal(18,2)`-equivalent precision and an explicit ISO 4217 currency code. Never use floating-point types or compare/add monetary values across currencies without an explicit conversion. At minimum:

| Record | Required value/date data |
|---|---|
| Campaign | Currency, approved gross budget/total, agency fee amount and fee basis (fixed/percentage), and effective dates. |
| Creator participation / repasse | Negotiated fee, currency, approved amount, payment terms/portion, due date, and amount already paid. |
| Invoice and receivable/payable | Gross amount, currency, issue date where applicable, due date, total received/paid, and computed outstanding balance. |
| Payment/receipt | Positive amount, currency matching the obligation, effective date, payment method, reference/proof when available, and responsible actor. |
| Approval | Approved amount and currency, approver, decision, timestamp, and comments/reason when rejected. |

Balances are derived as obligation amount minus the sum of immutable settlement entries. No settlement may exceed the outstanding balance. Exact fee policy, currency-conversion rules, and which roles may view agency margin require business confirmation before those rules are implemented.

## Implementation Handoff

The current backend targets .NET 10 and EF Core 10 and references a MySQL provider. Its current controller surface includes a generic CRUD controller; the domain entities use mixed identifier types and do not yet define the campaign lifecycle. This change specifies the domain contract only: it does not define Angular routes/services, concrete HTTP endpoint names, or a database migration. Persistence and API work must first confirm the identifier strategy and whether MySQL remains the selected provider; UI/service work must consume the eventual typed API contract rather than infer endpoints from domain class names.

### Requirement-to-design traceability

| Spec behavior | Design decision / section |
|---|---|
| Client/opportunity lifecycle and campaign conversion | Domain-first modeling; aggregate boundaries for `Cliente`, `Oportunidade`, and `Campanha`. |
| Creator participation, delivery, contract, invoice, and financial histories | Campaign-centered model; separate participation, delivery, contract, invoice, obligation, and settlement lifecycles. |
| Department read/write and sensitive-field access | Role and Visibility Model; API-boundary authorization and least privilege. |
| Explicit status transitions, overdue handling, close conditions | Lifecycle and Status Semantics; overdue derived from due date and outstanding balance. |
| Actor/timestamps and protected audit history | Audit and Financial Data Contract. |
| CRUD, duplicate prevention, archival, and preserved history | Client/opportunity aggregate invariants and spec scenarios; destructive deletion is rejected where history/active work exists. |
| Management dashboards and overdue-item reporting | Management summaries apply the same department and sensitive-field visibility policy as record queries. |

Before persistence or API implementation, confirm the open business decisions, choose an identifier strategy, and define typed request/response contracts for each use case. Do not expose a generic persistence CRUD surface as a substitute for domain transitions or assume the Angular service contract from the generic controller.

## Risks / Trade-offs

- [Role visibility complexity] → Mitigation: model permissions as first-class domain rules and keep them explicit in the business specification before implementation.
- [Domain expansion] → Mitigation: stage the model in capability-based increments so the campaign lifecycle, financial domain, and authorization rules evolve without breaking existing entities.
- [Over-modeling generic entities] → Mitigation: keep the design grounded in the Remo use-case so the model reflects business reality rather than theoretical abstractions.

## Migration Plan

1. Align the current domain entities with the campaign-first model without deleting the existing base abstractions that still provide useful structure.
2. Introduce new domain entities and status enums incrementally, preserving the existing directory structure under `Domain/Entities`.
3. Validate the new model against the business rules in the spec before implementation of repositories or API controllers.
4. Once the domain contract is accepted, build persistence and API layers to satisfy the same behavior contract.

## Open Questions

- Confirm the exact meaning of `front`, fee basis, creator payment portions, and repasse with the Remo business owner.
- Confirm which departments can see campaign budget, agency margin, creator fees, and customer/creator payment status; the initial matrix above is a least-privilege proposal, not a substitute for policy approval.
- Confirm whether the current MySQL provider remains in use and choose one identifier strategy before mapping the new domain to persistence.
- Decide whether invoice and payment records are separate aggregate roots or children after persistence consistency and volume requirements are known; their domain lifecycles and immutability rules remain as specified above.
