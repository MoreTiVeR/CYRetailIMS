using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
public class SaleSummaryReportHandler : BaseService, IRequestHandler<SaleSummaryReportQuery, BaseResponse<List<SaleSummaryReportResponseDTO>>>
{
	public SaleSummaryReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<List<SaleSummaryReportResponseDTO>>> Handle(SaleSummaryReportQuery request, CancellationToken cancellationToken)
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
																		  && tran.TransactionDate.Date == request.transactiondate.Date
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

		//var resBranch1 = resSaleSummaryReport.Where(w => w.branchid == 1).ToList();
		//var resBranch2 = resSaleSummaryReport.Where(w => w.branchid == 2).ToList();

		if (request.branchid.HasValue)
		{
			resSaleSummaryReport = resSaleSummaryReport.Where(w => w.branchid == request.branchid.Value);
		}

		//if (request.transactiondate.HasValue)
		//{
		//	resSaleSummaryReport = resSaleSummaryReport.Where(w => w.transactiondate == request.transactiondate.Value);
		//}

		#region Group by Branch
		//var resGroupByBranch = (from a in resSaleSummaryReport
		//						group a by new { a.branchid } into grps
		//						select new { grps.Key.branchid, data = grps.Where(w => w.branchid == grps.Key.branchid) }).ToList();

		//resSaleSummaryReport = resGroupByBranch.Select(s => new SaleSummaryReportResponseDTO
		//{
		//	transactionid = s.data.FirstOrDefault().transactionid,
		//	transactiondate = s.data.FirstOrDefault().transactiondate,
		//	totalamount = s.data.Sum(w => w.totalamount),
		//	amounttransfer = s.data.Sum(w => w.amounttransfer),
		//	amountdeposit = s.data.Sum(w => w.amountdeposit),
		//	amountcash = s.data.Sum(w => w.amountcash),
		//	depositfee = s.data.Sum(w => w.depositfee),
		//	createdby = s.data.FirstOrDefault().createdby,
		//	createdbystaff = s.data.FirstOrDefault().createdbystaff,

		//	branchid = s.data.FirstOrDefault().branchid,
		//	branchname = s.data.FirstOrDefault().branchname,

		//	auditid = s.data.FirstOrDefault().auditid,
		//	totalauditamount = s.data.Sum(w => w.totalauditamount),
		//	auditdescription = s.data.FirstOrDefault().auditdescription
		//}).AsEnumerable();
		#endregion

		if (!resSaleSummaryReport.Any())
		{
			throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
		}
		return new BaseResponse<List<SaleSummaryReportResponseDTO>>
		{
			result = true,
			data = resSaleSummaryReport.OrderBy(w => w.branchid).ToList(),
			message = "Success",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
