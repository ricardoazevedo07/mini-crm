using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/proposals")]
public sealed class ProposalsController(IRepository<Proposta> repository, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct = default) => Ok(await repository.ListAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct);
        return item is null ? NotFound(new { code = "not_found" }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProposalRequest request, CancellationToken ct = default)
    {
        var item = new Proposta { OportunidadeId = request.OportunidadeId, ValorTotal = new Dinheiro(request.Valor, request.Moeda) };
        await repository.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPost("{id:long}/send")]
    public async Task<IActionResult> Send(long id, CancellationToken ct = default) => await ChangeStatus(id, "Enviada", ct);

    [HttpPost("{id:long}/approve")]
    public async Task<IActionResult> Approve(long id, CancellationToken ct = default) => await ChangeStatus(id, "Aprovada", ct);

    [HttpPost("{id:long}/reject")]
    public async Task<IActionResult> Reject(long id, CancellationToken ct = default) => await ChangeStatus(id, "Rejeitada", ct);

    private async Task<IActionResult> ChangeStatus(long id, string status, CancellationToken ct)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Status = status;
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
