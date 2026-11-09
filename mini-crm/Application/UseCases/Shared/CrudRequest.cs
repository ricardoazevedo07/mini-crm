using System.Text.Json;

namespace mini_crm.Application.UseCases.Shared;

public sealed record CrudRequest(JsonElement Payload);
