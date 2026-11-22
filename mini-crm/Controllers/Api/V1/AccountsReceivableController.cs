using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/accounts-receivable")]
public sealed class AccountsReceivableController(IRepository<ContaReceber> repository, IUnitOfWork unitOfWork) : ControllerBase
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
    public async Task<IActionResult> Create([FromBody] FinancialRequest request, CancellationToken ct = default)
    {
        var item = new ContaReceber { CampanhaId = request.CampanhaId };
        var approved = request.ValorAprovado ?? request.Valor;
        item.Configurar(new Dinheiro(request.Valor, request.Moeda), new Dinheiro(approved, request.Moeda), request.DataVencimentoUtc);
        await repository.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPost("{id:long}/payments")]
    public async Task<IActionResult> Receive(long id, [FromBody] SettlementRequest request, CancellationToken ct = default)
    {
        var item = await repository.FindAsync([id], ct) ?? throw new KeyNotFoundException();
        item.RegistrarRecebimento(new Dinheiro(request.Valor, request.Moeda));
        repository.Update(item);
        await unitOfWork.SaveChangesAsync(ct);
        return Ok(item);
    }
}
