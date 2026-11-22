using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/curation-candidates")]
public sealed class CurationCandidatesController(IRepository<CandidatoCuradoria> candidates, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct = default)
    {
        var item = await candidates.FindAsync([id], ct);
        return item is null ? NotFound(new { code = "not_found" }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CurationCandidateRequest request, CancellationToken ct = default)
    {
        var existing = await candidates.ListAsync(ct);
        if (existing.Any(x => x.SolicitacaoCuradoriaId == request.SolicitacaoCuradoriaId && x.InfluencerEntidadeComercialId == request.InfluencerEntidadeComercialId))
            throw new InvalidOperationException("Duplicate curation candidate.");
        var item = new CandidatoCuradoria { Id = Guid.NewGuid(), SolicitacaoCuradoriaId = request.SolicitacaoCuradoriaId, InfluencerEntidadeComercialId = request.InfluencerEntidadeComercialId, Avaliacao = request.Avaliacao };
        await candidates.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken ct = default) => await ChangeStatus(id, "Aprovado", ct);

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, CancellationToken ct = default) => await ChangeStatus(id, "Rejeitado", ct);

    private async Task<IActionResult> ChangeStatus(Guid id, string status, CancellationToken ct)
    {
        var item = await candidates.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Status = status;
        candidates.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
