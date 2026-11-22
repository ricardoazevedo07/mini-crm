# Spec Delta

## Purpose

Provides the authenticated-operational application shell and shared interaction patterns for the Angular CRM, including navigation, dashboard, routing, responsive layout, global feedback, and unauthenticated API usage.

## ADDED Requirements

### Requirement: The CRM application shall present a consistent application shell
The frontend SHALL render a persistent responsive shell with sidebar navigation, top bar, breadcrumb context, global search, notification indicator, page title, and primary action slot.

#### Scenario: Desktop shell
- **WHEN** a user opens the CRM at a viewport width of at least 1024 pixels
- **THEN** the frontend SHALL show persistent sidebar navigation and the active route's breadcrumb, title, and content

#### Scenario: Mobile shell
- **WHEN** the viewport width is below 1024 pixels
- **THEN** the frontend SHALL collapse the sidebar behind an accessible menu control and keep the page content readable

### Requirement: The dashboard shall summarize CRM operational activity
The frontend SHALL provide a dashboard route that shows campaign, opportunity, receivable, payable, task, and deliverable summary cards using typed API responses.

#### Scenario: Dashboard data loads
- **WHEN** the dashboard route is opened and summary requests succeed
- **THEN** the frontend SHALL render metrics, recent activity, and actionable shortcuts for the most important modules

#### Scenario: Dashboard data fails
- **WHEN** one or more dashboard requests return an error
- **THEN** the frontend SHALL retain the shell and show a module-level retry state instead of replacing the whole application

### Requirement: Shared routes shall be deterministic and deep-linkable
The frontend SHALL provide deep-linkable Angular routes for every implemented CRM page and SHALL preserve resource identifiers in the URL.

#### Scenario: Direct resource navigation
- **WHEN** a user opens or refreshes a detail route containing a resource identifier
- **THEN** the frontend SHALL load that resource and render its detail or a not-found page

#### Scenario: Unknown route
- **WHEN** a user navigates to an unknown route
- **THEN** the frontend SHALL render a useful not-found state with navigation back to the dashboard

### Requirement: Shared API errors shall use consistent visual feedback
The frontend SHALL translate Problem Details responses into accessible inline errors, toasts, or page-level states while never exposing stack traces or SQL details.

#### Scenario: Validation error
- **WHEN** the API returns 400 with field errors
- **THEN** the frontend SHALL associate each message with the corresponding form control and mark the form as invalid

#### Scenario: Not-found or conflict
- **WHEN** the API returns 404 or 409
- **THEN** the frontend SHALL show a stable human-readable message and preserve user-entered form data when possible

#### Scenario: Unexpected error
- **WHEN** the API returns 500 or cannot be reached
- **THEN** the frontend SHALL show a sanitized error state with retry action

### Requirement: Authentication shall remain out of the frontend scope
The frontend SHALL call the `/api/v1` routes without login, JWT storage, credentials, route guards, or role-based UI projection.

#### Scenario: Initial application load
- **WHEN** the application starts before authentication is implemented
- **THEN** the frontend SHALL allow access to every planned route without redirecting to login or checking role claims
