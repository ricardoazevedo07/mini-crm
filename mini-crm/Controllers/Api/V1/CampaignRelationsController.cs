using Microsoft.AspNetCore.Mvc;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/campaigns/{id:long}")]
public sealed class CampaignRelationsController(IRepository<Campanha> campaigns, IRepository<EntregaCampanha> deliverables, IRepository<ContaReceber> receivables, IRepository<ContaPagar> payables) : ControllerBase
{
    [HttpGet("timeline")]
    public async Task<IActionResult> Timeline(long id, CancellationToken ct = default)
    {
        var campaign = await campaigns.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        var deliveries = await deliverables.ListAsync(ct);
        return Ok(new { campaignId = id, campaignStatus = campaign.Status, deliveries = deliveries.Where(x => x.CampanhaId == id).OrderBy(x => x.DataLimiteUtc).ToList() });
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary(long id, CancellationToken ct = default)
    {
        var campaign = await campaigns.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        var receivable = await receivables.ListAsync(ct);
        var payable = await payables.ListAsync(ct);
        return Ok(new { campaignId = id, status = campaign.Status, accountsReceivable = receivable.Where(x => x.CampanhaId == id).Sum(x => x.Saldo.Valor), accountsPayable = payable.Where(x => x.CampanhaId == id).Sum(x => x.Saldo().Valor) });
    }
}
