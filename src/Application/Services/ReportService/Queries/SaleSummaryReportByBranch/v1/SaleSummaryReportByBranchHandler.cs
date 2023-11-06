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

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByBranch.v1;
public class SaleSummaryReportByBranchHandler : BaseService, IRequestHandler<SaleSummaryReportByBranchQuery, BaseResponse<SaleSummaryReportResponseDTO>>
{
    public SaleSummaryReportByBranchHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<SaleSummaryReportResponseDTO>> Handle(SaleSummaryReportByBranchQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<SaleSummaryReportResponseDTO> resSaleSummaryReport = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync()
                                                                          join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
                                                                          join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
                                                                          join audit in await _unitOfWork.Repository<TTTransactionAudit>().QueryAsync(w => w.IsActive)
                                                                          on new { tran.BranchID, tran.TransactionDate.Date } equals new { audit.BranchID, audit.TransactionDate.Date } into tAudit
                                                                          from jAudit in tAudit.DefaultIfEmpty()
                                                                          where tran.IsActive
                                                                          && tran.BranchID == request.branchid
                                                                          && tran.TransactionDate.Date == request.transactiondate.Date
                                                                          select new SaleSummaryReportResponseDTO
                                                                          {
                                                                              transactionid = tran.TransactionID,
                                                                              transactiondate = tran.TransactionDate,

                                                                              totalamount = tran.TotalAmount,
                                                                              amounttransfer = tran.AmountTransfer,
                                                                              amountdeposit = tran.AmountDeposit,
                                                                              amountcash = tran.AmountCash,
                                                                              depositfee = tran.Fee,
                                                                              createdby = tran.CreatedBy,
                                                                              //createdbystaff = jEmp != null ? jEmp.FirstName : "N/A",

                                                                              branchid = tran.BranchID,
                                                                              branchname = branch.BranchName,

                                                                              auditid = jAudit.AuditID,
                                                                              totalauditamount = jAudit.TotalAuditAmount,
                                                                              auditdescription = jAudit.Description
                                                                          }).AsEnumerable();

        resSaleSummaryReport = resSaleSummaryReport.GroupBy(g => g.branchid).Select(s => new SaleSummaryReportResponseDTO
        {
            //branchid = s.Key.branchid,
            //branchname = s.First(w => w.branchid == s.Key.branchid).branchname,
            //transactiondate = s.First(w => w.branchid == s.Key.branchid).transactiondate,
            //         totalamount = s.First(w => w.branchid == s.Key.branchid).totalamount,
            //         amounttransfer = s.First(w => w.branchid == s.Key.branchid).amounttransfer,
            //         amountdeposit = s.First(w => w.branchid == s.Key.branchid).amountdeposit,
            //         amountcash = s.First(w => w.branchid == s.Key.branchid).amountcash,
            //         depositfee = s.First(w => w.branchid == s.Key.branchid).depositfee,
            //         createdby = s.First(w => w.branchid == s.Key.branchid).createdby,
            //         auditid = s.First(w => w.branchid == s.Key.branchid).auditid,
            //         totalauditamount = s.First(w => w.branchid == s.Key.branchid).totalauditamount,
            //         auditdescription = s.First(w => w.branchid == s.Key.branchid).auditdescription
            branchid = s.Key,
            branchname = s.First(w => w.branchid == s.Key).branchname,
            transactiondate = s.First(w => w.branchid == s.Key).transactiondate,
            totalamount = s.Sum(x => x.totalamount),
            amounttransfer = s.Sum(x => x.amounttransfer),
            amountdeposit = s.Sum(x => x.amountdeposit),
            amountcash = s.Sum(x => x.amountcash),
            depositfee = s.Sum(x => x.depositfee),
            createdby = s.First(w => w.branchid == s.Key).createdby,
            auditid = s.First(w => w.branchid == s.Key).auditid,
            totalauditamount = s.First(w => w.branchid == s.Key).totalauditamount,
            auditdescription = s.First(w => w.branchid == s.Key).auditdescription
        }).ToList();

        //IEnumerable<SaleSummaryReportResponseDTO> resSaleSummaryReport = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync()
        //                                                                  join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
        //                                                                  join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
        //                                                                  join audit in await _unitOfWork.Repository<TTTransactionAudit>().QueryAsync()
        //                                                                  on new { tran.BranchID, tran.TransactionDate } equals new { audit.BranchID, audit.TransactionDate } into tAudit
        //                                                                  from jAudit in tAudit.DefaultIfEmpty()
        //                                                                  where tran.IsActive
        //                                                                  && tran.BranchID == request.branchid
        //                                                                  && tran.TransactionDate.Date == request.transactiondate.Date
        //                                                                  select new SaleSummaryReportResponseDTO
        //                                                                  {
        //                                                                      transactionid = tran.TransactionID,
        //                                                                      transactiondate = tran.CreatedDate,

        //                                                                      totalamount = tran.TotalAmount,
        //                                                                      amounttransfer = tran.AmountTransfer,
        //                                                                      amountdeposit = tran.AmountDeposit,
        //                                                                      amountcash = tran.AmountCash,
        //                                                                      depositfee = tran.Fee,
        //                                                                      createdby = tran.CreatedBy,
        //                                                                      //createdbystaff = jEmp != null ? jEmp.FirstName : "N/A",

        //                                                                      branchid = tran.BranchID,
        //                                                                      branchname = branch.BranchName,

        //                                                                      auditid = jAudit.AuditID,
        //                                                                      totalauditamount = jAudit.TotalAuditAmount,
        //                                                                      auditdescription = jAudit.Description
        //                                                                  }).AsEnumerable();

        //resSaleSummaryReport = resSaleSummaryReport.GroupBy(g => new { g.branchid, g.transactionid }).Select(s => new SaleSummaryReportResponseDTO
        //{
        //    branchid = s.Key.branchid,
        //    branchname = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).branchname,
        //    transactiondate = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).transactiondate,
        //    totalamount = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).totalamount,
        //    amounttransfer = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).amounttransfer,
        //    amountdeposit = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).amountdeposit,
        //    amountcash = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).amountcash,
        //    depositfee = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).depositfee,
        //    createdby = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).createdby,
        //    auditid = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).auditid,
        //    totalauditamount = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).totalauditamount,
        //    auditdescription = s.First(w => w.branchid == s.Key.branchid && w.transactionid == s.Key.transactionid).auditdescription
        //}).ToList();

        if (!resSaleSummaryReport.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
        }

        #region Update updatedby data from emp name
        List<string> userNameList = resSaleSummaryReport.ToList().Select(s => s.createdby).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        resSaleSummaryReport = resSaleSummaryReport.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.createdby))
            {
                s.createdbystaff = empDataList.FirstOrDefault(w => w.UserName == s.createdby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.createdby).FirstName : s.createdby;
            }

            return s;
        }).ToList();
        #endregion

        return new BaseResponse<SaleSummaryReportResponseDTO>
        {
            result = true,
            data = resSaleSummaryReport.FirstOrDefault(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
