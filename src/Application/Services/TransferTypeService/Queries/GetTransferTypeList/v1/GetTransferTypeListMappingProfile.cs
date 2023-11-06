using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeByID.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;
public class GetTransferTypeListMappingProfile : Profile
{
    public GetTransferTypeListMappingProfile()
    {
        CreateMap<TMTransferType, GetTransferTypeListResponseDTO>()
            .ForMember(w => w.transfertypeid, f => f.MapFrom(w => w.TransferTypeID))
            .ForMember(w => w.transfertypename, f => f.MapFrom(w => w.TransferTypeName))
            .ForMember(w => w.description, f => f.MapFrom(w => w.Description))
            .ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
            .ForMember(w => w.createddate, f => f.MapFrom(w => w.CreatedDate))
            .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));
    }
}
