import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProblemDetails } from '../core/api-models';
import { CrmApiService } from '../core/crm-api.service';

@Component({
  selector: 'app-resource-page',
  standalone: false,
  styleUrl: './resource-page.css',
  templateUrl: './resource-page.html',
})
export class ResourcePage implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(CrmApiService);

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
      this.commands = Array.isArray(data['commands']) ? data['commands'] as { label: string; path: string }[] : [];
      this.load();
    });
  }

  load(): void {
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
    this.formOpen = true;
  }

  openDetail(item: Record<string, unknown>): void {
    this.selected = item;
  }

  openEdit(item: Record<string, unknown>): void {
    this.editingId = (item['id'] ?? item['entidadeComercialId'] ?? null) as string | number | null;
    this.bodyText = JSON.stringify(item, null, 2);
    this.formOpen = true;
  }

  save(): void {
    try {
      const body = JSON.parse(this.bodyText);
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

  idOf(item: Record<string, unknown>): string | number {
    return (item['id'] ?? item['entidadeComercialId'] ?? '-') as string | number;
  }
}
