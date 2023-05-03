namespace WolfInvoice.DTOs.Pagination;

/// <summary>
/// Represents pagination metadata.
/// </summary>
public class PaginationMeta
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationMeta"/> class.
    /// </summary>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The maximum number of items per page.</param>
    /// <param name="count">The total number of items.</param>
    public PaginationMeta(int page, int pageSize, int count)
    {
        Page = page;
        PageSize = pageSize;
        TotalPages = (count + pageSize - 1) / pageSize;
    }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// Gets the maximum number of items per page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; }
}
