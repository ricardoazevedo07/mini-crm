namespace mini_crm.Application.Common;

public sealed record PageRequest(int Page = 1, int PageSize = 20)
{
    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentOutOfRangeException(nameof(Page), "Page must be greater than or equal to 1.");
        if (PageSize is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(PageSize), "PageSize must be between 1 and 100.");
    }
}

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount)
{
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
