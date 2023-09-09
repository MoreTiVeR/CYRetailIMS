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

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
public class GetTransactionByBranchIDHandler : BaseService, IRequestHandler<GetTransactionByBranchIDQuery, BaseResponse<List<GetTransactionByBranchIDResponseDTO>>>
{
	public GetTransactionByBranchIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<List<GetTransactionByBranchIDResponseDTO>>> Handle(GetTransactionByBranchIDQuery request, CancellationToken cancellationToken)
	{
		List<GetTransactionByBranchIDResponseDTO> resTransaction = (from tran in await _unitOfWork.Repository<TTTransaction>().FindWithInclude(w => w.BranchID == request.branchid 
																	&& (w.TransactionDate.Date >= DateTime.Now.Date && w.TransactionDate.Date <= DateTime.Now.Date) 
																	&& w.IsActive, 
																	i => i.Include(ii => ii.TransactionType), idetail => idetail.Include(d => d.TTTransactonDetails))
																	join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
																	join trantype in await _unitOfWork.Repository<TMTransactionType>().QueryAsync() on tran.TransactionTypeID equals trantype.TransactionTypeID
																	//join detail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync() on tran.TransactionID equals detail.TransactionID
																	//where tran.BranchID == request.branchid
																	select new GetTransactionByBranchIDResponseDTO
																	{
																		transactionid = tran.TransactionID,
																		transactiondate = tran.TransactionDate,
																		transactiontypid = tran.TransactionID,
																		transactiontypename = tran.TransactionType.TransactionTypeName,
																		transactiontypedesc = tran.TransactionType.Description,
																		branchid = tran.BranchID,
																		branchname = branch.BranchName,
																		amountcash = tran.AmountCash,
																		amountdeposit = tran.AmountDeposit,
																		amounttransfer = tran.AmountTransfer,
																		totalamount = tran.TotalAmount,
																		creadeddate = tran.CreadedDate,
																		createdby = tran.CreatedBy,
																		isactive = tran.IsActive,
																		updateddate = tran.UpdatedDate,
																		updatedby = tran.UpdatedBy,
																		detail = (from a in tran.TTTransactonDetails
																				  join b in _unitOfWork.Repository<TMItem>().Query() on a.ItemID equals b.ItemID
																				  //where b.ItemID == a.ItemID
																				  select new GetTransactionDetailResponseDTO
																				  {
																					  transactiondetailid = a.TransactionDetailID,
																					  itemid = a.ItemID,
																					  itemname = b.Name,
																					  price = a.Price,
																					  qty = a.Qty,
																					  amount = a.Amount,
																					  isactive = a.IsActive.HasValue ? a.IsActive.Value : false
																				  }).ToList()
																	}).ToList();

		if (!resTransaction.Any())
		{
			throw new Exception("ไม่พบข้อมูลประวัติการขายสินค้า!");
		}
		return new BaseResponse<List<GetTransactionByBranchIDResponseDTO>>
		{
			result = true,
			data = resTransaction,
			message = " Success",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
