using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Profile for mapping GetSales operations
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<GetSalesResult, GetSalesResponse>();
        CreateMap<Application.Sales.GetSales.SaleDto, SaleDto>();
    }
}
