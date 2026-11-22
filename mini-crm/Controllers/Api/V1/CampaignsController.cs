using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/campaigns")]
public sealed class CampaignsController(IRepository<Campanha> campaigns, IRepository<MarcoCampanha> milestones, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct = default) => Ok(await campaigns.ListAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken ct = default)
    {
        var item = await campaigns.FindAsync([id], ct);
        return item is null ? NotFound(new { code = "not_found" }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CampaignRequest request, CancellationToken ct = default)
    {
        var item = new Campanha { OportunidadeId = request.OportunidadeId, ClienteEntidadeComercialId = request.ClienteEntidadeComercialId, ResponsavelId = request.ResponsavelId };
        var type = request.TipoTaxa.Equals("percentual", StringComparison.OrdinalIgnoreCase) ? TipoTaxa.Percentual : TipoTaxa.Fixa;
        item.Configurar(request.Frente, request.DataInicio, request.DataFim, new Dinheiro(request.TotalAprovado, request.Moeda), type, request.Taxa);
        await campaigns.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPost("{id:long}/start")]
    public async Task<IActionResult> Start(long id, CancellationToken ct = default) => await ChangeStatus(id, campaign => campaign.Iniciar(), ct);

    [HttpPost("{id:long}/complete")]
    public async Task<IActionResult> Complete(long id, CancellationToken ct = default)
    {
        var item = await campaigns.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Concluir(true, new Dinheiro(0, item.TotalAprovado.CodigoMoeda));
        campaigns.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:long}/cancel")]
    public async Task<IActionResult> Cancel(long id, CancellationToken ct = default)
    {
        var item = await campaigns.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Cancelar("Cancelada pela operação");
        campaigns.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpGet("{id:long}/milestones")]
    public async Task<IActionResult> Milestones(long id, CancellationToken ct = default)
    {
        var items = await milestones.ListAsync(ct);
        return Ok(items.Where(x => x.CampanhaId == id).OrderBy(x => x.DataLimiteUtc).ToList());
    }

    private async Task<IActionResult> ChangeStatus(long id, Action<Campanha> change, CancellationToken ct)
    {
        var item = await campaigns.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        change(item);
        campaigns.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
