using mini_crm.Domain.Entities;

namespace mini_crm.Application.Contracts;

public static class CommercialEntityMapper
{
    public static CommercialEntityResponse ToResponse(this EntidadeComercial entity) => entity switch
    {
        Cliente cliente => new(entity.EntidadeComercialId, nameof(Cliente), cliente.Codigo, cliente.Id, null, null, null, null),
        Influencer influencer => new(entity.EntidadeComercialId, nameof(Influencer), null, null, influencer.Id, null, null, null),
        Agencia agencia => new(entity.EntidadeComercialId, nameof(Agencia), null, null, null, agencia.Id, null, agencia.Nome),
        Intermediario intermediario => new(entity.EntidadeComercialId, nameof(Intermediario), null, null, null, null, intermediario.Id, intermediario.Nome),
        _ => new(entity.EntidadeComercialId, entity.GetType().Name, null, null, null, null, null, null)
    };
}
