using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale.
/// Contains business rules for discount calculation based on quantity.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the Sale ID (foreign key).
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the Sale navigation property.
    /// </summary>
    public Sale Sale { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product ID (External Identity).
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name (denormalized).
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage (0.0 to 1.0).
    /// Calculated automatically based on quantity.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item (after discount).
    /// </summary>
    public decimal TotalItemAmount { get; set; }

    /// <summary>
    /// Gets or sets whether this item is cancelled.
    /// </summary>
    public bool Cancelled { get; set; }

    /// <summary>
    /// Calculates the discount based on quantity.
    /// Business Rules:
    /// - Quantity less than 4: no discount
    /// - Quantity between 4 and 9: 10% discount
    /// - Quantity between 10 and 20: 20% discount
    /// - Quantity greater than 20: throws exception (not allowed)
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when quantity exceeds 20 items</exception>
    public void CalculateDiscount()
    {
        if (Quantity > 20)
        {
            throw new InvalidOperationException("Cannot sell more than 20 identical items.");
        }

        if (Quantity < 4)
        {
            Discount = 0;
        }
        else if (Quantity >= 4 && Quantity < 10)
        {
            Discount = 0.10m;
        }
        else if (Quantity >= 10 && Quantity <= 20)
        {
            Discount = 0.20m;
        }
    }

    /// <summary>
    /// Calculates the total amount for this item after applying discount.
    /// </summary>
    public void CalculateTotalAmount()
    {
        CalculateDiscount();
        TotalItemAmount = Quantity * UnitPrice * (1 - Discount);
    }

    /// <summary>
    /// Cancels this item logically.
    /// </summary>
    public void Cancel()
    {
        Cancelled = true;
    }
}
