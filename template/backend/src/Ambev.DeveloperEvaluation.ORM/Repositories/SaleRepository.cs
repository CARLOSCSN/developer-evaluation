using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all sales with optional filtering and pagination
    /// </summary>
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetAllAsync(
        int page = 1,
        int size = 10,
        string? branch = null,
        int? customerId = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        bool? cancelled = null,
        string? order = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.Include(s => s.Items).AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(branch))
        {
            query = query.Where(s => s.Branch.Contains(branch));
        }

        if (customerId.HasValue)
        {
            query = query.Where(s => s.CustomerId == customerId.Value);
        }

        if (minDate.HasValue)
        {
            query = query.Where(s => s.Date >= minDate.Value);
        }

        if (maxDate.HasValue)
        {
            query = query.Where(s => s.Date <= maxDate.Value);
        }

        if (cancelled.HasValue)
        {
            query = query.Where(s => s.Cancelled == cancelled.Value);
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply ordering
        if (!string.IsNullOrWhiteSpace(order))
        {
            query = ApplyOrdering(query, order);
        }
        else
        {
            // Default ordering
            query = query.OrderByDescending(s => s.Date);
        }

        // Apply pagination
        var sales = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (sales, totalCount);
    }

    /// <summary>
    /// Updates an existing sale
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Deletes (cancels) a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        sale.Cancel();
        await UpdateAsync(sale, cancellationToken);
        return true;
    }

    /// <summary>
    /// Applies dynamic ordering to the query based on the order string
    /// </summary>
    /// <param name="query">The query to apply ordering to</param>
    /// <param name="order">Order clause (e.g., "date desc, saleNumber asc")</param>
    /// <returns>The ordered query</returns>
    private IQueryable<Sale> ApplyOrdering(IQueryable<Sale> query, string order)
    {
        var orderParts = order.Split(',', StringSplitOptions.RemoveEmptyEntries);
        IOrderedQueryable<Sale>? orderedQuery = null;

        foreach (var part in orderParts)
        {
            var trimmed = part.Trim();
            var tokens = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (tokens.Length == 0) continue;

            var property = tokens[0].ToLower();
            var direction = tokens.Length > 1 && tokens[1].ToLower() == "desc" ? "desc" : "asc";

            if (orderedQuery == null)
            {
                orderedQuery = ApplyOrderByProperty(query, property, direction);
            }
            else
            {
                orderedQuery = ApplyThenByProperty(orderedQuery, property, direction);
            }
        }

        return orderedQuery ?? query.OrderByDescending(s => s.Date);
    }

    /// <summary>
    /// Applies OrderBy to a property dynamically
    /// </summary>
    private IOrderedQueryable<Sale> ApplyOrderByProperty(IQueryable<Sale> query, string property, string direction)
    {
        return property switch
        {
            "date" => direction == "desc" ? query.OrderByDescending(s => s.Date) : query.OrderBy(s => s.Date),
            "salenumber" => direction == "desc" ? query.OrderByDescending(s => s.SaleNumber) : query.OrderBy(s => s.SaleNumber),
            "customer" or "customername" => direction == "desc" ? query.OrderByDescending(s => s.CustomerName) : query.OrderBy(s => s.CustomerName),
            "customerid" => direction == "desc" ? query.OrderByDescending(s => s.CustomerId) : query.OrderBy(s => s.CustomerId),
            "branch" => direction == "desc" ? query.OrderByDescending(s => s.Branch) : query.OrderBy(s => s.Branch),
            "totalamount" or "total" => direction == "desc" ? query.OrderByDescending(s => s.TotalAmount) : query.OrderBy(s => s.TotalAmount),
            "cancelled" => direction == "desc" ? query.OrderByDescending(s => s.Cancelled) : query.OrderBy(s => s.Cancelled),
            _ => query.OrderByDescending(s => s.Date)
        };
    }

    /// <summary>
    /// Applies ThenBy to a property dynamically
    /// </summary>
    private IOrderedQueryable<Sale> ApplyThenByProperty(IOrderedQueryable<Sale> query, string property, string direction)
    {
        return property switch
        {
            "date" => direction == "desc" ? query.ThenByDescending(s => s.Date) : query.ThenBy(s => s.Date),
            "salenumber" => direction == "desc" ? query.ThenByDescending(s => s.SaleNumber) : query.ThenBy(s => s.SaleNumber),
            "customer" or "customername" => direction == "desc" ? query.ThenByDescending(s => s.CustomerName) : query.ThenBy(s => s.CustomerName),
            "customerid" => direction == "desc" ? query.ThenByDescending(s => s.CustomerId) : query.ThenBy(s => s.CustomerId),
            "branch" => direction == "desc" ? query.ThenByDescending(s => s.Branch) : query.ThenBy(s => s.Branch),
            "totalamount" or "total" => direction == "desc" ? query.ThenByDescending(s => s.TotalAmount) : query.ThenBy(s => s.TotalAmount),
            "cancelled" => direction == "desc" ? query.ThenByDescending(s => s.Cancelled) : query.ThenBy(s => s.Cancelled),
            _ => query.ThenByDescending(s => s.Date)
        };
    }
}
