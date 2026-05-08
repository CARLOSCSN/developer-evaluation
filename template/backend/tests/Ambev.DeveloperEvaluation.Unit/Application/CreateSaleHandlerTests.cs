using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _logger);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully with discounts applied
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success with calculated discounts")]
    public async Task Handle_ValidRequest_ReturnsSuccessWithDiscounts()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = 1,
            CustomerName = "Test Customer",
            Branch = "Branch A",
            Date = DateTime.UtcNow,
            Items = new List<CreateSaleItemCommand>
            {
                new CreateSaleItemCommand
                {
                    ProductId = 1,
                    ProductName = "Product A",
                    Quantity = 10, // Should get 20% discount
                    UnitPrice = 100m
                }
            }
        };

        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = "TEST123",
            CustomerId = command.CustomerId,
            CustomerName = command.CustomerName,
            Branch = command.Branch,
            Date = command.Date,
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = 1,
                    ProductName = "Product A",
                    Quantity = 10,
                    UnitPrice = 100m,
                    Discount = 0.20m,
                    TotalItemAmount = 800m
                }
            },
            TotalAmount = 800m
        };

        var result = new CreateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            TotalAmount = sale.TotalAmount
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(sale.Id);
        response.TotalAmount.Should().Be(800m);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that invalid sale creation request with quantity > 20 throws validation exception
    /// </summary>
    [Fact(DisplayName = "Given invalid quantity over 20 When creating sale Then throws validation exception")]
    public async Task Handle_QuantityOver20_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = 1,
            CustomerName = "Test Customer",
            Branch = "Branch A",
            Date = DateTime.UtcNow,
            Items = new List<CreateSaleItemCommand>
            {
                new CreateSaleItemCommand
                {
                    ProductId = 1,
                    ProductName = "Product A",
                    Quantity = 25, // Exceeds maximum
                    UnitPrice = 100m
                }
            }
        };

        var sale = new Sale
        {
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = 1,
                    ProductName = "Product A",
                    Quantity = 25,
                    UnitPrice = 100m
                }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
