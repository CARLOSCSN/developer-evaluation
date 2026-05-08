using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Command for retrieving sales with filtering and pagination
/// </summary>
public class GetSalesCommand : IRequest<GetSalesResult>
{
    /// <summary>
    /// Gets or sets the page number (1-based)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the optional branch filter
    /// </summary>
    public string? Branch { get; set; }

    /// <summary>
    /// Gets or sets the optional customer ID filter
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the optional minimum date filter
    /// </summary>
    public DateTime? MinDate { get; set; }

    /// <summary>
    /// Gets or sets the optional maximum date filter
    /// </summary>
    public DateTime? MaxDate { get; set; }

    /// <summary>
    /// Gets or sets the optional cancelled status filter
    /// </summary>
    public bool? Cancelled { get; set; }
}
