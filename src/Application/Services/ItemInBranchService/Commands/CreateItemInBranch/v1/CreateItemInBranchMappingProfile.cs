using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v1;

public class CreateItemInBranchMappingProfile : Profile
{
    public CreateItemInBranchMappingProfile()
    {
        CreateMap<CreateItemInBranchDetailCommand, TMItemInBranch>()
            .ForMember(m => m.BranchID, f => f.MapFrom(x => x.branchid))
            .ForMember(m => m.ItemID, f => f.MapFrom(x => x.itemid))
            .ForMember(m => m.Price, f => f.MapFrom(x => x.price))
            .ForMember(m => m.Qty, f => f.MapFrom(x => x.qty))
            .ForMember(m => m.NotifyMinQty, f => f.MapFrom(x => x.notifyminqty))
            .ForMember(m => m.NotifyMaxQty, f => f.MapFrom(x => x.notifymaxqty))
            .ForMember(m => m.DiscountPercent, f => f.MapFrom(x => x.discountpercent))
            .ForMember(m => m.CreatedBy, f => f.MapFrom(x => x.createdby))
            .ForMember(m => m.IsActive, f => f.MapFrom(x => x.isactive));

        //CreateMap<TMItemInBranch, TTStockImportHistory>()
        //    .ForMember(s => s.BranchID, f => f.MapFrom(ss => ss.BranchID))
        //    .ForMember(s => s.ItemID, f => f.MapFrom(ss => ss.ItemID))
        //    .ForMember(s => s.Price, f => f.MapFrom(ss => ss.Price))
        //    .ForMember(s => s.DiscountPercent, f => f.MapFrom(ss => ss.DiscountPercent))
        //    .ForMember(s => s.Qty, f => f.MapFrom(ss => ss.Qty))
        //    .ForMember(s => s.NotifyMinQty, f => f.MapFrom(ss => ss.NotifyMinQty))
        //    .ForMember(s => s.NotifyMaxQty, f => f.MapFrom(ss => ss.NotifyMaxQty));
    }
}
