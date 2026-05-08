using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Command for deleting (cancelling) a sale
/// </summary>
public class DeleteSaleCommand : IRequest<DeleteSaleResult>
{
    /// <summary>
    /// Gets or sets the sale ID to delete
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of DeleteSaleCommand
    /// </summary>
    /// <param name="id">The sale ID</param>
    public DeleteSaleCommand(Guid id)
    {
        Id = id;
    }
}
