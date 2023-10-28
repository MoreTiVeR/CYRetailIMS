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
																	  join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
																	  join audit in await _unitOfWork.Repository<TTTransactionAudit>().QueryAsync() on tran.BranchID equals audit.BranchID into tAudit
																	  from jAudit in tAudit.DefaultIfEmpty()
																	  where tran.IsActive
																	  && (tran.TransactionDate.Date >= request.transaction_startdate.Date && tran.TransactionDate.Date <= request.transaction_enddate.Date)
																	  select new AuditReportResponseDTO
																	  {
																		  //transactionid = tran.TransactionID,
																		  transactiondate = tran.CreadedDate,

																		  totalamount = tran.TotalAmount,
																		  amounttransfer = tran.AmountTransfer,
																		  amountdeposit = tran.AmountDeposit,
																		  amountcash = tran.AmountCash,
																		  depositfee = tran.Fee,
																		  //createdby = tran.CreatedBy,
																		  //createdbystaff = jEmp != null ? jEmp.FirstName : "N/A",

																		  //auditid = jAudit.AuditID,
																		  totalauditamount = jAudit.TotalAuditAmount,
																		  auditdescription = jAudit.Description
																	  }).AsEnumerable();

		#region Group by all branch by date
		var resGroupByDate = (from a in resSaleSummaryReport
							  group a by new { a.transactiondate.Date } into grps
							  select new { grps.Key.Date, data = grps.Where(w => w.transactiondate.Date == grps.Key.Date) }).ToList();

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

			//auditid = s.data.FirstOrDefault().auditid,
			totalauditamount = s.data.Sum(w => w.totalauditamount),
			auditdescription = s.data.FirstOrDefault().auditdescription
		}).AsEnumerable();
		//var _resSaleSummaryReport = resSaleSummaryReport.ToList();
		#endregion

		if (!resSaleSummaryReport.Any())
		{
			throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
		}
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
