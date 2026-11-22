import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Dashboard } from './dashboard/dashboard';
import { ResourcePage } from './resource-page/resource-page';
import { NotFound } from './not-found/not-found';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
  { path: 'dashboard', component: Dashboard, title: 'Dashboard' },
  { path: 'customers', component: ResourcePage, title: 'Clientes', data: { title: 'Clientes', resource: 'customers', bodyTemplate: '{"name":"","cnpj":"","segmento":"","preferenciaIdioma":"pt-BR","codigo":""}' } },
  { path: 'customers/:id/contacts', component: ResourcePage, title: 'Contatos do cliente', data: { title: 'Contatos do cliente', resource: 'customers/{id}/contacts', bodyTemplate: '{"name":"","cargo":"","email":"","telefone":"","principal":false}' } },
  { path: 'leads', component: ResourcePage, title: 'Leads', data: { title: 'Leads', resource: 'leads', bodyTemplate: '{"nome":"","empresa":"","email":"","telefone":"","responsavelId":null}', commands: [{ label: 'Qualificar', path: 'qualify' }, { label: 'Marcar perdido', path: 'lose' }] } },
  { path: 'opportunities', component: ResourcePage, title: 'Oportunidades', data: { title: 'Oportunidades', resource: 'opportunities', bodyTemplate: '{"clienteEntidadeComercialId":0,"responsavelId":0,"briefing":"","orcamento":0,"moeda":"BRL"}', commands: [{ label: 'Marcar ganha', path: 'win' }, { label: 'Marcar perdida', path: 'lose' }, { label: 'Converter em campanha', path: 'convert-to-campaign' }] } },
  { path: 'proposals', component: ResourcePage, title: 'Propostas', data: { title: 'Propostas', resource: 'proposals', bodyTemplate: '{"oportunidadeId":0,"valor":0,"moeda":"BRL"}', commands: [{ label: 'Enviar', path: 'send' }, { label: 'Aprovar', path: 'approve' }, { label: 'Rejeitar', path: 'reject' }] } },
  { path: 'proposals/:proposalId/items', component: ResourcePage, title: 'Itens da proposta', data: { title: 'Itens da proposta', resource: 'proposals/{proposalId}/items', bodyTemplate: '{"descricao":"","quantidade":1,"valorUnitario":0,"moeda":"BRL"}' } },
  { path: 'campaigns', component: ResourcePage, title: 'Campanhas', data: { title: 'Campanhas', resource: 'campaigns', bodyTemplate: '{"oportunidadeId":0,"clienteEntidadeComercialId":0,"responsavelId":0,"frente":"","dataInicio":"","dataFim":null,"totalAprovado":0,"moeda":"BRL","tipoTaxa":"fixa","taxa":0}', commands: [{ label: 'Iniciar', path: 'start' }, { label: 'Concluir', path: 'complete' }, { label: 'Cancelar', path: 'cancel' }] } },
  { path: 'campaigns/:id/milestones', component: ResourcePage, title: 'Marcos da campanha', data: { title: 'Marcos da campanha', resource: 'campaigns/{id}/milestones', bodyTemplate: '{}' } },
  { path: 'campaigns/:campaignId/creators', component: ResourcePage, title: 'Criadores da campanha', data: { title: 'Criadores da campanha', resource: 'campaigns/{campaignId}/creators', bodyTemplate: '{"influencerEntidadeComercialId":0,"compensacao":0,"moeda":"BRL","termos":""}' } },
  { path: 'campaigns/:id/timeline', component: ResourcePage, title: 'Linha do tempo da campanha', data: { title: 'Linha do tempo da campanha', resource: 'campaigns/{id}/timeline', bodyTemplate: '{}' } },
  { path: 'campaigns/:id/summary', component: ResourcePage, title: 'Resumo da campanha', data: { title: 'Resumo da campanha', resource: 'campaigns/{id}/summary', bodyTemplate: '{}' } },
  { path: 'influencers', component: ResourcePage, title: 'Influenciadores', data: { title: 'Influenciadores', resource: 'influencers', bodyTemplate: '{}' } },
  { path: 'influencers/:influencerId/social-profiles', component: ResourcePage, title: 'Perfis sociais', data: { title: 'Perfis sociais', resource: 'influencers/{influencerId}/social-profiles', bodyTemplate: '{}' } },
  { path: 'influencers/:influencerId/payment-profiles', component: ResourcePage, title: 'Perfis de pagamento', data: { title: 'Perfis de pagamento', resource: 'influencers/{influencerId}/payment-profiles', bodyTemplate: '{}' } },
  { path: 'curation-requests', component: ResourcePage, title: 'Solicitações de curadoria', data: { title: 'Solicitações de curadoria', resource: 'curation-requests', bodyTemplate: '{"campanhaId":0,"briefing":""}' } },
  { path: 'curation-candidates', component: ResourcePage, title: 'Candidatos de curadoria', data: { title: 'Candidatos de curadoria', resource: 'curation-candidates', bodyTemplate: '{"solicitacaoCuradoriaId":"","influencerEntidadeComercialId":0,"avaliacao":null}' } },
  { path: 'contracts', component: ResourcePage, title: 'Contratos', data: { title: 'Contratos', resource: 'contracts', bodyTemplate: '{"clienteEntidadeComercialId":0,"agenciaEntidadeComercialId":0,"agenteId":0,"influencerEntidadeComercialId":0,"conteudoContrato":""}' } },
  { path: 'deliverables', component: ResourcePage, title: 'Entregáveis', data: { title: 'Entregáveis', resource: 'deliverables', bodyTemplate: '{"campanhaId":0,"participacaoCampanhaId":"","titulo":"","dataLimiteUtc":""}', commands: [{ label: 'Enviar', path: 'submit' }, { label: 'Aprovar', path: 'approve' }, { label: 'Rejeitar', path: 'reject' }] } },
  { path: 'accounts-receivable', component: ResourcePage, title: 'Contas a receber', data: { title: 'Contas a receber', resource: 'accounts-receivable', bodyTemplate: '{"campanhaId":0,"participacaoCampanhaId":null,"valor":0,"valorAprovado":null,"moeda":"BRL","dataVencimentoUtc":""}' } },
  { path: 'accounts-payable', component: ResourcePage, title: 'Contas a pagar', data: { title: 'Contas a pagar', resource: 'accounts-payable', bodyTemplate: '{"campanhaId":0,"participacaoCampanhaId":null,"valor":0,"valorAprovado":null,"moeda":"BRL","dataVencimentoUtc":""}' } },
  { path: 'finance', redirectTo: 'accounts-receivable', pathMatch: 'full' },
  { path: 'financial-transactions', component: ResourcePage, title: 'Transações financeiras', data: { title: 'Transações financeiras', resource: 'financial-transactions', bodyTemplate: '{"contaReceberId":null,"contaPagarId":null,"repasseCreatorId":null,"valor":0,"moeda":"BRL","referencia":""}' } },
  { path: 'tasks', component: ResourcePage, title: 'Tarefas', data: { title: 'Tarefas', resource: 'tasks', bodyTemplate: '{"titulo":"","descricao":"","responsavelId":null,"prazoUtc":null}', commands: [{ label: 'Iniciar', path: 'start' }, { label: 'Concluir', path: 'complete' }, { label: 'Cancelar', path: 'cancel' }] } },
  { path: 'notifications', component: ResourcePage, title: 'Notificações', data: { title: 'Notificações', resource: 'notifications', bodyTemplate: '{"titulo":"","mensagem":""}', commands: [{ label: 'Marcar lida', path: 'read' }] } },
  { path: '**', component: NotFound, title: 'Página não encontrada' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
