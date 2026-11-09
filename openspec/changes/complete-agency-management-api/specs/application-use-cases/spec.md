# Spec Delta

## Purpose

Organiza as operações da aplicação em casos de uso independentes, testáveis e consistentes, separando o protocolo HTTP das regras de negócio e da persistência.

## ADDED Requirements

### Requirement: Each use case SHALL have a handler, request and response
Cada operação de negócio exposta pela API SHALL ser representada por uma pasta própria contendo um handler, um request e um response com o prefixo do caso de uso.

#### Scenario: Create influencer use case
- **WHEN** the create-influencer operation is implemented
- **THEN** it contains `CreateInfluencerHandler`, `CreateInfluencerRequest` and `CreateInfluencerResponse` in the corresponding use-case folder

### Requirement: Handlers SHALL coordinate application behavior
Handlers SHALL validate or invoke validation for their input, call the required service or repository abstractions, and return a typed result without depending on controller-specific concerns.

#### Scenario: Handler creates an entity
- **WHEN** a valid request reaches a handler
- **THEN** the handler invokes the application behavior, persists the result through abstractions and returns the response model

#### Scenario: Handler handles missing dependency
- **WHEN** a handler cannot find a referenced entity
- **THEN** it returns an application result that the controller can translate into `404 Not Found`

### Requirement: Controllers SHALL remain thin
Controllers SHALL translate HTTP requests into use-case requests, invoke handlers and translate their results into HTTP responses without containing persistence queries or business rules.

#### Scenario: Controller delegates creation
- **WHEN** an HTTP create request arrives
- **THEN** the controller delegates to the corresponding handler and does not access `AppDbContext` directly

### Requirement: Use cases SHALL not expose EF Core entities as API contracts
Use cases SHALL map domain entities to response DTOs and accept dedicated request DTOs so EF Core navigation properties and persistence details are not exposed directly.

#### Scenario: Response hides persistence details
- **WHEN** a resource is returned by a use case
- **THEN** the response contains only the public API fields and does not serialize EF Core navigation graphs unintentionally
