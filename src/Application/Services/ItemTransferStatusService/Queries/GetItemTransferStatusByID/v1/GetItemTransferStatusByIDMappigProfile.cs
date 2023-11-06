using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatusByID.v1;
internal class GetItemTransferStatusByIDMappigProfile : Profile
{
    public GetItemTransferStatusByIDMappigProfile()
    {
        CreateMap<TMItemTransferStatus, GetItemTransferStatusByIDResponseDTO>()
            .ForMember(w => w.transferstatusid, f => f.MapFrom(w => w.TransferStatusID))
            .ForMember(w => w.transferstatusname_th, f => f.MapFrom(w => w.TransferStatusName_TH))
            .ForMember(w => w.transferstatusname_en, f => f.MapFrom(w => w.TransferStatusName_EN))
            .ForMember(w => w.description, f => f.MapFrom(w => w.Description))
            .ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
            .ForMember(w => w.createddate, f => f.MapFrom(w => w.CreatedDate))
            .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));
    }
}
