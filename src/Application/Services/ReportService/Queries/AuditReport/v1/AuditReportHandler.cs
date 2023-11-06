using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
public class AuditReportHandler : BaseService, IRequestHandler<AuditReportQuery, BaseResponse<List<AuditReportResponseDTO>>>
{
    public AuditReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    /// <summary>
    /// Sum Data All Branch
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<BaseResponse<List<AuditReportResponseDTO>>> Handle(AuditReportQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<AuditReportResponseDTO> resSaleSummaryReport = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync()
                                                                    join audit in await _unitOfWork.Repository<TTTransactionAudit>().QueryAsync() on new { tran.BranchID, tran.TransactionDate.Date } equals new { audit.BranchID, audit.TransactionDate.Date } into tAudit
                                                                    from jAudit in tAudit.DefaultIfEmpty()
                                                                    where tran.IsActive
                                                                    && (tran.TransactionDate.Date >= request.transaction_startdate.Date && tran.TransactionDate.Date <= request.transaction_enddate.Date)
                                                                    select new AuditReportResponseDTO
                                                                    {
                                                                        //transactionid = tran.TransactionID,
                                                                        transactiondate = tran.TransactionDate,

                                                                        totalamount = tran.TotalAmount,
                                                                        amounttransfer = tran.AmountTransfer,
                                                                        amountdeposit = tran.AmountDeposit,
                                                                        amountcash = tran.AmountCash,
                                                                        depositfee = tran.Fee,
                                                                        //createdby = tran.CreatedBy,
                                                                        //createdbystaff = jEmp != null ? jEmp.FirstName : "N/A",

                                                                        auditid = jAudit != null ? jAudit.AuditID : 0,
                                                                        totalauditamount = jAudit != null ? jAudit.TotalAuditAmount : 0,
                                                                        auditdescription = jAudit != null ? jAudit.Description : string.Empty,
                                                                        auditor = jAudit != null ? jAudit.CreatedBy : string.Empty
                                                                    }).AsEnumerable();

        #region Group by all branch by date
        var resGroupByDate = (from a in resSaleSummaryReport
                              group a by new { a.transactiondate.Date } into grps
                              select new { transactiondate = grps.Key.Date, data = grps.Where(w => w.transactiondate.Date == grps.Key.Date) }).ToList();

        resSaleSummaryReport = resGroupByDate.Select(s => new AuditReportResponseDTO
        {
            //transactionid = s.data.FirstOrDefault().transactionid,
            transactiondate = s.data.FirstOrDefault().transactiondate,
            totalamount = s.data.Sum(w => w.totalamount),
            amounttransfer = s.data.Sum(w => w.amounttransfer),
            amountdeposit = s.data.Sum(w => w.amountdeposit),
            amountcash = s.data.Sum(w => w.amountcash),
            depositfee = s.data.Sum(w => w.depositfee),
            //createdby = s.data.FirstOrDefault().createdby,
            //createdbystaff = s.data.FirstOrDefault().createdbystaff,

            auditid = s.data.FirstOrDefault().auditid,
            totalauditamount = s.data.FirstOrDefault().totalauditamount,
            auditdescription = s.data.FirstOrDefault().auditdescription,
            auditor = s.data.FirstOrDefault().auditor
        }).AsEnumerable();
        #endregion

        if (!resSaleSummaryReport.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
        }

        #region Update updatedby data from emp name
        List<string> userNameList = resSaleSummaryReport.ToList().Select(s => s.auditor).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        resSaleSummaryReport = resSaleSummaryReport.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.auditor))
            {
                s.auditorname = empDataList.FirstOrDefault(w => w.UserName == s.auditor) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.auditor).FirstName : s.auditor;
            }

            return s;
        }).ToList();
        #endregion

        return new BaseResponse<List<AuditReportResponseDTO>>
        {
            result = true,
            data = resSaleSummaryReport.OrderBy(w => w.transactiondate).ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };

    }
}
