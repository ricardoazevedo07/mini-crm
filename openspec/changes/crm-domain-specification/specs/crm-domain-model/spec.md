# Spec Delta

## Purpose

This capability defines the business domain required for the Remo CRM so that the system can manage client acquisition, campaign lifecycle, financial operations, and role-based visibility according to the use-case.

## ADDED Requirements

### Requirement: Client and opportunity lifecycle are modeled explicitly
The system SHALL represent client and opportunity records as first-class business entities to support the commercial stage before a campaign is created.

#### Scenario: Register a new client and opportunity
- **WHEN** a commercial user registers a new client and the first contact is captured
- **THEN** the system SHALL persist the client identity, commercial owner, lead stage, and intake details as a distinct opportunity entry tied to that client

#### Scenario: Prevent duplicate client registration
- **WHEN** the same client is registered again with the same legal identity
- **THEN** the system SHALL flag the duplicate and prevent the creation of a second active client record without an override workflow

### Requirement: Campaign and creator records hold all operational data needed by the CRM
The system SHALL allow each campaign to be modeled as a single operational record with related creator participation, delivery, and status tracking.

#### Scenario: Create a campaign from a closed opportunity
- **WHEN** the commercial stage is converted into a campaign
- **THEN** the system SHALL create a campaign record with the associated owner, front, currency, fee structure, and related creator participation entries

#### Scenario: Track creator-level participation
- **WHEN** a campaign includes multiple creators or payment portions
- **THEN** the system SHALL model each creator participation as a separate record that retains status, payment terms, and repass information

### Requirement: Financial and legal records are governed by business phase and status
The system SHALL represent the contract, invoice, repayment, and reconciliation flow as distinct records that align with the campaign lifecycle and department responsibilities.

#### Scenario: Record contract and invoice obligations
- **WHEN** the legal and financial stages validate a campaign
- **THEN** the system SHALL create or link contract, invoice, and payment records that are associated to the same campaign and can be tracked independently by status

#### Scenario: Support late and partial payment tracking
- **WHEN** a payment or invoice is overdue or only partially settled
- **THEN** the system SHALL expose a status and due-date model that allows the financial team and management to identify delays and account balances

### Requirement: Role-based visibility and audit metadata are part of the domain contract
The system SHALL enforce that each user or role sees only the data and values allowed by the business rules, and SHALL retain who changed each record and when.

#### Scenario: Commercial user works with the campaign record
- **WHEN** a commercial user accesses the campaign lifecycle
- **THEN** the system SHALL allow them to manage sales, briefing, and campaign progression while following the permissions defined for their role

#### Scenario: Finance and management see sensitive values
- **WHEN** a financial user or manager opens a campaign with financial data
- **THEN** the system SHALL provide access to balances, totals, fees, and repass values according to the allowed visibility rules

#### Scenario: Audit the domain history
- **WHEN** a record is created or modified
- **THEN** the system SHALL retain the responsible user and timestamp so that the operations can be audited across sectors and departments

### Requirement: Client records support CRUD operations with validation
The system SHALL allow authorized users to create, retrieve, update, and delete client records while enforcing data integrity and commercial continuity.

#### Scenario: Create a new client
- **WHEN** a commercial user submits a valid client name, CNPJ, and contact details
- **THEN** the system SHALL persist the client record and assign a unique identifier for future operations

#### Scenario: Retrieve client details
- **WHEN** an authorized user requests a specific client by identifier or searches by name or CNPJ
- **THEN** the system SHALL return the client record with all associated opportunities and campaigns according to the user's role and visibility permissions

#### Scenario: Update client contact information
- **WHEN** a commercial user modifies client name, CNPJ, segment, or language preference
- **THEN** the system SHALL update the record and log the change with the user and timestamp for audit compliance

#### Scenario: Prevent deletion of client with active campaigns
- **WHEN** a user attempts to delete a client that has open opportunities or campaigns
- **THEN** the system SHALL reject the deletion and indicate that the client must be archived instead

### Requirement: Opportunity records support CRUD operations and status transitions
The system SHALL allow commercial users to manage opportunities through creation, viewing, modification, and conversion stages.

#### Scenario: Create a new opportunity
- **WHEN** a commercial user submits a client reference, briefing, estimated budget, and owns the lead
- **THEN** the system SHALL persist the opportunity record with an initial status and link it to the client

#### Scenario: Retrieve and filter opportunities
- **WHEN** a commercial user searches for opportunities by client, stage, owner, or date range
- **THEN** the system SHALL return matching records with their current status and associated client data

#### Scenario: Update opportunity details
- **WHEN** a commercial user modifies the briefing, budget, or stage of an opportunity
- **THEN** the system SHALL update the record and log the change without affecting related campaigns or contracts

#### Scenario: Convert opportunity to campaign
- **WHEN** a commercial user confirms an opportunity as won and prepares for execution
- **THEN** the system SHALL create a campaign record from the opportunity, preserve the client and commercial owner, and transition the opportunity status to closed

### Requirement: Campaign records support full CRUD with multi-creator coordination
The system SHALL allow campaigns to be created, managed, and tracked across all execution phases with related creator assignments and deliverables.

#### Scenario: Create a campaign
- **WHEN** an opportunity is converted or a commercial user initiates a new campaign with client, front, currency, and fee details
- **THEN** the system SHALL persist the campaign and initialize status, ownership, and financial tracking fields

#### Scenario: Retrieve campaign with all related data
- **WHEN** an authorized user requests a campaign
- **THEN** the system SHALL return the campaign record, associated creators, deliverables, contracts, invoices, and payments according to role-based visibility rules

#### Scenario: Update campaign metadata and status
- **WHEN** a campaign owner or authorized sector modifies status, timeline, budget, or delivery targets
- **THEN** the system SHALL update the record and restrict certain edits based on the current stage and user role

#### Scenario: Archive or close campaign
- **WHEN** the operational close checklist is complete and the financial reconciliation has been completed
- **THEN** the system SHALL mark the campaign as closed while preserving historical data and payments, and SHALL surface any reconciled but outstanding balances in the close summary; reconciliation SHALL NOT imply that every obligation has been paid

### Requirement: Campaign Creator records track individual participation and payments
The system SHALL allow tracking of each creator's role, compensation, and status within a campaign.

#### Scenario: Assign creator to campaign
- **WHEN** a commercial or curation user adds a creator to a campaign with fee, payment terms, and delivery scope
- **THEN** the system SHALL create a campaign-creator link record and initialize tracking fields for status and payment

#### Scenario: Retrieve creator assignments for a campaign
- **WHEN** authorized user views a campaign
- **THEN** the system SHALL list all assigned creators with their fee, payment status, and delivery timeline according to visibility rules

#### Scenario: Update creator compensation or status
- **WHEN** a commercial or financial user modifies the fee, payment terms, or approval status for a creator on a campaign
- **THEN** the system SHALL update the record and log the change without affecting other creators on the same campaign

#### Scenario: Remove creator from campaign
- **WHEN** a creator assignment is cancelled or a creator withdraws
- **THEN** the system SHALL mark the assignment as inactive while preserving payment history and obligations already created

### Requirement: Delivery records track execution and approval across the content lifecycle
The system SHALL enable the atendimento sector to record delivery milestones, approvals, and evidence for each campaign.

#### Scenario: Create delivery milestone
- **WHEN** atendimento registers a planned delivery with due date, description, and expected location or link
- **THEN** the system SHALL persist the delivery record linked to the campaign and initialize status as pending

#### Scenario: Retrieve deliveries for a campaign
- **WHEN** authorized users view campaign deliveries
- **THEN** the system SHALL return all milestones with status, due date, approval state, and evidence links according to role permissions

#### Scenario: Update delivery status and add evidence
- **WHEN** atendimento marks a delivery complete and uploads evidence (link, file, screenshot)
- **THEN** the system SHALL update the delivery record status and log the evidence with timestamp

#### Scenario: Handle delivery rejection
- **WHEN** brand or internal reviewer rejects a delivery
- **THEN** the system SHALL mark it as rejected, retain the rejection reason, and allow atendimento to resubmit a revised version

### Requirement: Contract records support lifecycle management and legal compliance
The system SHALL allow juridico to manage contracts, track signatures, and link obligations to campaigns.

#### Scenario: Create contract record
- **WHEN** juridico registers a contract type, associated campaign, parties, and links the signed agreement
- **THEN** the system SHALL persist the contract record with status, dates, and audit metadata

#### Scenario: Retrieve contracts by campaign or party
- **WHEN** authorized user searches contracts
- **THEN** the system SHALL return matching contracts with status, signature dates, and associated obligations

#### Scenario: Update contract status
- **WHEN** juridico marks a contract as signed, active, or terminated
- **THEN** the system SHALL update the record and trigger related financial workflows if applicable

#### Scenario: Link contract to campaign and creators
- **WHEN** a contract is associated to a campaign and creators
- **THEN** the system SHALL create the relationship so that financial and audit queries can trace obligations back to source

### Requirement: Financial records track receivables, payables, and repayments
The system SHALL allow financeiro to manage accounts receivable, accounts payable, invoices, and repayments with status and reconciliation tracking.

#### Scenario: Create account receivable
- **WHEN** financeiro registers a client invoice with amount, due date, and campaign reference
- **THEN** the system SHALL persist the receivable with status and link to the campaign and client

#### Scenario: Retrieve receivables and payables
- **WHEN** authorized financeiro or gestor views financial records
- **THEN** the system SHALL return accounts with status, due date, overdue flag, and associated creator or client data according to visibility rules

#### Scenario: Record payment received
- **WHEN** financeiro confirms a client payment with date and amount
- **THEN** the system SHALL update the receivable status to paid, log the receipt, and update account balance

#### Scenario: Create account payable
- **WHEN** financeiro registers a creator invoice or expense with amount, due date, and campaign reference
- **THEN** the system SHALL persist the payable record and link to the campaign and creator

#### Scenario: Record payment issued
- **WHEN** financeiro confirms a payment to creator or vendor with date and amount
- **THEN** the system SHALL update the payable status to paid, log the disbursement, and update account balance

#### Scenario: Track overdue payments
- **WHEN** a payment is not settled by its due date
- **THEN** the system SHALL mark it as overdue and expose it in alert and reporting queries for management attention

### Requirement: Invoice records track billing and status for financial operations
The system SHALL enable financeiro to create, track, and reconcile invoices with status and payment history.

#### Scenario: Create invoice record
- **WHEN** financeiro registers an invoice for client or creator with amount, format, and campaign reference
- **THEN** the system SHALL persist the invoice and initialize status and audit fields

#### Scenario: Retrieve invoices by campaign or party
- **WHEN** authorized user queries invoices
- **THEN** the system SHALL return matching records with status, amount, and associated client or creator data

#### Scenario: Update invoice status
- **WHEN** financeiro marks an invoice as sent, received, or cancelled
- **THEN** the system SHALL update the status and trigger related payment workflows if applicable

#### Scenario: Link invoice to payments
- **WHEN** a payment is recorded against an invoice
- **THEN** the system SHALL associate the payment to the invoice and update reconciliation status

### Requirement: Domain aggregates preserve independent lifecycles and references
The system SHALL represent client, opportunity, campaign, creator participation, delivery, contract, invoice, and financial obligations as identifiable records with explicit ownership of their lifecycle data. Cross-record workflows SHALL preserve references and SHALL NOT silently rewrite another record's history.

#### Scenario: Convert a won opportunity into a campaign
- **WHEN** a won opportunity is converted
- **THEN** the system SHALL create at most one campaign linked to the source opportunity, client, and commercial owner, and preserve the original opportunity history

#### Scenario: Track creator participation and delivery independently
- **WHEN** a campaign has multiple creators or deliveries
- **THEN** the system SHALL retain distinct participation and delivery records so changing one creator's fee, status, or deliverables does not overwrite another creator's data

#### Scenario: Preserve signed contracts and settled financial history
- **WHEN** a contract is signed or a payment/receipt is settled
- **THEN** the system SHALL preserve the signed/settled record and require a version, cancellation, or compensating financial entry for a correction

### Requirement: Department permissions define record and sensitive-field access
The system SHALL authorize read, create, update, approve, and sensitive-field access independently for each user according to assigned department, role, record responsibility, and approved policy. Client-side field hiding SHALL NOT be considered authorization.

#### Scenario: Apply least-privilege department access
- **WHEN** a user from commercial, curadoria, planejamento, atendimento, jurídico, financeiro, or gestão requests a CRM record
- **THEN** the system SHALL allow only the actions and fields assigned to that department by the approved permission matrix, and SHALL deny unauthorized access at the API boundary

#### Scenario: Separate editing from approval
- **WHEN** a user can edit a business record but lacks its approval permission
- **THEN** the system SHALL reject the approval action and leave the approval state unchanged

### Requirement: Lifecycle statuses and transitions are explicit
The system SHALL use a defined lifecycle for opportunities, campaigns, deliveries, contracts, invoices, receivables, payables, and creator repasses, and SHALL reject arbitrary status changes that bypass allowed domain transitions.

#### Scenario: Reject an invalid lifecycle transition
- **WHEN** a user requests a status transition that is not allowed from the record's current state
- **THEN** the system SHALL leave the record unchanged and return a business-rule conflict with the current and requested states

#### Scenario: Identify overdue obligations without losing settlement state
- **WHEN** an invoice, receivable, payable, or repasse passes its due date with an outstanding balance
- **THEN** the system SHALL expose it as overdue while retaining its partial-settlement status and payment history

#### Scenario: Require reasons and evidence for terminal transitions
- **WHEN** an opportunity is lost, a campaign or contract is cancelled, or a delivery is rejected
- **THEN** the system SHALL require the relevant reason and retain it in the audit history

### Requirement: Core domain records include auditable ownership metadata
The system SHALL retain the responsible actor and UTC timestamps for creation and each update of core client, opportunity, campaign, creator participation, delivery, contract, invoice, and financial records.

#### Scenario: Record creation and modification metadata
- **WHEN** an authorized user creates or updates a core record
- **THEN** the system SHALL set `CriadoPor`/`DataCriacao` and `AtualizadoPor`/`DataAtualizacao` as applicable and append an audit event identifying the actor, record, action, and changed fields

#### Scenario: Protect sensitive values in audit history
- **WHEN** an audit event includes financial or personal data
- **THEN** the system SHALL omit or mask bank credentials and sensitive personal identifiers and prevent ordinary users from editing or deleting the audit event

### Requirement: Financial amounts use explicit currency and preserve balance invariants
The system SHALL store each financial amount with an ISO 4217 currency and decimal precision suitable for currency, and SHALL calculate balances from recorded obligations and settlements.

#### Scenario: Prevent cross-currency or over-balance settlement
- **WHEN** a user records a payment in a different currency from its obligation or for more than its remaining balance
- **THEN** the system SHALL reject the settlement unless an explicit approved conversion is supplied, and SHALL leave the obligation and payment history unchanged

#### Scenario: Record approved amounts and due dates
- **WHEN** a financial obligation or approval is created
- **THEN** the system SHALL retain its amount, currency, due date, approved amount where applicable, and the responsible approver, and SHALL expose outstanding balance as obligation amount minus valid settlements; campaign and creator participation records SHALL retain approved totals, fee basis, negotiated amounts, and payment terms

### Requirement: Management summaries preserve department visibility
The system SHALL provide operational and financial summaries across clients, opportunities, campaigns, deliveries, contracts, invoices, and obligations while applying the same role and sensitive-field permissions as record-level access.

#### Scenario: Review campaign operational and financial status
- **WHEN** an authorized manager requests a campaign summary
- **THEN** the system SHALL show lifecycle status, owners, pending deliveries, contract state, receivables, payables, and outstanding balances allowed for that manager, without exposing restricted personal or bank details

#### Scenario: Report outstanding and overdue obligations
- **WHEN** an authorized finance or management user requests an overdue-items summary
- **THEN** the system SHALL identify obligations with a positive outstanding balance past due and include their campaign, responsible owner, due date, currency, and balance within the user's permitted visibility
