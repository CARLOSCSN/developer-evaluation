using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the <see cref="SaleItem"/> class discount calculation.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that items with quantity less than 4 have no discount
    /// </summary>
    [Theory(DisplayName = "Given quantity less than 4 When calculating discount Then discount is 0%")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void CalculateDiscount_QuantityLessThan4_ShouldHaveNoDiscount(int quantity)
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = 1,
            ProductName = "Product A",
            Quantity = quantity,
            UnitPrice = 100m
        };

        // Act
        item.CalculateDiscount();

        // Assert
        item.Discount.Should().Be(0m);
    }

    /// <summary>
    /// Tests that items with quantity between 4 and 9 have 10% discount
    /// </summary>
    [Theory(DisplayName = "Given quantity between 4 and 9 When calculating discount Then discount is 10%")]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(9)]
    public void CalculateDiscount_QuantityBetween4And9_ShouldHave10PercentDiscount(int quantity)
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = 1,
            ProductName = "Product A",
            Quantity = quantity,
            UnitPrice = 100m
        };

        // Act
        item.CalculateDiscount();

        // Assert
        item.Discount.Should().Be(0.10m);
    }

    /// <summary>
    /// Tests that items with quantity between 10 and 20 have 20% discount
    /// </summary>
    [Theory(DisplayName = "Given quantity between 10 and 20 When calculating discount Then discount is 20%")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void CalculateDiscount_QuantityBetween10And20_ShouldHave20PercentDiscount(int quantity)
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = 1,
            ProductName = "Product A",
            Quantity = quantity,
            UnitPrice = 100m
        };

        // Act
        item.CalculateDiscount();

        // Assert
        item.Discount.Should().Be(0.20m);
    }

    /// <summary>
    /// Tests that items with quantity greater than 20 throw exception
    /// </summary>
    [Theory(DisplayName = "Given quantity greater than 20 When calculating discount Then throws exception")]
    [InlineData(21)]
    [InlineData(25)]
    [InlineData(100)]
    public void CalculateDiscount_QuantityGreaterThan20_ShouldThrowException(int quantity)
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = 1,
            ProductName = "Product A",
            Quantity = quantity,
            UnitPrice = 100m
        };

        // Act & Assert
        var act = () => item.CalculateDiscount();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot sell more than 20 identical items.");
    }

    /// <summary>
    /// Tests that CalculateTotalAmount correctly calculates with discount
    /// </summary>
    [Fact(DisplayName = "Given valid item When calculating total Then applies discount correctly")]
    public void CalculateTotalAmount_ValidItem_ShouldCalculateCorrectly()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = 1,
            ProductName = "Product A",
            Quantity = 10, // Should get 20% discount
            UnitPrice = 100m
        };

        // Act
        item.CalculateTotalAmount();

        // Assert
        item.Discount.Should().Be(0.20m);
        item.TotalItemAmount.Should().Be(800m); // 10 * 100 * 0.80 = 800
    }
}
