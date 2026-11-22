# Spec Delta

## Purpose

Defines task and notification pages for operational assignment, status commands, deadlines, and read state without user-role or identity projection.

## ADDED Requirements

### Requirement: Tasks shall support list, detail, create, assignment, and lifecycle commands
The frontend SHALL provide task collection/detail/create pages and assign, start, complete, and cancel actions.

#### Scenario: Assign task
- **WHEN** a user selects a responsible person and confirms assignment
- **THEN** the frontend SHALL call the assign command and refresh responsible-party and status fields

#### Scenario: Complete task
- **WHEN** a user completes a task
- **THEN** the frontend SHALL update status and remove it from incomplete dashboard lists where appropriate

#### Scenario: Cancel task
- **WHEN** a user cancels a task
- **THEN** the frontend SHALL retain the task and show canceled status instead of deleting history

### Requirement: Task deadlines shall be visually distinguishable
The frontend SHALL show due status using neutral, due-soon, overdue, and completed visual states.

#### Scenario: Overdue task
- **WHEN** a pending task deadline is in the past
- **THEN** the frontend SHALL render an overdue indicator in list and detail views

### Requirement: Notifications shall support list, detail, create, and mark-read actions
The frontend SHALL provide notification list/detail/create views and mark-read commands that update the shell notification indicator.

#### Scenario: Mark notification read
- **WHEN** a user opens or marks a notification as read
- **THEN** the frontend SHALL call the read command and decrement or refresh the unread indicator

#### Scenario: Notification list empty
- **WHEN** no notifications are returned
- **THEN** the frontend SHALL show an empty state with no misleading unread count
