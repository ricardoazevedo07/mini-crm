using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/proposals/{proposalId:long}/items")]
public sealed class ProposalItemsController(IRepository<Proposta> proposals, IRepository<ItemProposta> items, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(long proposalId, CancellationToken ct = default)
    {
        var proposal = await proposals.FindAsync([proposalId], ct) ?? throw new KeyNotFoundException();
        return Ok(proposal.Itens);
    }

    [HttpPost]
    public async Task<IActionResult> Create(long proposalId, [FromBody] ProposalItemRequest request, CancellationToken ct = default)
    {
        var proposal = await proposals.FindAsync([proposalId], ct) ?? throw new KeyNotFoundException();
        if (proposal.Status == "Aprovada") throw new InvalidOperationException("Approved proposals cannot be edited.");
        var item = new ItemProposta { PropostaId = proposalId, Descricao = request.Descricao, Quantidade = request.Quantidade, ValorUnitario = new Dinheiro(request.ValorUnitario, request.Moeda) };
        await items.AddAsync(item, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Created($"/api/v1/proposals/{proposalId}/items/{item.Id}", item);
    }
}
