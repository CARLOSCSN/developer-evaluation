using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetSalesRequestProfile : Profile
{
    public GetSalesRequestProfile()
    {
        CreateMap<GetSalesResult, GetSalesResponse>();
        CreateMap<Application.Sales.GetSales.SaleDto, Features.Sales.GetSales.SaleDto>();
    }
}
