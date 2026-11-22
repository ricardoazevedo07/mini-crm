using Microsoft.AspNetCore.Mvc;
using mini_crm.Application.Contracts;
using mini_crm.Application.UseCases.Customers;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/customers")]
public sealed class CustomersController(CustomerUseCases useCases) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? query, [FromQuery] bool? active,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await useCases.SearchAsync(query, active, new Application.Common.PageRequest(page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken = default)
    {
        var result = await useCases.GetAsync(id, cancellationToken);
        return result is null ? NotFound(new { code = "not_found", detail = "The requested customer was not found." }) : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var result = await useCases.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var result = await useCases.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:long}/archive")]
    public async Task<IActionResult> Archive(long id, CancellationToken cancellationToken = default)
    {
        await useCases.ArchiveAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:long}/contacts")]
    public async Task<IActionResult> GetContacts(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var contacts = await useCases.ListContactsAsync(id, cancellationToken);
            return Ok(contacts);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { code = "not_found", detail = "The requested customer was not found." });
        }
    }

    [HttpPost("{id:long}/contacts")]
    public async Task<IActionResult> CreateContact(long id, [FromBody] CreateCustomerContactRequest request, CancellationToken cancellationToken = default)
    {
        var result = await useCases.AddContactAsync(id, request, cancellationToken);
        return Created($"/api/v1/customers/{id}/contacts/{result.Id}", result);
    }

    [HttpGet("{id:long}/contacts/{contactId:guid}")]
    public async Task<IActionResult> GetContact(long id, Guid contactId, CancellationToken cancellationToken = default)
    {
        var result = await useCases.GetContactAsync(id, contactId, cancellationToken);
        return result is null ? NotFound(new { code = "not_found", detail = "The requested contact was not found." }) : Ok(result);
    }

    [HttpPut("{id:long}/contacts/{contactId:guid}")]
    public async Task<IActionResult> UpdateContact(long id, Guid contactId, [FromBody] UpdateCustomerContactRequest request, CancellationToken cancellationToken = default)
    {
        var result = await useCases.UpdateContactAsync(id, contactId, request, cancellationToken);
        return Ok(result);
    }
}
