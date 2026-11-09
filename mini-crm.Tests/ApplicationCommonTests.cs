using mini_crm.Application.Common;
using Xunit;

namespace mini_crm.Tests;

public class ApplicationCommonTests
{
    [Fact]
    public void FailurePreservesApplicationError()
    {
        var error = ApplicationErrors.NotFound("Influencer not found");
        var result = OperationResult<string>.Failure(error);

        Assert.False(result.Succeeded);
        Assert.Null(result.Value);
        Assert.Equal(404, result.Error!.StatusCode);
    }

    [Fact]
    public void PageRequestNormalizesBounds()
    {
        var request = new PageRequest(0, 500);

        Assert.Equal(1, request.NormalizedPage);
        Assert.Equal(100, request.NormalizedPageSize);
    }

    [Fact]
    public void PagedResultCalculatesTotalPages()
    {
        var result = new PagedResult<int>([1, 2], 1, 2, 5);

        Assert.Equal(3, result.TotalPages);
    }
}
