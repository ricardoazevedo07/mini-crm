# Proposal

## Why

The backend now exposes the CRM domain through `/api/v1`, but `mini-crm-front` is still the untouched Angular template and has no routes, domain services, forms, or CRM layout. A structured frontend proposal is needed to translate the operational use cases into accessible screens and prevent later implementation from diverging from controller contracts.

## What Changes

- Replace the Angular starter screen with a responsive CRM application shell containing sidebar, top bar, navigation, notifications, search, breadcrumbs, and generic CRM dashboard layout.
- Add one Angular page per primary use case or controller surface, including list/detail/create/edit pages and explicit lifecycle command dialogs for sales, campaigns, influencers, curation, contracts, deliverables, finance, tasks, and notifications.
- Add typed Angular HTTP services for every `/api/v1` controller currently present, including `customers`, `leads`, `opportunities`, `proposals`, `campaigns`, `influencers`, `curation-requests`, `curation-candidates`, `contracts`, `deliverables`, `accounts-receivable`, `accounts-payable`, `financial-transactions`, `tasks`, and `notifications`.
- Implement pagination, filters, sorting, loading, empty, 400/404/409/500 error, retry, confirmation, and optimistic UI rules consistently without authentication assumptions.
- Add reusable table, form, status badge, dialog, page header, metric card, timeline, file/evidence, and Problem Details components.
- Add responsive desktop/mobile layouts and accessible keyboard/focus behavior.
- Keep authentication and authorization deferred. The frontend will not implement login, JWT storage, role guards, or permission-based hiding unless a future change adds that contract.

## Capabilities

### New Capabilities

- `crm-sales-ui`: Customer, contact, lead, opportunity, proposal, and proposal-item screens and lifecycle flows.
- `crm-campaign-ui`: Campaign, milestone, creator participation, timeline, summary, and campaign command screens.
- `crm-influencer-ui`: Influencer, social-profile, payment-profile, curation-request, and curation-candidate screens.
- `crm-operations-ui`: Contract and deliverable screens, including approval, signature, rejection, revision, scheduling, and publication flows.
- `crm-finance-ui`: Receivable, payable, payment, transaction, balance, reconciliation, and campaign financial-summary screens.
- `crm-workflow-ui`: Task assignment/completion and notification read/list flows.
- `crm-shell-ui`: Application shell, navigation, routing, global search, dashboard, notifications, responsive layout, shared table/form/dialog patterns, and Problem Details presentation.

### Modified Capabilities

- None. There are no published project capabilities in `openspec/specs` yet.

## Impact

- **Angular project:** `mini-crm-front/mini-crm-front/src/app` receives routing, feature modules/components/pages, typed models, API services, interceptors, and shared UI.
- **Backend contract:** `/api/v1` controllers and their DTOs become the source of truth for HTTP methods, paths, IDs, status commands, pagination, and errors.
- **No backend changes:** This change does not modify ASP.NET controllers or database schemas.
- **No authentication:** No login or role/permission behavior is planned; unauthenticated backend routes are called directly.
- **Testing:** Component, service, routing, and HTTP interaction tests cover CRUD, filters, lifecycle commands, error translation, and responsive shell behavior.
