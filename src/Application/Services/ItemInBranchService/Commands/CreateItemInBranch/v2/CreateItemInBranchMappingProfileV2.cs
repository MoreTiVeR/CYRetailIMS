using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v2;

public class CreateItemInBranchMappingProfileV2 : Profile
{
    public CreateItemInBranchMappingProfileV2()
    {
        CreateMap<CreateItemInBranchDetailCommandV2, TMItemInBranch>()
            .ForMember(m => m.BranchID, f => f.MapFrom(x => x.branchid))
            .ForMember(m => m.ItemID, f => f.MapFrom(x => x.itemid))
            .ForMember(m => m.Price, f => f.MapFrom(x => x.price))
            .ForMember(m => m.Qty, f => f.MapFrom(x => x.qty))
            .ForMember(m => m.NotifyMinQty, f => f.MapFrom(x => x.notifyminqty))
            .ForMember(m => m.NotifyMaxQty, f => f.MapFrom(x => x.notifymaxqty))
            .ForMember(m => m.DiscountPercent, f => f.MapFrom(x => x.discountpercent))
            .ForMember(m => m.CreatedBy, f => f.MapFrom(x => x.createdby))
            .ForMember(m => m.IsActive, f => f.MapFrom(x => x.isactive));
    }
}
