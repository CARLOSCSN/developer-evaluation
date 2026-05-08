using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleCommand
/// </summary>
public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateSaleCommand
    /// </summary>
    public UpdateSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than 0");

        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(200)
            .WithMessage("Customer name must not exceed 200 characters");

        RuleFor(x => x.Branch)
            .NotEmpty()
            .WithMessage("Branch is required")
            .MaximumLength(100)
            .WithMessage("Branch must not exceed 100 characters");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateSaleItemValidator());
    }
}

/// <summary>
/// Validator for UpdateSaleItemCommand
/// </summary>
public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateSaleItemCommand
    /// </summary>
    public UpdateSaleItemValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0");

        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(200)
            .WithMessage("Product name must not exceed 200 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than 0");
    }
}
