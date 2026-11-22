import { describe, expect, it } from 'vitest';
import { CreateCustomerRequest, PagedResult, ProblemDetails } from './api-models';

describe('API model contracts', () => {
  it('accepts the customer controller request shape', () => {
    const request: CreateCustomerRequest = {
      name: 'Empresa Demo',
      cnpj: '12345678000190',
      segmento: 'Marketing',
      preferenciaIdioma: 'pt-BR',
    };

    expect(request.name).toBe('Empresa Demo');
  });

  it('supports paged response and Problem Details shapes', () => {
    const page: PagedResult<number> = { items: [1], page: 1, pageSize: 20, totalCount: 1, totalPages: 1 };
    const problem: ProblemDetails = { status: 409, code: 'conflict', detail: 'Operação inválida.' };

    expect(page.totalPages).toBe(1);
    expect(problem.status).toBe(409);
  });
});
