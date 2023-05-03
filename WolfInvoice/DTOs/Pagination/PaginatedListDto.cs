namespace WolfInvoice.DTOs.Pagination;

/// <summary>
/// Represents a paginated list of items along with pagination metadata.
/// </summary>
/// <typeparam name="TModel">The type of the items in the list.</typeparam>
public class PaginatedListDto<TModel>
{
    /// <summary>
    /// The list of items in the paginated list.
    /// </summary>
    public IEnumerable<TModel> Items { get; }

    /// <summary>
    /// The pagination metadata for the paginated list.
    /// </summary>
    public PaginationMeta Meta { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedListDto{TModel}"/> class with the specified items and metadata.
    /// </summary>
    /// <param name="items">The items in the paginated list.</param>
    /// <param name="meta">The pagination metadata for the paginated list.</param>
    public PaginatedListDto(IEnumerable<TModel> items, PaginationMeta meta)
    {
        Items = items;
        Meta = meta;
    }
}
