import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProblemDetails } from '../core/api-models';
import { CrmApiService } from '../core/crm-api.service';
import { RoleService } from '../shared/role.service';
import { FormField, buildFields, fromInput, labelOf, toInput } from '../shared/form-fields';

@Component({
  selector: 'app-resource-page',
  standalone: false,
  styleUrl: './resource-page.css',
  templateUrl: './resource-page.html',
})
export class ResourcePage implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(CrmApiService);
  protected readonly roles = inject(RoleService);

  title = 'Recurso';
  resource = '';
  items: Record<string, unknown>[] = [];
  loading = false;
  error: ProblemDetails | null = null;
  formOpen = false;
  editingId: string | number | null = null;
  bodyText = '{}';
  selected: Record<string, unknown> | null = null;
  commands: { label: string; path: string }[] = [];
  /** Modelo do corpo (bodyTemplate da rota) usado para montar os campos. */
  private template: Record<string, unknown> = {};
  /** Base do corpo enviado: o template (novo) ou o registro inteiro (edição), como antes. */
  private base: Record<string, unknown> = {};
  fields: FormField[] = [];
  values: Record<string, string | boolean> = {};
  advanced = false;

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.title = String(data['title'] ?? 'Recurso');
      let resource = String(data['resource'] ?? '');
      this.route.paramMap.subscribe((params) => {
        params.keys.forEach((key) => {
          resource = resource.replace(`{${key}}`, params.get(key) ?? '');
        });
        this.resource = resource;
      });
      this.bodyText = String(data['bodyTemplate'] ?? '{}');
      try { this.template = JSON.parse(this.bodyText); } catch { this.template = {}; }
      this.commands = Array.isArray(data['commands']) ? data['commands'] as { label: string; path: string }[] : [];
      this.load();
    });
  }

  get blocked(): string | null {
    return this.roles.blockedMessage(this.resource);
  }

  load(): void {
    if (this.blocked) { this.items = []; this.loading = false; this.error = null; return; }
    this.loading = true;
    this.error = null;
    this.api.list<unknown>(this.resource).subscribe({
      next: (response) => {
        const value = response as { items?: Record<string, unknown>[] };
        this.items = Array.isArray(response) ? response : value.items ?? [];
        this.loading = false;
      },
      error: (problem: ProblemDetails) => {
        this.error = problem;
        this.loading = false;
      },
    });
  }

  openCreate(): void {
    this.editingId = null;
    this.error = null;
    this.prepareForm({ ...this.template });
    this.formOpen = true;
  }

  openDetail(item: Record<string, unknown>): void {
    this.selected = item;
  }

  openEdit(item: Record<string, unknown>): void {
    this.editingId = (item['id'] ?? item['entidadeComercialId'] ?? null) as string | number | null;
    this.error = null;
    this.prepareForm({ ...item });
    this.formOpen = true;
  }

  /** Prepara campos e valores; o JSON avançado continua disponível e sincronizado. */
  private prepareForm(base: Record<string, unknown>): void {
    this.base = base;
    const shape = Object.keys(this.template).length ? this.template : this.scalarShape(base);
    this.fields = buildFields(shape).filter((f) => this.roles.canSeeValues || !this.roles.isMoneyKey(f.key));
    this.values = {};
    this.fields.forEach((f) => { this.values[f.key] = toInput(f, base[f.key] ?? shape[f.key]); });
    this.bodyText = JSON.stringify(base, null, 2);
    this.advanced = this.fields.length === 0;
  }

  /** Para rotas sem template, edita os campos simples (texto, número, sim/não) do próprio registro. */
  private scalarShape(item: Record<string, unknown>): Record<string, unknown> {
    const shape: Record<string, unknown> = {};
    Object.keys(item).forEach((k) => {
      const v = item[k];
      if (k !== 'id' && (v === null || ['string', 'number', 'boolean'].includes(typeof v))) shape[k] = v;
    });
    return shape;
  }

  /** Corpo a enviar: a base com os campos do formulário aplicados (mesmas chaves e tipos de antes). */
  private bodyFromFields(): Record<string, unknown> {
    const body: Record<string, unknown> = { ...this.base };
    this.fields.forEach((f) => { body[f.key] = fromInput(f, this.values[f.key], this.base[f.key] ?? this.template[f.key]); });
    return body;
  }

  toggleAdvanced(): void {
    if (!this.advanced) {
      this.bodyText = JSON.stringify(this.bodyFromFields(), null, 2);
    } else {
      try {
        const parsed = JSON.parse(this.bodyText) as Record<string, unknown>;
        this.base = parsed;
        this.fields.forEach((f) => { this.values[f.key] = toInput(f, parsed[f.key]); });
      } catch {
        this.error = { status: 400, code: 'validation_error', detail: 'Corrija o JSON antes de voltar aos campos.' };
        return;
      }
    }
    this.advanced = !this.advanced;
  }

  save(): void {
    try {
      const body = this.advanced ? JSON.parse(this.bodyText) : this.bodyFromFields();
      const request = this.editingId === null
        ? this.api.create(this.resource, body)
        : this.api.update(this.resource, this.editingId, body);
      request.subscribe({
        next: () => { this.formOpen = false; this.load(); },
        error: (problem: ProblemDetails) => { this.error = problem; },
      });
    } catch {
      this.error = { status: 400, code: 'validation_error', detail: 'Informe um JSON válido.' };
    }
  }

  runCommand(item: Record<string, unknown>, command: { label: string; path: string }): void {
    const id = this.idOf(item);
    this.api.post(this.resource, `${id}/${command.path}`).subscribe({
      next: () => this.load(),
      error: (problem: ProblemDetails) => { this.error = problem; },
    });
  }

  keys(item: Record<string, unknown>): string[] {
    return Object.keys(item).filter((key) => key !== 'pagamentos' && key !== 'contratos').slice(0, 6);
  }

  allKeys(item: Record<string, unknown>): string[] {
    return Object.keys(item);
  }

  /** Transforma o nome técnico do campo em rótulo legível (ex.: dataVencimentoUtc → Data vencimento). */
  label(key: string): string {
    return labelOf(key);
  }

  /** Valor exibido na tabela/detalhe: esconde dinheiro para quem não vê valores. */
  show(key: string, value: unknown): unknown {
    if (!this.roles.canSeeValues && this.roles.isMoneyKey(key)) return 'Oculto';
    return value ?? '—';
  }

  isStatus(key: string): boolean {
    return /status|situacao/i.test(key);
  }

  statusClass(value: unknown): string {
    const v = String(value ?? '').toLowerCase();
    if (/(ganh|aprov|conclu|pago|assinad|ativ|execu|lida)/.test(v)) return 'status--ok';
    if (/(perd|cancel|rejeit|atras|vencid)/.test(v)) return 'status--travado';
    if (/(pend|abert|envi|aguard|propos|rascunho)/.test(v)) return 'status--atencao';
    return '';
  }

  idOf(item: Record<string, unknown>): string | number {
    return (item['id'] ?? item['entidadeComercialId'] ?? '-') as string | number;
  }
}
