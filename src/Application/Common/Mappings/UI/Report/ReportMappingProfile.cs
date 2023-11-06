using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;

namespace CYRetailIMS.Application.Common.Mappings.UI.Report;
public class ReportMappingProfile : Profile
{
    public ReportMappingProfile()
    {
        CreateMap<SaleSummaryReportResponseDTO, AuditSaleSummaryReportViewModel>()
            .ForMember(w => w.TransactionID, f => f.MapFrom(w => w.transactionid))
            .ForMember(w => w.TransactionDate, f => f.MapFrom(w => w.transactiondate))
            //.ForMember(w => w.TxnDateTime, f => f.MapFrom(w => w.transactiondate))
            .ForMember(w => w.TotalAmount, f => f.MapFrom(w => w.totalamount))
            .ForMember(w => w.AmountTransfer, f => f.MapFrom(w => w.amounttransfer))
            .ForMember(w => w.AmountDeposit, f => f.MapFrom(w => w.amountdeposit))
            .ForMember(w => w.AmountCash, f => f.MapFrom(w => w.amountcash))
            .ForMember(w => w.DepositFee, f => f.MapFrom(w => w.depositfee))
            .ForMember(w => w.BranchID, f => f.MapFrom(w => w.branchid))
            .ForMember(w => w.BranchName, f => f.MapFrom(w => w.branchname))
            .ForMember(w => w.AuditID, f => f.MapFrom(w => w.auditid))
            .ForMember(w => w.TotalAuditAmount, f => f.MapFrom(w => w.totalauditamount))
            .ForMember(w => w.AuditDescription, f => f.MapFrom(w => w.auditdescription))
            .ForMember(w => w.CreatedBy, f => f.MapFrom(w => w.createdby))
            .ForMember(w => w.CreatedbyStaff, f => f.MapFrom(w => w.createdbystaff));

        //CreateMap<AuditSaleSummaryReportViewModel, >

    }
}
