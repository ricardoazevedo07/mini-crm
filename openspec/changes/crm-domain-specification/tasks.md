# Tasks

## 1. Domain Model Alignment

- [x] 1.1 Review the existing entities in `mini-crm/Domain/Entities` and document the gaps against the Remo use-case; verify the missing campaign, opportunity, and financial lifecycle concepts are explicitly captured by the design.
- [x] 1.2 Define the aggregate boundaries for `Cliente`, `Oportunidade`, `Campanha`, `CampanhaCreator`, `Contrato`, and financial records; verify each entity owns the lifecycle data required by the spec.
- [x] 1.3 Add the role and visibility model for commercial, curadoria, planejamento, atendimento, jurídico, financeiro, and gestão; verify the design states what each sector can view and modify.

## 2. Status, Ownership, and Audit Design

- [x] 2.1 Model the lifecycle status enums for opportunity, campaign, delivery, contract, invoice, repayment, and delay handling; verify each enum reflects the actual stages described in the case study.
- [x] 2.2 Add audit metadata to core domain entities (`CriadoPor`, `AtualizadoPor`, `DataCriacao`, `DataAtualizacao`); verify the design records both responsibility and change time.
- [x] 2.3 Specify the minimum required value fields for totals, fee, currency, due dates, and approved amounts; verify the domain contract covers the data declared as sensitive in the use-case.

## 3. Spec and Design Readiness

- [x] 3.1 Confirm the capability `crm-domain-model` covers the full lifecycle and department boundaries described in the use-case; verify the proposal, spec, and design align with the same behaviors.
- [x] 3.2 Check the design against the scenario requirements and ensure no acceptance criterion is missing; verify each requirement in the spec has at least one scenario and each design decision maps back to a required behavior.
- [x] 3.3 Prepare the implementation handoff for the next implementation phase; verify that the codebase has a clear domain target, expected entities, and permission boundaries before persistence or API work begins.

## Workflow follow-up

- Archive this change after review and before implementation begins.
- Verify the archived history reflects the final domain model and the accepted CRM lifecycle before code changes are started.
