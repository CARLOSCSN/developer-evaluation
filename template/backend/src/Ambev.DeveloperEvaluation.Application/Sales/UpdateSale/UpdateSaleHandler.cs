using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Get existing sale
        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        if (existingSale.Cancelled)
            throw new InvalidOperationException("Cannot update a cancelled sale");

        // Update sale properties
        existingSale.CustomerId = command.CustomerId;
        existingSale.CustomerName = command.CustomerName;
        existingSale.Branch = command.Branch;
        existingSale.Date = command.Date;

        // Clear and recreate items
        existingSale.Items.Clear();
        var newItems = _mapper.Map<List<SaleItem>>(command.Items);
        
        foreach (var item in newItems)
        {
            item.SaleId = existingSale.Id;
            try
            {
                item.CalculateTotalAmount();
            }
            catch (InvalidOperationException ex)
            {
                throw new ValidationException($"Item validation failed for product {item.ProductName}: {ex.Message}");
            }
            existingSale.Items.Add(item);
        }

        // Recalculate total
        existingSale.CalculateTotalAmount();
        existingSale.MarkAsUpdated();

        // Save to database
        var updatedSale = await _saleRepository.UpdateAsync(existingSale, cancellationToken);

        // Log domain event
        var saleModifiedEvent = new SaleModifiedEvent(updatedSale);
        _logger.LogInformation(
            "Sale modified: ID={SaleId}, SaleNumber={SaleNumber}, Customer={CustomerName}, Branch={Branch}, TotalAmount={TotalAmount}, ItemCount={ItemCount}",
            updatedSale.Id,
            updatedSale.SaleNumber,
            updatedSale.CustomerName,
            updatedSale.Branch,
            updatedSale.TotalAmount,
            updatedSale.Items.Count);

        var result = _mapper.Map<UpdateSaleResult>(updatedSale);
        return result;
    }
}
