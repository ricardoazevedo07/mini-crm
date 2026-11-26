import { Injectable } from '@angular/core';

/**
 * Perfil de visualização ("Ver como").
 * O backend ainda não tem autenticação: isto só simula na tela o que cada setor vê.
 * Quando houver login, este serviço passa a ler o perfil do usuário autenticado.
 */
export type Role = 'Gestor' | 'Comercial' | 'Financeiro' | 'Curadoria' | 'Planejamento' | 'Atendimento' | 'Jurídico';

export const ROLES: Role[] = ['Gestor', 'Comercial', 'Financeiro', 'Curadoria', 'Planejamento', 'Atendimento', 'Jurídico'];

/** Setores que veem valores de campanha. */
const SEE_VALUES: Role[] = ['Gestor', 'Comercial', 'Financeiro', 'Curadoria'];

/** Áreas restritas: prefixo do resource/rota → perfis com acesso. */
const RESTRICTED: { match: RegExp; roles: Role[]; label: string }[] = [
  { match: /^(accounts-receivable|accounts-payable|financial-transactions)/, roles: ['Financeiro', 'Gestor'], label: 'O Financeiro é só do Financeiro e do Gestor.' },
  { match: /^dashboard/, roles: ['Gestor', 'Comercial', 'Financeiro'], label: 'A tela de Gestão é do Gestor, dos Comerciais e do Financeiro.' },
];

/** Campos de valor em dinheiro (escondidos para quem não vê valores). */
const MONEY_KEY = /(valor|orcamento|total|taxa|compensacao|saldo|preco|montante|cache|fee)/i;

@Injectable({ providedIn: 'root' })
export class RoleService {
  role: Role = 'Gestor';

  constructor() {
    try {
      const saved = localStorage.getItem('crm.role') as Role | null;
      if (saved && ROLES.includes(saved)) this.role = saved;
    } catch { /* armazenamento indisponível */ }
  }

  setRole(role: Role): void {
    this.role = role;
    try { localStorage.setItem('crm.role', role); } catch { /* armazenamento indisponível */ }
  }

  get canSeeValues(): boolean {
    return SEE_VALUES.includes(this.role);
  }

  isMoneyKey(key: string): boolean {
    return MONEY_KEY.test(key);
  }

  /** Retorna a mensagem de bloqueio quando o perfil atual não pode abrir a área; null quando pode. */
  blockedMessage(resourceOrPath: string): string | null {
    const path = resourceOrPath.replace(/^\//, '');
    const rule = RESTRICTED.find((r) => r.match.test(path));
    return rule && !rule.roles.includes(this.role) ? rule.label : null;
  }
}
