# Spec Delta

## Purpose

Defines contract and deliverable operation screens, including legal lifecycle, version review, approval/signature commands, revisions, evidence, scheduling, publication, and completion.

## ADDED Requirements

### Requirement: Contracts shall support a dedicated list and detail experience
The frontend SHALL provide contract list/detail/create pages with party references, campaign/creator scope, status, version, signature, and cancellation context.

#### Scenario: Open contract
- **WHEN** a user opens a contract detail
- **THEN** the frontend SHALL show its parties and lifecycle while excluding EF navigation graph fields

#### Scenario: Create contract
- **WHEN** a user submits valid campaign/party references
- **THEN** the frontend SHALL create the contract and navigate to its detail

### Requirement: Signed contract versions shall be presented as immutable history
The frontend SHALL disable edit controls for signed versions and show a new amendment/version workflow.

#### Scenario: Signed contract displayed
- **WHEN** a contract status is signed
- **THEN** the frontend SHALL mark its content read-only and provide amendment controls where supported

#### Scenario: Amendment request succeeds
- **WHEN** the API creates a new contract version
- **THEN** the frontend SHALL show the prior version snapshot and the current revised version

### Requirement: Contract lifecycle commands shall be explicit
The frontend SHALL provide review, send-for-signature, register-signature, cancel, and amendment actions as dedicated controls.

#### Scenario: Register signature
- **WHEN** a user registers a signature
- **THEN** the frontend SHALL update status and signature date and SHALL not allow direct status editing

#### Scenario: Cancel contract
- **WHEN** a user cancels a contract
- **THEN** the frontend SHALL require a cancellation reason and preserve prior lifecycle history

### Requirement: Deliverables shall support submission and review workflows
The frontend SHALL provide deliverable list/detail/create pages and submit, approve, reject, revise, schedule, publish, and complete actions.

#### Scenario: Submit deliverable
- **WHEN** a user submits a deliverable for approval
- **THEN** the frontend SHALL call the submit command and refresh its status and review history

#### Scenario: Reject deliverable
- **WHEN** a user rejects a deliverable
- **THEN** the frontend SHALL require a rejection reason before submission

#### Scenario: Request revision
- **WHEN** a user requests revision
- **THEN** the frontend SHALL preserve the prior review and create a revision entry with comments and timestamps

### Requirement: Published deliverables shall require evidence
The frontend SHALL prevent a publish action from succeeding without evidence and SHALL display publication timestamp and evidence link.

#### Scenario: Missing publication evidence
- **WHEN** a user attempts to publish with an empty evidence field
- **THEN** the frontend SHALL show a field-level validation error and not call the API

#### Scenario: Publish succeeds
- **WHEN** the API accepts evidence and publication time
- **THEN** the frontend SHALL show the publish status and evidence in the deliverable timeline
