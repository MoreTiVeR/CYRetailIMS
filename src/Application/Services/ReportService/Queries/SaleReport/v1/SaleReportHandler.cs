using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
public class SaleReportHandler : BaseService, IRequestHandler<SaleReportQuery, BaseResponse<List<SaleReportResponseDTO>>>
{
	public SaleReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<List<SaleReportResponseDTO>>> Handle(SaleReportQuery request, CancellationToken cancellationToken)
	{
		var resData = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync()
					   join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
					   join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on detail.ItemID equals item.ItemID
					   join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
					   join brand in await _unitOfWork.Repository<TMItemBrand>().QueryAsync() on item.BrandID equals brand.BrandID
					   join emp in await _unitOfWork.Repository<TMEmployee>().FindWithInclude(w => w.IsActive, i => i.Include(ic => ic.User)) on tran.CreatedBy equals emp.User.UserName
					   into tEmp
					   from jEmp in tEmp.DefaultIfEmpty()
					   where tran.IsActive
					   select new SaleReportResponseDTO
					   {
						   transactionid = tran.TransactionID,
						   itemcode = item.ItemCode,
						   itemname = item.Name,
						   brandid = item.BrandID,
						   brandname = brand.BrandName,
						   qty = detail.Qty,
						   unitprice = item.Price,
						   amounttransfer = tran.AmountTransfer,
						   amountdeposit = tran.AmountDeposit,
						   depositfee = tran.Fee,
						   amountcash = tran.AmountCash,
						   totalamount = tran.TotalAmount,
						   branchid = tran.BranchID,
						   branchname = branch.BranchName,
						   createdby = tran.CreatedBy,
						   createddate = tran.CreadedDate,
						   createdbystaff = jEmp != null ? jEmp.FirstName : "N/A"
					   }).AsEnumerable();
		if (request.branchid.HasValue)
		{
			resData = resData.Where(w => w.branchid == request.branchid.Value);
		}

		if (request.transaction_startdate.HasValue && request.transaction_enddate.HasValue)
		{
			resData = resData.Where(w => w.createddate.Date >= request.transaction_startdate.Value.Date && w.createddate.Date <= request.transaction_enddate.Value.Date);
		}

		if (request.transaction_startdate.HasValue && !request.transaction_enddate.HasValue)
		{
			resData = resData.Where(w => w.createddate.Date >= request.transaction_startdate.Value.Date);
		}

		if (!resData.Any())
		{
			throw new Exception("ไม่พบข้อมูลรายงานขายสินค้า");
		}
		return new BaseResponse<List<SaleReportResponseDTO>>
		{
			result = true,
			data = resData.ToList(),
			message = "Success",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};

	}
}
