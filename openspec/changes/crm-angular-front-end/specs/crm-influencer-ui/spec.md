# Spec Delta

## Purpose

Defines influencer profiles, social and payment profiles, curation requests, candidates, evaluation, recommendations, approvals, selection, and related filtering pages.

## ADDED Requirements

### Requirement: Influencers shall support typed CRM profile pages
The frontend SHALL provide influencer list/detail/create/edit pages based on existing commercial-entity semantics and SHALL expose business identifiers separately from route identifiers.

#### Scenario: List influencers
- **WHEN** a user opens the influencer collection
- **THEN** the frontend SHALL support pagination and filters and render profile and commercial-entity fields returned by the API

#### Scenario: View influencer relations
- **WHEN** a user opens an influencer detail
- **THEN** the frontend SHALL show social profiles, payment profiles, and linked campaign/contract information supported by the API

### Requirement: Influencer social and payment profiles shall be nested resources
The frontend SHALL provide nested social-profile and payment-profile sections with read/list behavior and typed API models.

#### Scenario: View social profiles
- **WHEN** a user opens an influencer social-profiles tab
- **THEN** the frontend SHALL list social platforms and links without leaking persistence navigation graphs

#### Scenario: View payment profile
- **WHEN** a user opens an influencer payment-profile tab
- **THEN** the frontend SHALL display banking fields without introducing authentication or role checks

### Requirement: Curation requests shall have a dedicated page
The frontend SHALL provide curation-request list/detail/create and candidate pages with campaign briefing and request status.

#### Scenario: Create curation request
- **WHEN** a user submits campaign and briefing data
- **THEN** the frontend SHALL call the curation-request endpoint and navigate to the new request detail

#### Scenario: Request candidates
- **WHEN** a user opens a curation request
- **THEN** the frontend SHALL list candidate records scoped to that request

### Requirement: Curation candidates shall expose explicit lifecycle commands
The frontend SHALL provide candidate evaluation, recommendation, submission, approve, reject, and select actions only where the API contract is implemented.

#### Scenario: Create unique candidate
- **WHEN** a user adds an influencer to a curation request
- **THEN** the frontend SHALL prevent a duplicate request/influencer pair and display API conflict feedback

#### Scenario: Advance candidate
- **WHEN** a user invokes an implemented candidate command
- **THEN** the frontend SHALL refresh candidate status and preserve the related request and influencer IDs
