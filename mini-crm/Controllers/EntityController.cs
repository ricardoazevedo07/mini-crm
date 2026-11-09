using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Common;
using mini_crm.Application.UseCases.Shared;

namespace mini_crm.Controllers;

public abstract class EntityController<TEntity>(GenericCrudHandler<TEntity> handler) : ControllerBase
    where TEntity : class, new()
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await handler.ListAsync(new PageRequest(page, pageSize), cancellationToken);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] System.Text.Json.JsonElement payload, CancellationToken cancellationToken = default)
    {
        var result = await handler.HandleAsync(new CrudRequest(payload), cancellationToken);
        return result.Succeeded ? Created(string.Empty, result.Value) : StatusCode(result.Error!.StatusCode, result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken = default)
    {
        var result = await handler.GetAsync(ParseKeys(id), cancellationToken);
        return result.Succeeded ? Ok(result.Value) : StatusCode(result.Error!.StatusCode, result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] System.Text.Json.JsonElement payload, CancellationToken cancellationToken = default)
    {
        var result = await handler.UpdateAsync(ParseKeys(id), new CrudRequest(payload), cancellationToken);
        return result.Succeeded ? Ok(result.Value) : StatusCode(result.Error!.StatusCode, result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await handler.DeleteAsync(ParseKeys(id), cancellationToken);
        return result.Succeeded ? NoContent() : StatusCode(result.Error!.StatusCode, result.Error);
    }

    private static object[] ParseKeys(string value) => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(key => long.TryParse(key, out var number) ? (object)number : key)
        .ToArray();
}
