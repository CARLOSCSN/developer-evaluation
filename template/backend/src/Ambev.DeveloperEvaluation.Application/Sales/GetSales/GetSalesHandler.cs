using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesCommand requests
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSalesHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSalesCommand request
    /// </summary>
    /// <param name="command">The GetSales command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated list of sales</returns>
    public async Task<GetSalesResult> Handle(GetSalesCommand command, CancellationToken cancellationToken)
    {
        var (sales, totalCount) = await _saleRepository.GetAllAsync(
            command.Page,
            command.Size,
            command.Branch,
            command.CustomerId,
            command.MinDate,
            command.MaxDate,
            command.Cancelled,
            command.Order,
            cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)command.Size);

        return new GetSalesResult
        {
            Sales = _mapper.Map<List<SaleDto>>(sales),
            TotalItems = totalCount,
            CurrentPage = command.Page,
            TotalPages = totalPages
        };
    }
}
