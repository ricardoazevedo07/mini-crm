using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/deliverables")]
public sealed class DeliverablesController(IRepository<EntregaCampanha> repository, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct = default) => Ok(await repository.ListAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct);
        return item is null ? NotFound(new { code = "not_found" }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DeliveryRequest request, CancellationToken ct = default)
    {
        var item = new EntregaCampanha { Id = Guid.NewGuid(), CampanhaId = request.CampanhaId, ParticipacaoCampanhaId = request.ParticipacaoCampanhaId };
        item.Configurar(request.Titulo, request.DataLimiteUtc);
        await repository.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> Submit(Guid id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Iniciar();
        item.EnviarParaAprovacao();
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Aprovar();
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Rejeitar("Rejeitada pela operação");
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> Publish(Guid id, [FromBody] PublishDeliveryRequest request, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Publicar(request.Evidencia, request.PublicadoEmUtc?.ToUniversalTime() ?? DateTime.UtcNow);
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
