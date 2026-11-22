# Spec Delta

## Purpose

Defines the campaign operating pages that connect campaigns to milestones, creators, timeline, summary, status commands, and creator participation history.

## ADDED Requirements

### Requirement: Campaign pages shall present operational and financial context
The frontend SHALL provide campaign list/detail/create pages with client, commercial owner, operational owner, dates, currency, fee basis, status, timeline, summary, and milestone sections.

#### Scenario: Create campaign
- **WHEN** a user submits a valid campaign form
- **THEN** the frontend SHALL call the campaign create endpoint and navigate to the created campaign detail

#### Scenario: View campaign summary
- **WHEN** a user opens the campaign summary tab
- **THEN** the frontend SHALL display receivable, payable, deliverable, creator, and status data in a CRM summary layout

### Requirement: Campaign status changes shall use explicit commands
The frontend SHALL expose start, complete, cancel, and reopen where the backend supports them. Campaign status SHALL not appear as an editable generic update field.

#### Scenario: Start campaign
- **WHEN** a user confirms start on a planned campaign
- **THEN** the frontend SHALL call the start command and refresh the campaign status

#### Scenario: Invalid campaign transition
- **WHEN** the API returns 409
- **THEN** the frontend SHALL preserve the campaign state and show the transition conflict

#### Scenario: Cancel campaign
- **WHEN** a user confirms cancellation
- **THEN** the frontend SHALL require a reason in the dialog and preserve existing campaign history in the detail view

### Requirement: Campaign milestones shall support CRUD and due-state presentation
The frontend SHALL provide milestone list/create/edit/complete interactions and shall visually distinguish overdue and completed milestones.

#### Scenario: Overdue milestone
- **WHEN** a milestone is past its deadline and incomplete
- **THEN** the frontend SHALL display an overdue marker and sort/filter it appropriately

### Requirement: Creator participation shall be campaign scoped
The frontend SHALL provide creator list/create/approve/cancel operations under each campaign and SHALL prevent duplicate active participation for the same creator.

#### Scenario: Add creator
- **WHEN** a user adds an influencer to a campaign
- **THEN** the frontend SHALL submit the creator and compensation fields and preserve the campaign scope

#### Scenario: Cancel creator participation
- **WHEN** a user cancels participation
- **THEN** the frontend SHALL show a confirmation, preserve linked contracts/deliverables/financial history, and refresh the participation status

### Requirement: Campaign timeline shall represent operational history
The frontend SHALL render a chronological campaign timeline from milestone and deliverable data with status, due date, evidence, and publication information where available.

#### Scenario: Timeline update
- **WHEN** campaign status, milestone, or deliverable data changes
- **THEN** the frontend SHALL refresh the timeline and preserve chronological ordering
