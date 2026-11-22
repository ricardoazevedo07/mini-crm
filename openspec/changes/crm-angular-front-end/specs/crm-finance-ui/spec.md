# Spec Delta

## Purpose

Defines the financial UI for campaign receivables, payables, settlement entries, repasses, balances, financial transactions, reconciliation, and campaign summaries.

## ADDED Requirements

### Requirement: Accounts receivable and payable shall have dedicated screens
The frontend SHALL provide separate collection/detail/create screens for receivables and payables and SHALL display amount, approved amount, currency, due date, status, and outstanding balance.

#### Scenario: Create receivable
- **WHEN** a user creates a receivable
- **THEN** the frontend SHALL require campaign, amount, ISO currency, due date, and approved amount when applicable

#### Scenario: Create payable
- **WHEN** a user creates a payable
- **THEN** the frontend SHALL require campaign, party or participation scope where applicable, amount, currency, and due date

### Requirement: Settlement actions shall preserve immutable history
The frontend SHALL append receipt/payment entries and SHALL display settlement history, partial status, remaining balance, and over-balance conflicts.

#### Scenario: Record partial settlement
- **WHEN** a settlement is less than the remaining balance
- **THEN** the frontend SHALL update the page to partial status and remaining balance

#### Scenario: Reject invalid settlement
- **WHEN** currency differs or value exceeds the remaining balance
- **THEN** the frontend SHALL show 400 or 409 feedback and retain the settlement form

### Requirement: Financial transactions shall support a dedicated ledger view
The frontend SHALL provide transaction list/detail/create views for receivable, payable, and repasse settlement entries.

#### Scenario: Reconcile transaction
- **WHEN** a reconciliation command is available
- **THEN** the frontend SHALL submit the transaction reference and refresh reconciliation status

#### Scenario: View immutable entry
- **WHEN** a settled transaction is displayed
- **THEN** the frontend SHALL provide read-only history and shall not offer deletion or balance overwrite controls

### Requirement: Campaign financial summary shall use campaign scope
The frontend SHALL present receivables, payables, repasses, transactions, and outstanding balances for a single campaign.

#### Scenario: Open campaign finance tab
- **WHEN** a user opens finance from campaign detail
- **THEN** the frontend SHALL call campaign-scoped summaries and show currencies and balances consistently

### Requirement: Banking and financial data shall remain accessible without frontend authorization
The frontend SHALL not add login checks, role guards, or permission projections to financial or banking screens during this change.

#### Scenario: Finance route opens
- **WHEN** an unauthenticated user navigates to a financial route
- **THEN** the frontend SHALL show the route normally without redirect or 401 UI
