# Spec Delta

## Purpose

Defines the customer, contact, lead, opportunity, proposal, and proposal-item pages that cover the commercial workflow from qualification through campaign conversion.

## ADDED Requirements

### Requirement: Customers shall support list, detail, create, update, archive, and contact operations
The frontend SHALL provide customer list/detail/create/edit/archive pages and SHALL present active state, CNPJ, name, segment, language preference, and contacts.

#### Scenario: Search customers
- **WHEN** a user filters customers by name, CNPJ, or active state
- **THEN** the frontend SHALL call the customer API with supported query values and render deterministic pagination

#### Scenario: Create duplicate customer
- **WHEN** the API reports that an active legal identity already exists
- **THEN** the frontend SHALL show the conflict and keep the entered form values

#### Scenario: Archive customer
- **WHEN** a user confirms customer archival and the API succeeds
- **THEN** the frontend SHALL update the customer status and retain the record in its detail view

### Requirement: Customer contacts shall be managed as a nested resource
The frontend SHALL provide add, edit, list, and detail interactions for contacts nested under a customer.

#### Scenario: Mark primary contact
- **WHEN** a user creates or updates a contact and selects it as primary
- **THEN** the frontend SHALL submit the nested contact payload and visually identify the primary contact

#### Scenario: Contact validation fails
- **WHEN** name or email is missing
- **THEN** the frontend SHALL prevent submission and display field-level errors

### Requirement: Leads shall support CRUD and lifecycle commands
The frontend SHALL provide lead list/detail/create/edit pages and qualify/lose command actions.

#### Scenario: Qualify lead
- **WHEN** a user invokes qualify on a valid lead
- **THEN** the frontend SHALL call the qualify command and refresh the lead status

#### Scenario: Lose lead
- **WHEN** a user invokes lose
- **THEN** the frontend SHALL require or accept a loss reason when the API contract supports it and preserve the lead history

### Requirement: Opportunities shall expose win, loss, and campaign conversion commands
The frontend SHALL render opportunity list/detail/create views and dedicated win, lose, and convert-to-campaign actions without exposing status as an editable metadata field.

#### Scenario: Win opportunity
- **WHEN** a user requests the win action
- **THEN** the frontend SHALL invoke the win command and refresh the opportunity lifecycle state

#### Scenario: Convert won opportunity
- **WHEN** a user confirms conversion of a won opportunity
- **THEN** the frontend SHALL call the conversion command and navigate to the created campaign resource

#### Scenario: Reject invalid transition
- **WHEN** the API returns 409 for an invalid opportunity transition
- **THEN** the frontend SHALL keep the current state and display the conflict reason

### Requirement: Proposals and items shall protect approved versions
The frontend SHALL provide proposal list/detail/create, send/approve/reject commands, proposal items, and version presentation. It SHALL prevent editing a proposal or item after approval in place.

#### Scenario: Edit draft proposal
- **WHEN** a user edits a proposal not marked approved
- **THEN** the frontend SHALL submit editable metadata and item changes

#### Scenario: Approved proposal edit attempted
- **WHEN** the API rejects an edit to an approved proposal
- **THEN** the frontend SHALL disable editing and offer the version or new-amendment workflow defined by the API
