# Spec Delta

## Purpose

Disponibiliza uma API REST completa para administrar o ciclo comercial, financeiro e fiscal de uma agência que intermedeia influencers, clientes, agências de marketing e agentes.

## ADDED Requirements

### Requirement: A API SHALL expose CRUD endpoints for commercial entities
A API SHALL expose endpoints for clients, influencers, agencies, intermediaries and agents, using DTOs and HTTP status codes appropriate to each operation.

#### Scenario: Create an influencer
- **WHEN** a valid influencer creation request is sent to `POST /api/influencers`
- **THEN** the API creates the influencer and returns `201 Created` with its response DTO and resource location

#### Scenario: Read a missing commercial entity
- **WHEN** a request targets an identifier that does not exist
- **THEN** the API returns `404 Not Found` with the standard error format

#### Scenario: Update an existing agency
- **WHEN** a valid update request is sent to `PUT /api/agencies/{id}`
- **THEN** the API persists the changes and returns the updated response DTO

### Requirement: A API SHALL expose endpoints for social and banking data
A API SHALL allow management of social networks, influencer social links, banks, bank accounts and influencer bank-account associations.

#### Scenario: Add a social link to an influencer
- **WHEN** a valid social-network association is sent for an existing influencer and network
- **THEN** the API creates the association with its link and returns `201 Created`

#### Scenario: Reject a duplicate banking association
- **WHEN** an association already exists for the same influencer and bank account
- **THEN** the API rejects the request with `409 Conflict`

### Requirement: A API SHALL expose endpoints for contracts and financial operations
A API SHALL allow management of contracts, expenses and payments, including their relationships with clients, agencies, influencers and commercial entities.

#### Scenario: Create a contract
- **WHEN** a valid contract references existing client, agency-agent and influencer records
- **THEN** the API creates the contract and returns `201 Created`

#### Scenario: Create a payment with optional relationships
- **WHEN** a valid payment request contains an existing commercial entity, invoice and optionally an expense or contract
- **THEN** the API persists the payment and returns its response DTO

### Requirement: A API SHALL expose endpoints for fiscal documents
A API SHALL allow management of invoice formats, invoice statuses, invoices and invoice scheduling records.

#### Scenario: Schedule an invoice
- **WHEN** a valid scheduling request references an existing invoice and status
- **THEN** the API creates the schedule and returns `201 Created`

#### Scenario: Reject an invoice with an invalid format
- **WHEN** an invoice request references a missing invoice format
- **THEN** the API returns `400 Bad Request` or `404 Not Found` according to the validation policy and does not persist the invoice

### Requirement: A API SHALL validate input and return consistent errors
The API SHALL validate request DTOs and return a consistent problem-details-compatible error payload for invalid data, missing resources, conflicts and unexpected failures.

#### Scenario: Invalid request
- **WHEN** a request omits a required field or contains an invalid value
- **THEN** the API returns `400 Bad Request` with field-level validation details

#### Scenario: Unexpected failure
- **WHEN** an unhandled application failure occurs
- **THEN** the API returns a sanitized `500 Internal Server Error` response without exposing implementation details

### Requirement: A API SHALL support paged collection queries
Collection endpoints SHALL support pagination and return metadata sufficient for clients to navigate the result set.

#### Scenario: List influencers with pagination
- **WHEN** a client requests `GET /api/influencers?page=2&pageSize=20`
- **THEN** the API returns at most 20 records and pagination metadata including the current page and total count

### Requirement: The API SHALL publish all endpoints in Swagger
The API SHALL expose OpenAPI documentation and Swagger UI for the implemented resources and their request and response contracts.

#### Scenario: Browse API documentation
- **WHEN** a developer opens `/swagger` in the development environment
- **THEN** Swagger UI lists the available resource endpoints and their schemas
