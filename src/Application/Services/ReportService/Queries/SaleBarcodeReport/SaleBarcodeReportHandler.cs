using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport;
public class SaleBarcodeReportHandler : BaseService, IRequestHandler<SaleBarcodeReportQuery, BaseResponse<SaleBarcodeReportResponseDTO>>
{
    public SaleBarcodeReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<SaleBarcodeReportResponseDTO>> Handle(SaleBarcodeReportQuery request, CancellationToken cancellationToken)
    {
        int totalRow = 0;
        IEnumerable<SaleBarcodeReportResponseDetailDTO> resEodSummaryReport = (from eodsummary in await _unitOfWork.Repository<TTEndOfDaySummary>().QueryAsync()
                                                                               //join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
                                                                               join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on eodsummary.BranchID equals branch.BranchID
                                                                               join audit in await _unitOfWork.Repository<TTTransactionAudit>().QueryAsync(w => w.IsActive)
                                                                               on new { eodsummary.BranchID, eodsummary.SummaryDate.Date } equals new { audit.BranchID, audit.TransactionDate.Date } into tAudit
                                                                               from jAudit in tAudit.DefaultIfEmpty()
                                                                               where eodsummary.IsActive
                                                                               && (eodsummary.SummaryDate.Date >= request.transaction_startdate.Date && eodsummary.SummaryDate.Date <= request.transaction_enddate.Date)
                                                                               select new SaleBarcodeReportResponseDetailDTO
                                                                               {
                                                                                   transactionid = eodsummary.EndOfDayId,
                                                                                   transactiondate = eodsummary.SummaryDate,
                                                                                   branchid = eodsummary.BranchID,
                                                                                   branchname = branch.BranchName,
                                                                                   username = eodsummary.CreatedBy, //ชื่อพนักงาน
                                                                                   amountcash = eodsummary.TotalCash, //เงินสดฝาก
                                                                                   amounttransfer = eodsummary.TotalTransfer,
                                                                                   substitutefee = eodsummary.SubstituteWage.HasValue ? eodsummary.SubstituteWage.Value : 0, //tran.SubstituteFee,
                                                                                   depositfee = eodsummary.Fee.HasValue ? eodsummary.Fee.Value : 0,
                                                                                   otherfee = eodsummary.OtherExpense.HasValue ? eodsummary.OtherExpense.Value : 0, //tran.OtherFee
                                                                                   totalamount = eodsummary.FinalTotal,
                                                                                   vat = 0, //eodsummary.VAT
                                                                                   discount = 0, //eodsummary.Discount
                                                                                   remark = eodsummary.OtherExpenseNote,
                                                                                   //eodsummarystatus = eodsummary.IsActive,
                                                                                   //remark = jAudit != null ? jAudit.Description : null,
                                                                                   //auditstatus = jAudit != null ? "" : "รอตรวจสอบ",
                                                                                   auditid = jAudit != null ? jAudit.AuditID : 0,
                                                                                   auditorname = jAudit != null ? jAudit.CreatedBy : "",
                                                                                   //referenceno = tran.ReferenceNo,
                                                                               }).AsEnumerable();

        #region Filter
        if (request.branchid.HasValue)
        {
            resEodSummaryReport = resEodSummaryReport.Where(w => w.branchid == request.branchid.Value).ToList();
        }

        if (!string.IsNullOrEmpty(request.searchvalue))
        {
            resEodSummaryReport = resEodSummaryReport.Where(w => (w.branchname != null && w.branchname.Contains(request.searchvalue)) 
            || (!string.IsNullOrEmpty(w.remark) && w.remark.Contains(request.searchvalue))).ToList();
        }
        #endregion

        #region Group by all branch by date
        var resGroupByDate = (from a in resEodSummaryReport
                              group a by new { a.branchid, a.transactiondate.Date } into grps
                              select new
                              {
                                  branchid = grps.Key.branchid,
                                  transactiondate = grps.Key.Date,
                                  data = grps.Where(w => w.transactiondate.Date == grps.Key.Date)
                              }).ToList();

        resEodSummaryReport = resGroupByDate.Select(s => new SaleBarcodeReportResponseDetailDTO
        {
            //transactionid = s.data.FirstOrDefault().transactionid,
            transactiondate = s.transactiondate,
            branchid = s.branchid,
            branchname = s.data.FirstOrDefault(w => w.branchid == s.branchid && w.transactiondate.Date == s.transactiondate.Date).branchname,
            username = s.data.FirstOrDefault(w => w.branchid == s.branchid && w.transactiondate.Date == s.transactiondate.Date).username,
            amountcash = s.data.Sum(w => w.amountcash),
            amounttransfer = s.data.Sum(w => w.amounttransfer),
            substitutefee = s.data.Sum(w => w.substitutefee),
            depositfee = s.data.Sum(w => w.depositfee),
            otherfee = s.data.Sum(w => w.otherfee),
            totalamount = s.data.Sum(w => w.totalamount),
            vat = s.data.Sum(w => w.vat ?? 0),
            discount = s.data.Sum(w => w.discount ?? 0),
            remark = s.data.FirstOrDefault(w => w.branchid == s.branchid && w.transactiondate.Date == s.transactiondate.Date).remark,
            auditid = s.data.FirstOrDefault().auditid,
            auditorname = s.data.FirstOrDefault().auditorname
        }).AsEnumerable();
        #endregion

        if (!resEodSummaryReport.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานสรุปยอดสินวันสาขา");
        }

        //Final data
        List<SaleBarcodeReportResponseDetailDTO> resData = resEodSummaryReport.ToList();

        #region Update updatedby data from emp name
        List<string> userNameList = resData.Select(s => s.username).Distinct().ToList();
        List<string> auditorNameList = resData.Select(s => s.auditorname).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName) || auditorNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        resData = resData.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.username))
            {
                s.username = empDataList.FirstOrDefault(w => w.UserName == s.username) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.username).FirstName : s.username;
            }

            if (!string.IsNullOrEmpty(s.auditorname))
            {
                s.auditorname = empDataList.FirstOrDefault(w => w.UserName == s.auditorname) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.auditorname).FirstName : s.auditorname;
            }
            return s;
        }).ToList();
        #endregion

        //Addign total row
        totalRow = resData.Count();

        //Assign data
        if (!request.isexportalldata)
        {
            resData = resData.Skip(request.startrow).Take(request.pagesize).ToList();
        }

        return new BaseResponse<SaleBarcodeReportResponseDTO>
        {
           result = true,
            status = "200",
            message = "Success",
            soruce = "SaleBarcodeReportHandler",
            data = new SaleBarcodeReportResponseDTO
            {
                totalrow = totalRow,
                data = resData
            }
        };
    }
}
