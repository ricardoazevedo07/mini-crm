# Tasks

## 1. Angular foundation and API configuration

- [x] 1.1 Replace the Angular starter root content with a CRM shell route outlet and remove placeholder links/assets; verify `npm run build` succeeds and the app shell renders at `/dashboard`.
- [x] 1.2 Configure `HttpClient` providers, API environment URL, and optional development proxy for `/api/v1`; verify a typed request reaches the configured backend base URL and all service names/routes match the exact controller matrix in `design.md`.
- [x] 1.6 Add TypeScript request/response interfaces for every controller DTO listed in `design.md`, including exact property names, route ID types, optional fields, and response wrappers; verify each interface compiles and is covered by at least one HTTP contract test.
- [x] 1.3 Create feature-folder structure for `shell`, `sales`, `campaigns`, `influencers`, `operations`, `finance`, and `workflow`; verify every feature has routing and barrel exports where appropriate.
- [x] 1.4 Add typed Problem Details, pagination, filter, sort, and API error models; verify 400, 404, 409, 500, and network failures serialize to stable UI errors.
- [x] 1.5 Implement an HTTP error interceptor and loading notification mechanism; verify field errors are retained and 500 responses never expose server details.

## 2. Shared shell and reusable UI

- [x] 2.1 Replace `app.html` and `app.css` with the CRM application shell, including sidebar, top bar, breadcrumb, notification indicator, and content outlet; verify desktop and mobile shell tests and viewport behavior.
- [x] 2.2 Configure application routes for dashboard and every CRM feature; verify direct navigation and refresh work for list/detail routes and unknown routes show a not-found page.
- [x] 2.3 Create reusable page header, metric card, status badge, table, paginator, empty state, loading state, confirmation dialog, and problem-details components; verify shared component unit tests.
- [ ] 2.4 Implement the dashboard page with campaign, opportunity, receivable, payable, deliverable, and task summary cards and recent activity; verify partial API failures preserve the shell and show module-level retry controls.
- [ ] 2.5 Add global search or route entry points to CRM collections, accessible keyboard behavior, focus restoration, and mobile navigation; verify keyboard and component interaction tests.

## 3. Customer and sales pages

- [x] 3.1 Create typed `CustomersApi` methods for list, detail, create, update, archive, contact list/create/update, and customer filters; verify HTTP paths, payloads, and pagination against controller contracts.
- [x] 3.2 Implement customer list, detail, create/edit, archive, and contact pages with CNPJ, segment, language, active state, and nested contacts; verify create/update/archive and duplicate-CNPJ conflict scenarios.
- [x] 3.3 Create typed `LeadsApi` methods and lead list/detail/create/edit/qualify/lose pages; verify list filters, status commands, and loss-flow behavior.
- [x] 3.4 Create typed `OpportunitiesApi` methods and opportunity list/detail/create/win/lose/convert pages; verify win/loss and conversion navigation and 409 preservation behavior.
- [x] 3.5 Create typed `ProposalsApi` methods and proposal detail/create/send/approve/reject pages; verify approved proposals block in-place editing.
- [x] 3.6 Create proposal-item nested components and list/create/edit interactions for draft proposals; verify item totals, server-owned IDs, and approved-proposal rejection behavior.
- [ ] 3.7 Add sales feature routing and end-to-end component tests for customer-to-opportunity-to-proposal navigation; verify 400/404/409 states and `Location` navigation behavior.

## 4. Influencer and curation pages

- [x] 4.1 Create typed `InfluencersApi` service for influencer list/detail and profile relations; verify route IDs, paging, and response mappings.
- [x] 4.2 Implement influencer list/detail/create/edit pages and nested social-profile/payment-profile sections; verify persistence navigation fields are not rendered and unauthenticated routes load normally.
- [x] 4.3 Create typed `CurationRequestsApi` and request list/detail/create/candidate pages; verify campaign and briefing validation and candidate scoping.
- [x] 4.4 Create typed `CurationCandidatesApi` and candidate detail/create/approve/reject/evaluation controls; verify duplicate request/influencer pairs and lifecycle conflict handling.
- [ ] 4.5 Add influencer/curation routing and HTTP/component tests for filtering, nested routes, duplicate candidates, and 404/409 responses.

## 5. Campaign and creator pages

- [x] 5.1 Create typed `CampaignsApi` methods for campaign CRUD, milestones, start/complete/cancel, timeline, and summary; verify every status change is a command and metadata updates do not send status.
- [x] 5.2 Implement campaign list/detail/create/edit pages with overview, commercial/operational owners, dates, currency, fee, and status tabs; verify route refresh and invalid transition states.
- [ ] 5.3 Implement campaign milestone list/create/edit/complete UI with overdue and due-soon states; verify deadline sorting and complete-state presentation.
- [x] 5.4 Create `CampaignCreatorsApi` interactions and nested creator list/add/approve/cancel UI; verify duplicate active participation and cancellation history behavior.
- [ ] 5.5 Implement campaign timeline and summary tabs; verify chronological deliverable/milestone rendering and campaign financial totals.
- [ ] 5.6 Add campaign feature routing and tests for create, start, complete/cancel, creator cancellation, timeline, summary, and 409 conflicts.

## 6. Contract and deliverable pages

- [x] 6.1 Create typed `ContractsApi` methods for contract list/detail/create and send-for-signature/register-signature/cancel commands; verify party IDs and immutable status fields.
- [x] 6.2 Implement contract list/detail/create/version/amendment pages with signed read-only state and cancellation reason dialog; verify signed versions cannot be edited in place.
- [x] 6.3 Create typed `DeliverablesApi` methods for list/detail/create/submit/approve/reject/publish commands; verify required rejection reason and publication evidence/time.
- [ ] 6.4 Implement deliverable list/detail/create/revision/approval/timeline UI; verify submit, revision, reject, approve, schedule, publish, and complete states.
- [ ] 6.5 Add operations routing and tests for signed contracts, amendments, rejection reasons, publication evidence, and lifecycle 409 responses.

## 7. Finance and settlement pages

- [x] 7.1 Create typed `AccountsReceivableApi` and `AccountsPayableApi` methods for list/detail/create and individual settlement/payment operations; verify campaign scope, ISO currency, due date, approved amount, and balance fields.
- [ ] 7.2 Implement separate receivable and payable list/detail/create pages with status and outstanding-balance presentations; verify partial, liquidated, overdue, and canceled states.
- [ ] 7.3 Implement receipt/payment dialogs with remaining balance and currency checks; verify currency mismatch and over-balance API conflicts retain form data.
- [x] 7.4 Create typed `FinancialTransactionsApi` methods and transaction list/detail/create/reconcile UI; verify immutable settlement history and reference fields.
- [ ] 7.5 Implement campaign finance summary and repasse/banking-related readouts without authentication/permission checks; verify route accessibility and scoped totals.
- [ ] 7.6 Add finance feature routing and tests for create, partial settlement, over-balance 409, currency mismatch, reconciliation, and unauthenticated route access.

## 8. Task and notification pages

- [x] 8.1 Create typed `TasksApi` methods for list/detail/create/assign/start/complete/cancel; verify owner, deadline, and status payloads.
- [ ] 8.2 Implement task list/detail/create/assignment pages with overdue/due-soon statuses and explicit lifecycle controls; verify completion and cancellation behavior.
- [x] 8.3 Create typed `NotificationsApi` methods for list/detail/create/read; verify shell unread count updates after mark-read.
- [ ] 8.4 Add workflow routing and tests for task assignment/completion/cancellation and notification mark-read/empty state.

## 9. Integration, quality, and documentation

- [ ] 9.1 Add Angular HTTP testing setup and service contract tests for every implemented `/api/v1` controller; verify request method, URL, exact JSON property names, route identifier type, command body, pagination, and error mapping against the controller matrix.
- [ ] 9.2 Add routing and component tests for all primary use-case pages and lifecycle dialogs; verify no route requires login or role policy.
- [x] 9.3 Run `npm run build` and the Angular test suite; verify production build, unit/component tests, and no high-severity template or TypeScript diagnostics.
- [ ] 9.4 Exercise the frontend against a running backend with database data available; verify customer/sales, campaign/delivery, contract, finance, task, and notification representative flows.
- [ ] 9.5 Document the route map, service map, environment configuration, and frontend error conventions in the project README; verify setup and representative test commands work as documented.

## Workflow follow-up

- Apply this change only after backend `/api/v1` route DTOs are stable and the development database/migration is available.
- Archive the change after implementation review and verification; authentication and authorization remain separate future work.
