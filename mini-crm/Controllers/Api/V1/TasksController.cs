using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/tasks")]
public sealed class TasksController(IRepository<TarefaCrm> repository, IUnitOfWork unitOfWork) : ControllerBase
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
    public async Task<IActionResult> Create([FromBody] TaskRequest request, CancellationToken ct = default)
    {
        var item = new TarefaCrm { Id = Guid.NewGuid(), Titulo = request.Titulo, Descricao = request.Descricao, ResponsavelId = request.ResponsavelId, PrazoUtc = request.PrazoUtc };
        await repository.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPost("{id:guid}/assign")]
    public async Task<IActionResult> Assign(Guid id, [FromBody] TaskRequest request, CancellationToken ct = default) => await Change(id, item => { item.ResponsavelId = request.ResponsavelId; item.Status = "Atribuída"; }, ct);

    [HttpPost("{id:guid}/start")]
    public async Task<IActionResult> Start(Guid id, CancellationToken ct = default) => await Change(id, item => item.Status = "Em andamento", ct);

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken ct = default) => await Change(id, item => item.Status = "Concluída", ct);

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct = default) => await Change(id, item => item.Status = "Cancelada", ct);

    private async Task<IActionResult> Change(Guid id, Action<TarefaCrm> change, CancellationToken ct)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        change(item);
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
