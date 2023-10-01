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

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByTransID.v1;
public class SaleSummaryReportByTransIDHandler : BaseService, IRequestHandler<SaleSummaryReportByTransIDQuery, BaseResponse<SaleSummaryReportResponseDTO>>
{
    public SaleSummaryReportByTransIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<SaleSummaryReportResponseDTO>> Handle(SaleSummaryReportByTransIDQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<SaleSummaryReportResponseDTO> resSaleSummaryReport = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync()
                                                                          join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
                                                                          join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
                                                                          join emp in await _unitOfWork.Repository<TMEmployee>().FindWithInclude(w => w.IsActive, i => i.Include(ic => ic.User)) on tran.CreatedBy equals emp.User.UserName
                                                                          into tEmp
                                                                          from jEmp in tEmp.DefaultIfEmpty()
                                                                          join audit in await _unitOfWork.Repository<TTTransactionAudit>().QueryAsync() on tran.TransactionID equals audit.TransactionID into tAudit
                                                                          from jAudit in tAudit.DefaultIfEmpty()
                                                                          where tran.IsActive
                                                                          && tran.TransactionID == request.transactionid
                                                                          select new SaleSummaryReportResponseDTO
                                                                          {
                                                                              transactionid = tran.TransactionID,
                                                                              transactiondate = tran.CreadedDate,

                                                                              totalamount = tran.TotalAmount,
                                                                              amounttransfer = tran.AmountTransfer,
                                                                              amountdeposit = tran.AmountDeposit,
                                                                              amountcash = tran.AmountCash,
                                                                              depositfee = tran.Fee,
                                                                              createdby = tran.CreatedBy,
                                                                              createdbystaff = jEmp != null ? jEmp.FirstName : "N/A",

                                                                              branchid = tran.BranchID,
                                                                              branchname = branch.BranchName,

                                                                              auditid = jAudit.AuditID,
                                                                              totalauditamount = jAudit.TotalAuditAmount,
                                                                              auditdescription = jAudit.Description
                                                                          }).AsEnumerable();


        if (!resSaleSummaryReport.Any())
        {
            throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
        }
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
