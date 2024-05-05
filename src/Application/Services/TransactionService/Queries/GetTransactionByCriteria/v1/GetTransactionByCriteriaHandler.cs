
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByCriteria.v1;
public class GetTransactionByCriteriaHandler : BaseService, IRequestHandler<GetTransactionByCriteriaQuery, BaseResponse<GetTransactionByCriteriaResponseDTO>>
{
    public GetTransactionByCriteriaHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetTransactionByCriteriaResponseDTO>> Handle(GetTransactionByCriteriaQuery request, CancellationToken cancellationToken)
    {
        GetTransactionByCriteriaResponseDTO resTransaction = await (from tran in await _unitOfWork.Repository<TTTransaction>().FindWithInclude(w => w.BranchID == request.branchid
                                                                    //&& (w.TransactionDate.Date >= DateTime.Now.Date && w.TransactionDate.Date <= DateTime.Now.Date) 
                                                                    && w.TransactionID == request.transactionid && w.BranchID == request.branchid
                                                                    && w.IsActive, i => i.Include(ii => ii.TransactionType), idetail => idetail.Include(d => d.TTTransactonDetails))
                                                                    join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync() on tran.BranchID equals branch.BranchID
                                                                    join trantype in await _unitOfWork.Repository<TMTransactionType>().QueryAsync() on tran.TransactionTypeID equals trantype.TransactionTypeID
                                                                    join emp in await _unitOfWork.Repository<TMEmployee>().FindWithInclude(w => w.IsActive, i => i.Include(ic => ic.User))
                                                                    on tran.CreatedBy equals emp.User.UserName into tUser
                                                                    from jUser in tUser.DefaultIfEmpty()
                                                                    select new GetTransactionByCriteriaResponseDTO
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
                                                                        createddate = tran.CreatedDate,
                                                                        createdby = tran.CreatedBy,
                                                                        createdbystaff = jUser != null ? jUser.FirstName : "N/A",
                                                                        isactive = tran.IsActive,
                                                                        updateddate = tran.UpdatedDate,
                                                                        updatedby = tran.UpdatedBy,
                                                                        remark = tran.Remark,
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
                                                                    }).FirstOrDefaultAsync();

        if (resTransaction == null)
        {
            throw new Exception("Transaction not found");
        }

        #region Update updatedby data from emp name
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => w.UserName == resTransaction.createdby || w.UserName == resTransaction.updatedby, i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        if (!string.IsNullOrEmpty(resTransaction.createdby))
        {
            resTransaction.createdbystaff = empDataList.FirstOrDefault(w => w.UserName == resTransaction.createdby) != null
            ? empDataList.FirstOrDefault(w => w.UserName == resTransaction.createdby).FirstName : resTransaction.createdby;
        }
        if (!string.IsNullOrEmpty(resTransaction.updatedby))
        {
            resTransaction.updatedby = empDataList.FirstOrDefault(w => w.UserName == resTransaction.updatedby) != null
            ? empDataList.FirstOrDefault(w => w.UserName == resTransaction.updatedby).FirstName : resTransaction.updatedby;
        }
        #endregion

        return new BaseResponse<GetTransactionByCriteriaResponseDTO>
        {
            result = true,
            data = resTransaction,
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
