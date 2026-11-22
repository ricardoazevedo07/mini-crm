namespace mini_crm.Application.Contracts;

public sealed record LeadRequest(string Nome, string? Empresa, string? Email, string? Telefone, long? ResponsavelId);
public sealed record ProposalRequest(long OportunidadeId, decimal Valor, string Moeda);
public sealed record CampaignRequest(long OportunidadeId, long ClienteEntidadeComercialId, long ResponsavelId, string Frente, DateTime DataInicio, DateTime? DataFim, decimal TotalAprovado, string Moeda, string TipoTaxa, decimal Taxa);
public sealed record DeliveryRequest(long CampanhaId, Guid ParticipacaoCampanhaId, string Titulo, DateTime DataLimiteUtc);
public sealed record CurationRequestCreate(long CampanhaId, string Briefing);
public sealed record CurationCandidateRequest(Guid SolicitacaoCuradoriaId, long InfluencerEntidadeComercialId, decimal? Avaliacao);
public sealed record ContractRequest(long ClienteEntidadeComercialId, long AgenciaEntidadeComercialId, int AgenteId, long InfluencerEntidadeComercialId, string ConteudoContrato);
public sealed record FinancialRequest(long CampanhaId, Guid? ParticipacaoCampanhaId, decimal Valor, decimal? ValorAprovado, string Moeda, DateTime DataVencimentoUtc);
public sealed record SettlementRequest(decimal Valor, string Moeda, string Referencia);
public sealed record TaskRequest(string Titulo, string? Descricao, long? ResponsavelId, DateTime? PrazoUtc);
public sealed record NotificationRequest(string Titulo, string Mensagem);
public sealed record PublishDeliveryRequest(string Evidencia, DateTime? PublicadoEmUtc);
public sealed record OpportunityCreateRequest(long ClienteEntidadeComercialId, long ResponsavelId, string Briefing, decimal Orcamento, string Moeda);
public sealed record ProposalItemRequest(string Descricao, decimal Quantidade, decimal ValorUnitario, string Moeda);
public sealed record CampaignCreatorRequest(long InfluencerEntidadeComercialId, decimal Compensacao, string Moeda, string Termos);
public sealed record TransactionRequest(long? ContaReceberId, long? ContaPagarId, Guid? RepasseCreatorId, decimal Valor, string Moeda, string Referencia);
