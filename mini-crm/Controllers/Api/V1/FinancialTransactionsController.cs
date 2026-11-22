using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/financial-transactions")]
public sealed class FinancialTransactionsController(IRepository<LancamentoLiquidacao> transactions, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct = default) => Ok(await transactions.ListAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct = default)
    {
        var item = await transactions.FindAsync([id], ct);
        return item is null ? NotFound(new { code = "not_found" }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TransactionRequest request, CancellationToken ct = default)
    {
        var item = new LancamentoLiquidacao { Id = Guid.NewGuid(), ContaReceberId = request.ContaReceberId ?? 0, ContaPagarId = request.ContaPagarId, RepasseCreatorId = request.RepasseCreatorId };
        item.Configurar(new Dinheiro(request.Valor, request.Moeda), DateTime.UtcNow, request.Referencia);
        await transactions.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }
}
