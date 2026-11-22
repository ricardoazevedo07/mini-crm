using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/leads")]
public sealed class LeadsController(IRepository<Lead> repository, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var items = await repository.ListAsync(ct);
        var filtered = items.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return Ok(new { items = filtered, page, pageSize, totalCount = items.Count, totalPages = (int)Math.Ceiling(items.Count / (double)pageSize) });
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct);
        return item is null ? NotFound(new { code = "not_found" }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LeadRequest request, CancellationToken ct = default)
    {
        var item = new Lead { Nome = request.Nome, Empresa = request.Empresa, Email = request.Email, Telefone = request.Telefone, ResponsavelId = request.ResponsavelId };
        await repository.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, LeadRequest request, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Nome = request.Nome;
        item.Empresa = request.Empresa;
        item.Email = request.Email;
        item.Telefone = request.Telefone;
        item.ResponsavelId = request.ResponsavelId;
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:long}/qualify")]
    public async Task<IActionResult> Qualify(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Status = "Qualificado";
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:long}/lose")]
    public async Task<IActionResult> Lose(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Status = "Perdido";
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
