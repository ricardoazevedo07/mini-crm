# Proposal

## Why

The current domain model covers a few generic business entities, but it does not yet reflect the operational CRM described in the Remo use-case. The project needs a domain specification that captures the actual campaign lifecycle, authorization boundaries, and financial controls before implementation continues.

## What Changes

- Define the CRM domain around the actual business flow: brand/client, opportunity, briefing, campaign, creator assignment, deliveries, contracts, invoices, receivables/payables, repasses, and management reporting.
- Add explicit authorization concepts so each sector sees only the values and data intended by its role.
- Introduce lifecycle and status models to represent the stages from first contact through campaign execution and financial closure.
- Record ownership and audit metadata for who changed what and when.
- Keep the domain aligned with the business rules described in the use-case rather than a generic financial model.

## Capabilities

### New Capabilities
- `crm-domain-model`: Business domain for CRM operations, including customer, opportunity, campaign, delivery, financial, and governance entities, along with role-based visibility and lifecycle/status rules.

### Modified Capabilities
- None

## Impact

- Domain layer under `mini-crm/Domain/Entities` will need to expand and re-align with the actual CRM process.
- Future persistence, repository, and API layers will rely on this domain model and must respect the defined authorization and status rules.
- The specification will guide implementation across commercial, curadoria, planning, atendimento, jurídico, financeiro, and gestão workflows.
