export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface ProblemDetails {
  status?: number;
  title?: string;
  detail?: string;
  instance?: string;
  code?: string;
  errors?: Record<string, string[]>;
}

export interface CreateCustomerRequest { name: string; cnpj: string; segmento?: string; preferenciaIdioma?: string; codigo?: string; }
export interface UpdateCustomerRequest { name: string; cnpj: string; segmento?: string; preferenciaIdioma?: string; }
export interface CustomerResponse { id: number; businessId: string; code: string; name: string; cnpj: string; segmento?: string; preferenciaIdioma?: string; active: boolean; }
export interface CustomerContactRequest { name: string; cargo?: string; email: string; telefone?: string; principal: boolean; }
export interface CustomerContactResponse { id: string; name: string; cargo?: string; email: string; telefone?: string; principal: boolean; }

export interface LeadRequest { nome: string; empresa?: string; email?: string; telefone?: string; responsavelId?: number; }
export interface LeadResponse { id: number; nome: string; empresa?: string; email?: string; telefone?: string; responsavelId?: number; status: string; }
export interface OpportunityCreateRequest { clienteEntidadeComercialId: number; responsavelId: number; briefing: string; orcamento: number; moeda: string; }
export interface ProposalRequest { oportunidadeId: number; valor: number; moeda: string; }
export interface ProposalItemRequest { descricao: string; quantidade: number; valorUnitario: number; moeda: string; }

export interface CampaignRequest { oportunidadeId: number; clienteEntidadeComercialId: number; responsavelId: number; frente: string; dataInicio: string; dataFim?: string; totalAprovado: number; moeda: string; tipoTaxa: 'fixa' | 'percentual'; taxa: number; }
export interface CampaignCreatorRequest { influencerEntidadeComercialId: number; compensacao: number; moeda: string; termos: string; }

export interface CurationRequestCreate { campanhaId: number; briefing: string; }
export interface CurationCandidateRequest { solicitacaoCuradoriaId: string; influencerEntidadeComercialId: number; avaliacao?: number; }
export interface ContractRequest { clienteEntidadeComercialId: number; agenciaEntidadeComercialId: number; agenteId: number; influencerEntidadeComercialId: number; conteudoContrato: string; }
export interface DeliveryRequest { campanhaId: number; participacaoCampanhaId: string; titulo: string; dataLimiteUtc: string; }
export interface PublishDeliveryRequest { evidencia: string; publicadoEmUtc?: string; }
export interface FinancialRequest { campanhaId: number; participacaoCampanhaId?: string; valor: number; valorAprovado?: number; moeda: string; dataVencimentoUtc: string; }
export interface SettlementRequest { valor: number; moeda: string; referencia: string; }
export interface TransactionRequest { contaReceberId?: number; contaPagarId?: number; repasseCreatorId?: string; valor: number; moeda: string; referencia: string; }
export interface TaskRequest { titulo: string; descricao?: string; responsavelId?: number; prazoUtc?: string; }
export interface NotificationRequest { titulo: string; mensagem: string; }
