using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/curation-requests")]
public sealed class CurationRequestsController(IRepository<SolicitacaoCuradoria> requests, IRepository<CandidatoCuradoria> candidates, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct = default) => Ok(await requests.ListAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct = default)
    {
        var item = await requests.FindAsync([id], ct);
        return item is null ? NotFound(new { code = "not_found" }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CurationRequestCreate request, CancellationToken ct = default)
    {
        var item = new SolicitacaoCuradoria { Id = Guid.NewGuid(), CampanhaId = request.CampanhaId, Briefing = request.Briefing };
        await requests.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpGet("{id:guid}/candidates")]
    public async Task<IActionResult> Candidates(Guid id, CancellationToken ct = default)
    {
        var items = await candidates.ListAsync(ct);
        return Ok(items.Where(x => x.SolicitacaoCuradoriaId == id).ToList());
    }
}
