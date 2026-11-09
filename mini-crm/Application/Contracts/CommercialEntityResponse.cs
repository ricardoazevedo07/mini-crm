namespace mini_crm.Application.Contracts;

public sealed record CommercialEntityResponse(
    long EntidadeComercialId,
    string Tipo,
    string? Codigo,
    Guid? ClienteId,
    Guid? InfluencerId,
    int? AgenciaId,
    int? IntermediarioId,
    string? Nome);
