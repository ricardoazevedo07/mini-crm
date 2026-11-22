using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/contracts")]
public sealed class ContractsController(IContratoRepository repository, IUnitOfWork unitOfWork) : ControllerBase
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
    public async Task<IActionResult> Create([FromBody] ContractRequest request, CancellationToken ct = default)
    {
        var item = new Contrato { ClienteEntidadeComercialId = request.ClienteEntidadeComercialId, AgenciaEntidadeComercialId = request.AgenciaEntidadeComercialId, AgenteId = request.AgenteId, InfluencerEntidadeComercialId = request.InfluencerEntidadeComercialId, ConteudoContrato = request.ConteudoContrato };
        await repository.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPost("{id:long}/send-for-signature")]
    public async Task<IActionResult> SendForSignature(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.StatusCrm = StatusContratoCrm.EmRevisao;
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:long}/register-signature")]
    public async Task<IActionResult> RegisterSignature(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Assinar(DateTime.UtcNow);
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }

    [HttpPost("{id:long}/cancel")]
    public async Task<IActionResult> Cancel(long id, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.Cancelar("Cancelado pela operação");
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
