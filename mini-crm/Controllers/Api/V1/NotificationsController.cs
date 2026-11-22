using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/notifications")]
public sealed class NotificationsController(IRepository<NotificacaoCrm> repository, IUnitOfWork unitOfWork) : ControllerBase
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
    public async Task<IActionResult> Create([FromBody] NotificationRequest request, CancellationToken ct = default)
    {
        var item = new NotificacaoCrm { Id = Guid.NewGuid(), Titulo = request.Titulo, Mensagem = request.Mensagem, CriadaEmUtc = DateTime.UtcNow };
        await repository.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPost("{id:guid}/read")]
    public async Task<IActionResult> Read(Guid id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Lida = true;
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
