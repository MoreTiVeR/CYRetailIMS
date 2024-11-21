using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;

namespace CYRetailIMS.Application.Common.Mappings.UI.MoneyTransfer;
public class EditMoneyTransferMappingProfile : Profile
{
    public EditMoneyTransferMappingProfile()
    {
        CreateMap<GetMoneyTransferByCriteriaResponseDTO, EditMoneyTransferViewModel>()
            .ForMember(w => w.MoneyTransferID, f => f.MapFrom(s => s.moneytransferid))
            .ForMember(w => w.BranchID, f => f.MapFrom(s => s.branchid))
            .ForMember(w => w.AmountTransfer, f => f.MapFrom(s => s.amounttransfer))
            //.ForMember(w => w.ImageFile, f => f.MapFrom(s => s.slipimagepath))
            .ForMember(w => w.TransferDate, f => f.MapFrom(s => s.transferdate))
            .ForMember(w => w.Description, f => f.MapFrom(s => s.description))
            .ForMember(w => w.IsActive, f => f.MapFrom(s => s.isactive));
    }
}
