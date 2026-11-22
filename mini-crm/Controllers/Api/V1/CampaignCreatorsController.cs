using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/campaigns/{campaignId:long}/creators")]
public sealed class CampaignCreatorsController(IRepository<ParticipacaoCampanha> participations, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(long campaignId, CancellationToken ct = default)
    {
        var items = await participations.ListAsync(ct);
        return Ok(items.Where(x => x.CampanhaId == campaignId).ToList());
    }

    [HttpPost]
    public async Task<IActionResult> Create(long campaignId, [FromBody] CampaignCreatorRequest request, CancellationToken ct = default)
    {
        var items = await participations.ListAsync(ct);
        if (items.Any(x => x.CampanhaId == campaignId && x.InfluencerEntidadeComercialId == request.InfluencerEntidadeComercialId && x.Status != StatusParticipacao.Cancelado && x.Status != StatusParticipacao.Concluido))
            throw new InvalidOperationException("Duplicate active creator participation.");
        var item = new ParticipacaoCampanha { Id = Guid.NewGuid(), CampanhaId = campaignId, InfluencerEntidadeComercialId = request.InfluencerEntidadeComercialId };
        item.Negociar(new Dinheiro(request.Compensacao, request.Moeda), request.Termos);
        await participations.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Created($"/api/v1/campaigns/{campaignId}/creators/{item.Id}", item);
    }

    [HttpPost("{participationId:guid}/approve")]
    public async Task<IActionResult> Approve(long campaignId, Guid participationId, CancellationToken ct = default)
    {
        var item = await participations.FindAsync([participationId], ct) ?? throw new KeyNotFoundException();
        if (item.CampanhaId != campaignId) throw new KeyNotFoundException();
        item.Aprovar(item.CompensacaoNegociada);
        participations.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{participationId:guid}/cancel")]
    public async Task<IActionResult> Cancel(long campaignId, Guid participationId, CancellationToken ct = default)
    {
        var item = await participations.FindAsync([participationId], ct) ?? throw new KeyNotFoundException();
        if (item.CampanhaId != campaignId) throw new KeyNotFoundException();
        item.Cancelar("Cancelada pela operação");
        participations.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
