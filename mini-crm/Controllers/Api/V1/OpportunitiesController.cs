using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/opportunities")]
public sealed class OpportunitiesController(IRepository<Oportunidade> repository, IRepository<Campanha> campaigns, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var items = await repository.ListAsync(ct);
        var data = items.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return Ok(new { items = data, page, pageSize, totalCount = items.Count, totalPages = (int)Math.Ceiling(items.Count / (double)pageSize) });
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct);
        return item is null ? NotFound(new { code = "not_found" }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OpportunityCreateRequest request, CancellationToken ct = default)
    {
        var item = new Oportunidade { ClienteEntidadeComercialId = request.ClienteEntidadeComercialId, ResponsavelId = request.ResponsavelId };
        item.Qualificar(request.Briefing, new Dinheiro(request.Orcamento, request.Moeda));
        await repository.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPost("{id:long}/win")]
    public async Task<IActionResult> Win(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.MarcarGanha();
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:long}/lose")]
    public async Task<IActionResult> Lose(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Perder("Perdida pela operação comercial");
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:long}/convert-to-campaign")]
    public async Task<IActionResult> ConvertToCampaign(long id, CancellationToken ct = default)
    {
        var opportunity = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        opportunity.MarcarGanha();
        var campaign = new Campanha { OportunidadeId = opportunity.Id, ClienteEntidadeComercialId = opportunity.ClienteEntidadeComercialId, ResponsavelId = opportunity.ResponsavelId };
        campaign.Configurar("Geral", DateTime.UtcNow, null, new Dinheiro(opportunity.OrcamentoEstimado.Valor, opportunity.OrcamentoEstimado.CodigoMoeda), TipoTaxa.Fixa, 0);
        await campaigns.AddAsync(campaign, ct);
        await unitOfWork.SaveChangesAsync(ct);
        opportunity.VincularCampanha(campaign.Id);
        repository.Update(opportunity);
        await unitOfWork.SaveChangesAsync(ct);
        return Created($"/api/v1/campaigns/{campaign.Id}", campaign);
    }
}
