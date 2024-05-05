using System;
using System.Collections.Generic;

using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByCriteria.v1;

namespace CYRetailIMS.Application.Common.Mappings.UI.Transaction;
public class TransactionsMappingProfile : Profile
{
    public TransactionsMappingProfile()
    {
        CreateMap<GetTransactionByCriteriaResponseDTO, EditTransactionViewModel>()
            .ForMember(w => w.BranchID, f => f.MapFrom(s => s.branchid))
            .ForMember(w => w.TransactionDate, f => f.MapFrom(s => s.transactiondate.ToDateString()))
            .ForMember(w => w.TotalAmount, f => f.MapFrom(s => s.totalamount))
            .ForMember(w => w.AmountTransfer, f => f.MapFrom(s => s.amounttransfer))
            .ForMember(w => w.AmountDeposit, f => f.MapFrom(s => s.amountdeposit))
            .ForMember(w => w.AmountFee, f => f.MapFrom(s => s.depositfee))
            .ForMember(w => w.AmountCash, f => f.MapFrom(s => s.amountcash))
            .ForMember(w => w.Remark, f => f.MapFrom(s => s.remark))
            .ForMember(w => w.Detail, f => f.MapFrom(s => s.detail));

        CreateMap<GetTransactionDetailResponseDTO, EditTransactionDetailViewModel>()
            //.ForMember(w => w.TransactionID, f => f.MapFrom(s => s.transactiondetailid))
            .ForMember(w => w.TransactionDetailID, f => f.MapFrom(s => s.transactiondetailid))
            .ForMember(w => w.ItemName, f => f.MapFrom(s => s.itemname))
            .ForMember(w => w.Price, f => f.MapFrom(s => s.price))
            .ForMember(w => w.Qty, f => f.MapFrom(s => s.qty))
            .ForMember(w => w.Amount, f => f.MapFrom(s => s.amount));
    }
}
