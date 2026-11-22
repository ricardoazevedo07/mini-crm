# Tasks

## 1. Client and domain foundations

- [x] 1.1 Extend `Cliente` with the legal/commercial fields and active/archive state required by the domain contract while preserving `EntidadeComercialId` and `Cliente.Id`; verify normalized legal identity validation and duplicate handling with domain tests.
- [x] 1.2 Add a distinct client-contact record and relationship; verify contacts can be added, updated, and retained independently without changing existing client keys.
- [x] 1.3 Add the CRM currency and monetary primitives using ISO 4217 codes and decimal precision; verify invalid currency codes and non-currency numeric types are rejected or unavailable in the domain contract.
- [x] 1.4 Map client/contact additions in `AppDbContext`, including required indexes and delete/archive rules; verify EF model metadata preserves the existing TPH mapping and keys.

## 2. Opportunity and campaign lifecycle

- [x] 2.1 Implement the opportunity aggregate, its status transitions, owner/briefing/budget data, and required loss reason; verify allowed transitions and invalid-transition behavior with domain tests.
- [x] 2.2 Implement the campaign aggregate with source opportunity, owner references, `Frente`, schedule, currency, approved total, and fee basis; verify required fields, default state, and campaign lifecycle transitions.
- [x] 2.3 Enforce one campaign per source opportunity and preserve the won opportunity history; verify duplicate conversion is rejected and the persistence constraint prevents concurrent duplicate campaigns.
- [x] 2.4 Implement campaign close validation for operational checklist and financial reconciliation, preserving reconciled outstanding balances; verify closure rejects missing prerequisites and reports outstanding balances in domain tests.
- [x] 2.5 Map opportunity/campaign relationships, status, money precision, and uniqueness constraints; verify EF model metadata and targeted repository behavior.

## 3. Creator participation, delivery, and contracts

- [x] 3.1 Implement per-creator campaign participation with negotiated/approved compensation, currency, terms, status, and repasse references; verify duplicate active participation is rejected and cancellation retains obligations.
- [x] 3.2 Implement delivery records and revision/evidence history with due dates, review state, rejection reasons, and publication evidence/time; verify invalid transitions and missing terminal evidence are rejected.
- [x] 3.3 Extend existing `Contrato` with campaign/creator linkage, status, signature/version data, and cancellation reason without changing its current key or party relationships; verify signed versions cannot be edited and amendments preserve prior versions.
- [x] 3.4 Map participation, delivery history, and contract extensions with explicit foreign keys and delete behavior; verify EF model relationships preserve existing contract mappings and legacy rows.

## 4. Financial obligations and settlement history

- [x] 4.1 Implement separate receivable and payable aggregates with amount, ISO currency, due date, approved amount, status, and computed outstanding balance; verify partial settlement and derived overdue behavior.
- [x] 4.2 Implement creator repasse records and immutable settlement entries linked to obligations; verify currency mismatches and settlements above the remaining balance are rejected without changing prior entries.
- [x] 4.3 Extend existing invoice records/statuses to link invoices to obligations while preserving `NotaFiscal`, `Pagamento`, and invoice-scheduling keys and legacy behavior; verify invoice status does not replace payment history.
- [x] 4.4 Map obligations, repasses, settlements, and invoice links with decimal precision, currency constraints, indexes, and delete rules; verify the EF model and relationship queries.

## 5. Migration and integrated verification

- [x] 5.1 Register required repositories and unit-of-work transaction behavior for atomic multi-record domain operations; verify dependency-injection resolution and transaction rollback tests.
- [x] 5.2 Generate and review MySQL migrations for the complete CRM model; verify migration up/down scripts and that no legacy table/key is destructively rewritten.
- [x] 5.3 Run domain and persistence tests plus `dotnet build`; verify new lifecycle, financial, and mapping tests pass and existing tests remain green.
- [ ] 5.4 Apply the migration to the development MySQL database and exercise opportunity-to-campaign, creator/delivery, contract, invoice, and partial settlement flows; verify stored relationships, balances, and legacy endpoint compatibility.

## Workflow follow-up

- Authentication, authorization, department permissions, and audit metadata/events remain deferred to future changes as explicitly requested.
- After this change is implemented and reviewed, resume `/opsx-apply crm-use-case-api`; do not mark its controller, OpenAPI, or endpoint tasks complete based only on the domain work.
- Archive this change after review and verification.
