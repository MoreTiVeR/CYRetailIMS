using AutoMapper;
using CYRetailIMS.Application.Services.TransactionTypeService.Queries.GetTrasnactionList.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.TransactionTypeService.Queries.GetTrasnactionByCriteria.v1;
public class GetTrasnactionByCriteriaMappingProfile : Profile
{
    public GetTrasnactionByCriteriaMappingProfile()
    {
        CreateMap<TMTransactionType, GetTrasnactionByCriteriaResponseDTO>()
            .ForMember(s => s.transactiontypeid, f => f.MapFrom(x => x.TransactionTypeID))
            .ForMember(s => s.transactiontypecode, f => f.MapFrom(x => x.TransactionTypeCode))
            .ForMember(s => s.transactiontypename, f => f.MapFrom(x => x.TransactionTypeName))
            .ForMember(s => s.desc, f => f.MapFrom(x => x.Description))
            .ForMember(s => s.isactive, f => f.MapFrom(x => x.IsActive));
    }
}
