using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Profile for mapping GetSale operations
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Sale, GetSaleResult>();
        CreateMap<SaleItem, GetSaleItemResult>();
    }
}
