# Spec Delta

## Purpose

Defines a stable, versioned HTTP contract for the CRM business use cases so backend controllers and API consumers can implement the same resource routes, request/response shapes, lifecycle commands, and error behavior.

## ADDED Requirements

### Requirement: CRM endpoints use a versioned typed HTTP contract
The system SHALL expose CRM endpoints under `/api/v1` using dedicated request and response contracts rather than serializing persistence entities or accepting arbitrary JSON payloads.

#### Scenario: Create a CRM resource
- **WHEN** a caller submits a valid request to a CRM create endpoint
- **THEN** the API SHALL validate the typed request, persist through the corresponding use case, return `201 Created` with a response DTO and a `Location` for the created resource

#### Scenario: Preserve legacy endpoint compatibility
- **WHEN** an existing client calls an unversioned `/api/...` route
- **THEN** the API SHALL preserve the current route contract during this additive change and SHALL NOT silently redirect it to a new DTO or resource shape

#### Scenario: Protect technical and business identifiers
- **WHEN** a caller creates or updates a resource
- **THEN** the API SHALL assign or preserve server-owned identifiers and SHALL reject requests that attempt to change a technical identifier

### Requirement: Authentication and authorization are deferred
The `/api/v1` endpoints defined by this change SHALL NOT require authentication or apply authorization policies, role checks, or sensitive-field access checks. Implementing authentication, authorization, and verified user attribution is deferred to a separate future change. This requirement applies to all endpoints in this change, including financial and banking endpoints.

#### Scenario: Call an endpoint without credentials
- **WHEN** a caller invokes an otherwise valid `/api/v1` endpoint without credentials
- **THEN** the API SHALL process the request according to the resource contract without requiring an authenticated identity or role

### Requirement: CRM endpoints reuse compatible legacy records without exposing the generic CRUD contract
The system SHALL map CRM resources with an existing semantically compatible agency-management record to that record and preserve its identity and relationships. A legacy `/api/...` route SHALL NOT be considered implementation of its typed `/api/v1` counterpart. The system SHALL NOT create duplicate persistence records for clients, influencers, contracts, payments, invoices, or other existing records solely to support the versioned API.

#### Scenario: Expose an existing record through a typed CRM endpoint
- **WHEN** a CRM use case needs a record already represented by a compatible legacy entity
- **THEN** the `/api/v1` endpoint SHALL use a typed request/response and application use case over the existing record and preserve its established identity, while the unversioned route remains compatible

#### Scenario: Defer a CRM concept without a domain record
- **WHEN** a CRM endpoint depends on a concept with no domain and persistence record, such as an opportunity, campaign, creator participation, delivery, receivable, or payable
- **THEN** implementation SHALL wait for that concept's domain and persistence contract and SHALL NOT substitute unrelated legacy records or arbitrary JSON

### Requirement: Customer and sales resources support CRUD and search
The system SHALL provide versioned customer, customer-contact, lead, opportunity, proposal, and proposal-item endpoints with filters and CRUD operations allowed by each resource's lifecycle.

#### Scenario: Create and retrieve a customer
- **WHEN** a caller creates a customer or searches by identifier, legal name, or document number
- **THEN** the API SHALL return a typed customer response and prevent duplicate active legal identities

#### Scenario: Manage leads and opportunities
- **WHEN** a caller creates, reads, updates, or lists a lead or opportunity with supported filters
- **THEN** the API SHALL return the resource's current lifecycle state and owner and SHALL NOT change lifecycle status through generic resource update

#### Scenario: Manage a proposal and its items
- **WHEN** a caller creates or edits a draft proposal and its items
- **THEN** the API SHALL return a typed proposal response with calculated totals and SHALL reject edits to an approved proposal except through a new version

### Requirement: Campaign resources expose operational CRUD and relations
The system SHALL provide versioned campaign, campaign-milestone, campaign-creator, and campaign-summary endpoints with typed filters and consistent typed responses.

#### Scenario: Create or retrieve a campaign
- **WHEN** a caller creates a campaign or requests its details, list, timeline, or summary
- **THEN** the API SHALL return the campaign's client, commercial and operational owners, dates, currency, fee data, and associated records defined by the resource contract

#### Scenario: Manage creator participation
- **WHEN** a caller adds, retrieves, updates, or cancels a creator participation
- **THEN** the API SHALL preserve a distinct participation per creator and campaign and SHALL retain linked contract, delivery, and payment history when the participation is cancelled

#### Scenario: Prevent arbitrary campaign state changes
- **WHEN** a caller updates campaign metadata
- **THEN** the API SHALL leave campaign status unchanged unless the caller invokes an explicit lifecycle command

### Requirement: Influencer and curation resources support typed operations
The system SHALL expose versioned influencer, social-profile, niche, metric-snapshot, payment-profile, curation-request, and curation-candidate endpoints with typed request and response contracts.

#### Scenario: Manage influencer profiles and metrics
- **WHEN** a caller creates, retrieves, updates, or lists influencer profiles and metric snapshots
- **THEN** the API SHALL return the influencer fields and historical metric snapshots defined by the response contract

#### Scenario: Manage curation candidates
- **WHEN** a caller creates, evaluates, recommends, submits, approves, rejects, or selects a candidate
- **THEN** the API SHALL validate the linked request, influencer, and social profile and SHALL expose each state change through a dedicated operation with its result

#### Scenario: Retrieve an influencer payment profile
- **WHEN** a caller requests an influencer payment profile without credentials
- **THEN** the API SHALL return the typed payment-profile response without applying authentication or authorization checks

### Requirement: Contracts and deliverables have explicit lifecycle endpoints
The system SHALL provide typed endpoints for contracts and their documents, clauses and approvals, and for campaign deliverables, approvals and revisions.

#### Scenario: Manage a contract
- **WHEN** a caller creates, reads, updates, or lists a contract or its documents
- **THEN** the API SHALL validate campaign and party references and SHALL preserve signed versions as immutable records

#### Scenario: Advance a contract lifecycle
- **WHEN** a caller requests review, approval, signature, completion, cancellation, or amendment
- **THEN** the API SHALL apply only an allowed transition, distinguish approval from signature, and require a cancellation reason when applicable

#### Scenario: Track delivery review and publication
- **WHEN** a caller submits, requests revision, registers a revision, approves, rejects, schedules, publishes, or completes a deliverable
- **THEN** the API SHALL retain review history, evidence, and reasons and SHALL require publication evidence/time before marking it published

### Requirement: Financial resources preserve settlement and approval history
The system SHALL provide typed endpoints for accounts receivable/payable, their individual receipts/payments, financial approvals, transactions, reconciliation, and campaign financial summaries.

#### Scenario: Create and query a financial obligation
- **WHEN** a caller creates or queries a receivable or payable without credentials
- **THEN** the API SHALL require campaign, amount, ISO currency, due date, and applicable party references and SHALL return the outstanding balance and status

#### Scenario: Record a receipt or payment
- **WHEN** a caller records a settlement without credentials
- **THEN** the API SHALL append an individual settlement record, reject a currency mismatch or amount above the remaining balance, and update the derived balance without deleting prior entries

#### Scenario: Approve or reconcile a financial operation
- **WHEN** a caller approves, rejects, or reconciles a financial operation without credentials
- **THEN** the API SHALL record the decision, timestamp, and comments; verified actor attribution and permission-based segregation are deferred to the future authentication and authorization change

#### Scenario: Retrieve financial and banking data
- **WHEN** a caller requests an account, transaction, campaign financial summary, or payment profile without credentials
- **THEN** the API SHALL return the typed response defined for that resource without applying authorization checks

### Requirement: Business transitions are explicit commands
The system SHALL expose dedicated operations for lifecycle transitions rather than allowing callers to assign arbitrary status values in generic create or update payloads.

#### Scenario: Convert a won opportunity
- **WHEN** a caller converts a won opportunity to a campaign
- **THEN** the API SHALL create at most one campaign, preserve source references, and return the created campaign resource

#### Scenario: Reject an invalid transition
- **WHEN** a caller requests a transition that is not valid for the resource's current state
- **THEN** the API SHALL return `409 Conflict` with a stable error code and leave the resource and its related records unchanged

#### Scenario: Preserve history on cancellation or correction
- **WHEN** a caller cancels an active assignment, contract, campaign, or financial operation or corrects a signed/settled record
- **THEN** the API SHALL require the relevant reason and preserve history using cancellation, amendment, or compensating entries rather than physical deletion

### Requirement: CRM collections support deterministic pagination and filtering
The system SHALL support query-string pagination and resource-specific filters on collection endpoints and SHALL return stable pagination metadata.

#### Scenario: List a filtered page
- **WHEN** a caller requests a collection with `page`, `pageSize`, supported filters, and a sort order
- **THEN** the API SHALL return only the requested page, the applied page values, total count, total pages, and items ordered deterministically

#### Scenario: Reject invalid query values
- **WHEN** a caller submits invalid pagination or filter values
- **THEN** the API SHALL return `400 Bad Request` with field-level validation details and SHALL NOT silently substitute a success-shaped response

### Requirement: API failures use a consistent Problem Details contract
The system SHALL return consistent sanitized error responses for validation failures, missing resources, conflicts, and unexpected errors. Authentication and authorization failures are not part of this change because the endpoints do not require credentials or apply access policies.

#### Scenario: Report a validation or missing-resource error
- **WHEN** a request contains invalid fields or references a resource that does not exist
- **THEN** the API SHALL return respectively `400 Bad Request` or `404 Not Found` with a Problem Details response and stable error code

#### Scenario: Report a business conflict
- **WHEN** a caller requests a conflicting lifecycle or uniqueness operation
- **THEN** the API SHALL return `409 Conflict` with a sanitized Problem Details response

#### Scenario: Handle an unexpected error
- **WHEN** an unhandled server error occurs
- **THEN** the API SHALL return a sanitized `500 Internal Server Error` Problem Details response without stack traces, SQL details, or credentials

### Requirement: OpenAPI documents the actual controller contracts
The system SHALL publish discoverable OpenAPI documentation for every implemented versioned CRM endpoint, including its request, response, query parameters, and possible error responses. The OpenAPI document SHALL NOT declare authentication schemes or authorization requirements for this change.

#### Scenario: Discover a controller contract
- **WHEN** a developer retrieves the OpenAPI document for `/api/v1`
- **THEN** the document SHALL describe the implemented routes and schemas consistently with the behavior served by the controllers
