using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command for retrieving a sale by ID
/// </summary>
public class GetSaleCommand : IRequest<GetSaleResult>
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of GetSaleCommand
    /// </summary>
    /// <param name="id">The sale ID</param>
    public GetSaleCommand(Guid id)
    {
        Id = id;
    }
}
