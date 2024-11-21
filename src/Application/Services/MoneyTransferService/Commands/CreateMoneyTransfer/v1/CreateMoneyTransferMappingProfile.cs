using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
public class CreateMoneyTransferMappingProfile : Profile
{
    public CreateMoneyTransferMappingProfile()
    {
        CreateMap<CreateMoneyTransferCommand, TTMoneyTransfer>()
            .ForMember(w => w.BranchID, f => f.MapFrom(s => s.branchid))
            .ForMember(w => w.TransferDate, f => f.MapFrom(s => s.transferdate))
            .ForMember(w => w.AmountTransfer, f => f.MapFrom(s => s.amounttransfer))
            .ForMember(w => w.Description, f => f.MapFrom(s => s.description))
            .ForMember(w => w.SlipImagePath, f => f.MapFrom(s => s.slipimagepath))
            .ForMember(w => w.CreatedBy, f => f.MapFrom(s => s.createdby));
    }
}
