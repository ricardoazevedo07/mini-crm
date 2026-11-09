# Spec Delta

## Purpose

Define uma camada de persistência baseada em repositories que encapsula o acesso ao `AppDbContext`, mantém as consultas testáveis e preserva a separação entre aplicação e infraestrutura.

## ADDED Requirements

### Requirement: Repositories SHALL encapsulate persistence access
Cada agregado gerenciado pela API SHALL possuir uma abstração de repository no domínio ou na aplicação e uma implementação na infraestrutura.

#### Scenario: Save an entity
- **WHEN** a handler needs to create or update an entity
- **THEN** it uses the repository abstraction and does not access `AppDbContext` directly

### Requirement: Repositories SHALL provide asynchronous operations
Repository operations that access the database SHALL be asynchronous and SHALL accept cancellation tokens where appropriate.

#### Scenario: Query a collection asynchronously
- **WHEN** a list use case requests a page of entities
- **THEN** the repository executes the query asynchronously and returns the page without blocking the request thread

### Requirement: Repositories SHALL support relationship-aware queries
Repositories SHALL provide explicit queries needed to load or validate relationships without serializing unbounded navigation graphs.

#### Scenario: Validate contract references
- **WHEN** a contract is created
- **THEN** the persistence layer verifies the referenced client, agency-agent association and influencer using targeted queries

### Requirement: Persistence operations SHALL preserve transaction consistency
Operations that modify multiple related records SHALL use a transaction boundary or a unit-of-work mechanism so partial writes are not committed.

#### Scenario: Create payment with related changes
- **WHEN** a payment operation modifies more than one persisted aggregate
- **THEN** all changes commit together or all changes are rolled back

### Requirement: Repository queries SHALL support pagination
Collection repositories SHALL provide ordered, paged queries and total-count information without materializing the complete table in memory.

#### Scenario: Page a large collection
- **WHEN** a client requests a page of records
- **THEN** the repository applies ordering and database-level pagination before materializing results
