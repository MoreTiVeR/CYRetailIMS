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
        IEnumerable<SaleBarcodeReportResponseDetailDTO> resBarcodeSummaryReport = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync()
                                                                                   //join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
                                                                                   join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
                                                                                   join audit in await _unitOfWork.Repository<TTTransactionAudit>().QueryAsync(w => w.IsActive)
                                                                                   on new { tran.BranchID, tran.TransactionDate.Date } equals new { audit.BranchID, audit.TransactionDate.Date } into tAudit
                                                                                   from jAudit in tAudit.DefaultIfEmpty()
                                                                                   where tran.IsActive
                                                                                   && (tran.TransactionDate.Date >= request.transaction_startdate.Date && tran.TransactionDate.Date <= request.transaction_enddate.Date)
                                                                                   select new SaleBarcodeReportResponseDetailDTO
                                                                                   {
                                                                                       transactionid = tran.TransactionID,
                                                                                       transactiondate = tran.TransactionDate,
                                                                                       branchid = tran.BranchID,
                                                                                       branchname = branch.BranchName,
                                                                                       username = tran.CreatedBy, //ชื่อพนักงาน
                                                                                       amountcash = tran.AmountCash, //เงินสดฝาก
                                                                                       amounttransfer = tran.AmountTransfer,
                                                                                       substitutefee = 0, //tran.SubstituteFee,
                                                                                       depositfee = tran.Fee,
                                                                                       otherfee = 0, //tran.OtherFee
                                                                                       totalamount = tran.TotalAmount,
                                                                                       vat = tran.Vat,
                                                                                       discount = tran.Discount,
                                                                                       remark = jAudit != null ? jAudit.Description : null,
                                                                                       //auditstatus = jAudit != null ? "" : "รอตรวจสอบ",
                                                                                       auditid = jAudit != null ? jAudit.AuditID : 0,
                                                                                       auditorname = jAudit != null ? jAudit.CreatedBy : "",
                                                                                       //referenceno = tran.ReferenceNo,
                                                                                   }).AsEnumerable();

        #region Filter
        if (request.branchid.HasValue)
        {
            resBarcodeSummaryReport = resBarcodeSummaryReport.Where(w => w.branchid == request.branchid.Value).ToList();
        }

        if (!string.IsNullOrEmpty(request.searchvalue))
        {
            resBarcodeSummaryReport = resBarcodeSummaryReport.Where(w => (w.branchname != null && w.branchname.Contains(request.searchvalue)) 
            || (!string.IsNullOrEmpty(w.remark) && w.remark.Contains(request.searchvalue))).ToList();
        }
        #endregion

        #region Group by all branch by date
        var resGroupByDate = (from a in resBarcodeSummaryReport
                              group a by new { a.branchid, a.transactiondate.Date } into grps
                              select new
                              {
                                  branchid = grps.Key.branchid,
                                  transactiondate = grps.Key.Date,
                                  data = grps.Where(w => w.transactiondate.Date == grps.Key.Date)
                              }).ToList();

        resBarcodeSummaryReport = resGroupByDate.Select(s => new SaleBarcodeReportResponseDetailDTO
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

        if (!resBarcodeSummaryReport.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานสรุปยอดสินวันสาขา");
        }

        //Final data
        List<SaleBarcodeReportResponseDetailDTO> resData = resBarcodeSummaryReport.ToList();

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
