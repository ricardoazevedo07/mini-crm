/**
 * Monta campos de formulário a partir do bodyTemplate da rota e devolve
 * o MESMO corpo JSON que a tela enviava antes (mesmas chaves e tipos).
 */
export type FieldType = 'text' | 'textarea' | 'number' | 'checkbox' | 'date' | 'datetime' | 'select';

export interface FormField {
  key: string;
  label: string;
  type: FieldType;
  options?: { value: string; label: string }[];
  nullable: boolean;
  hint?: string;
}

const SELECTS: Record<string, { value: string; label: string }[]> = {
  moeda: [{ value: 'BRL', label: 'Real (BRL)' }, { value: 'USD', label: 'Dólar (USD)' }, { value: 'EUR', label: 'Euro (EUR)' }],
  frente: [{ value: 'CURADORIA', label: 'Curadoria' }, { value: 'ASSESSORIA', label: 'Assessoria' }],
  tipoTaxa: [{ value: 'fixa', label: 'Fixa (R$)' }, { value: 'percentual', label: 'Percentual (%)' }],
  preferenciaIdioma: [{ value: 'pt-BR', label: 'Português' }, { value: 'en-US', label: 'Inglês' }, { value: 'es', label: 'Espanhol' }],
};

const LONG_TEXT = /(briefing|termos|descricao|conteudo|mensagem)/i;

export function labelOf(key: string): string {
  const words = key.replace(/Utc$/, '').replace(/EntidadeComercialId$/, '').replace(/([a-z0-9])([A-Z])/g, '$1 $2').replace(/_/g, ' ').toLowerCase();
  return words.charAt(0).toUpperCase() + words.slice(1);
}

function hintOf(key: string): string | undefined {
  if (/Id$/.test(key)) return 'Número de identificação (ID) do registro relacionado.';
  if (/Utc$/.test(key)) return 'Data e hora no seu fuso; o sistema guarda em UTC.';
  return undefined;
}

export function buildFields(template: Record<string, unknown>): FormField[] {
  return Object.keys(template).map((key) => {
    const v = template[key];
    let type: FieldType = 'text';
    if (SELECTS[key]) type = 'select';
    else if (typeof v === 'boolean') type = 'checkbox';
    else if (typeof v === 'number') type = 'number';
    else if (/Utc$/.test(key)) type = 'datetime';
    else if (/^data[A-Z]/.test(key)) type = 'date';
    else if (/Id$/.test(key) && v === null) type = 'number';
    else if (LONG_TEXT.test(key)) type = 'textarea';
    return { key, label: labelOf(key), type, options: SELECTS[key], nullable: v === null, hint: hintOf(key) };
  });
}

/** Valor do registro → valor que o campo mostra. */
export function toInput(field: FormField, value: unknown): string | boolean {
  if (field.type === 'checkbox') return value === true;
  if (value === null || value === undefined) return '';
  const s = String(value);
  if (field.type === 'date') return s.slice(0, 10);
  if (field.type === 'datetime') {
    const d = new Date(s);
    if (isNaN(d.getTime())) return '';
    const p = (n: number) => String(n).padStart(2, '0');
    return `${d.getFullYear()}-${p(d.getMonth() + 1)}-${p(d.getDate())}T${p(d.getHours())}:${p(d.getMinutes())}`;
  }
  return s;
}

/** Valor do campo → valor enviado à API, respeitando o tipo do template. */
export function fromInput(field: FormField, input: string | boolean, original: unknown): unknown {
  if (field.type === 'checkbox') return input === true;
  const s = String(input ?? '').trim();
  if (s === '') return field.nullable ? null : (typeof original === 'number' ? 0 : '');
  if (field.type === 'number') return Number(s.replace(',', '.'));
  if (field.type === 'datetime') return new Date(s).toISOString();
  return s;
}
