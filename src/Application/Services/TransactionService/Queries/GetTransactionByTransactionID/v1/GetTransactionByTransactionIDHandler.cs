using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByTransactionID.v1;
public class GetTransactionByTransactionIDHandler : BaseService, IRequestHandler<GetTransactionByTransactionIDQuery, BaseResponse<GetTransactionByBranchIDResponseDTO>>
{
    public GetTransactionByTransactionIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetTransactionByBranchIDResponseDTO>> Handle(GetTransactionByTransactionIDQuery request, CancellationToken cancellationToken)
    {
        List<GetTransactionByBranchIDResponseDTO> resTransaction = (from tran in await _unitOfWork.Repository<TTTransaction>().FindWithInclude(w => w.TransactionID == request.transactionid
                                                                    //&& (w.TransactionDate.Date >= DateTime.Now.Date && w.TransactionDate.Date <= DateTime.Now.Date) 
                                                                    //&& w.TransactionDate.Month >= DateTime.Now.Month
                                                                    && w.IsActive, i => i.Include(ii => ii.TransactionType), idetail => idetail.Include(d => d.TTTransactonDetails))
                                                                    join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
                                                                    join trantype in await _unitOfWork.Repository<TMTransactionType>().QueryAsync() on tran.TransactionTypeID equals trantype.TransactionTypeID
                                                                    join emp in await _unitOfWork.Repository<TMEmployee>().FindWithInclude(w => w.IsActive, i => i.Include(ic => ic.User)) on tran.CreatedBy equals emp.User.UserName
                                                                    into tUser
                                                                    from jUser in tUser.DefaultIfEmpty()
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
                                                                        depositfee = tran.Fee,
                                                                        creadeddate = tran.CreadedDate,
                                                                        createdby = tran.CreatedBy,
                                                                        createdbystaff = jUser != null ? jUser.FirstName : "N/A",
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

        #region Update updatedby data from emp name
        List<string> userNameList = resTransaction.Select(s => s.createdby).Union(resTransaction.Select(s => s.updatedby)).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        resTransaction = resTransaction.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.createdby))
            {
                s.createdbystaff = empDataList.FirstOrDefault(w => w.UserName == s.createdby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.createdby).FirstName : s.createdby;
            }

            if (!string.IsNullOrEmpty(s.updatedby))
            {
                s.updatedby = empDataList.FirstOrDefault(w => w.UserName == s.updatedby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.updatedby).FirstName : s.updatedby;
            }
            return s;
        }).ToList();
        #endregion

        return new BaseResponse<GetTransactionByBranchIDResponseDTO>
        {
            result = true,
            data = resTransaction.FirstOrDefault(),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
