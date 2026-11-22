using Microsoft.AspNetCore.Mvc;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Controllers.Api.V1;

[ApiController]
[Route("api/v1/influencers/{influencerId:long}/social-profiles")]
public sealed class InfluencerSocialProfilesController(IInfluencerRedeSocialRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(long influencerId, CancellationToken ct = default)
    {
        var items = await repository.ListAsync(ct);
        return Ok(items.Where(x => x.InfluencerEntidadeComercialId == influencerId).ToList());
    }
}
