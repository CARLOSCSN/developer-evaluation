using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Profile for mapping UpdateSale operations
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleItemCommand, SaleItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SaleId, opt => opt.Ignore())
            .ForMember(dest => dest.Sale, opt => opt.Ignore())
            .ForMember(dest => dest.Discount, opt => opt.Ignore())
            .ForMember(dest => dest.TotalItemAmount, opt => opt.Ignore())
            .ForMember(dest => dest.Cancelled, opt => opt.Ignore());

        CreateMap<Sale, UpdateSaleResult>();
        CreateMap<SaleItem, UpdateSaleItemResult>();
    }
}
