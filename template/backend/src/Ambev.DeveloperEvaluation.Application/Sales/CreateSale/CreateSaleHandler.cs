using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Map command to entity
        var sale = _mapper.Map<Sale>(command);

        // Calculate discounts and totals for each item
        foreach (var item in sale.Items)
        {
            try
            {
                item.CalculateTotalAmount();
            }
            catch (InvalidOperationException ex)
            {
                throw new ValidationException($"Item validation failed for product {item.ProductName}: {ex.Message}");
            }
        }

        // Calculate total sale amount
        sale.CalculateTotalAmount();

        // Save to database
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        // Log domain event
        var saleCreatedEvent = new SaleCreatedEvent(createdSale);
        _logger.LogInformation(
            "Sale created: ID={SaleId}, SaleNumber={SaleNumber}, Customer={CustomerName}, Branch={Branch}, TotalAmount={TotalAmount}, ItemCount={ItemCount}",
            createdSale.Id,
            createdSale.SaleNumber,
            createdSale.CustomerName,
            createdSale.Branch,
            createdSale.TotalAmount,
            createdSale.Items.Count);

        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }
}
