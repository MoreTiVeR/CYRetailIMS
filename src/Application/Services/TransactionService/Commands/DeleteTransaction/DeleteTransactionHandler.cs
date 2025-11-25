using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.EventHandlers;
using CYRetailIMS.Application.Services.TransactionService.EventHandlers;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using CYRetailIMS.Domain.Events.TTTransactionDetails;
using CYRetailIMS.Domain.Events.TTTransactions;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.DeleteTransaction;
public class DeleteTransactionHandler : BaseService, IRequestHandler<DeleteTransactionCommand, BaseResponse<CommandResponse>>
{
    public DeleteTransactionHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        IQueryable<TTTransaction> resTrans = await _unitOfWork.Repository<TTTransaction>().FindWithInclude(w => w.TransactionID == request.transactionid 
        && w.IsActive, i => i.Include(w => w.TTTransactonDetails));
        if (resTrans == null || !resTrans.Any())
        {
            //throw new Exception("Transaction not found or has been deleted.");
            return new BaseResponse<CommandResponse>
            {
                result = false,
                message = "Transaction not found or has been deleted.",
                soruce = "DB",
                status = StatusCodes.Status404NotFound.ToString(),
                data = new CommandResponse
                {
                    result = false,
                    error = new ErrorData
                    {
                        type = StatusCodes.Status404NotFound.ToString(),
                        status = StatusCodes.Status404NotFound.ToString(),
                        message = "ไม่พบรายการขาย หรือรายการได้ถูกยกเลิกแล้ว"
                    }
                }
                //error = new ErrorResponse
                //{
                //    error = new ErrorData
                //    {
                //        type = StatusCodes.Status404NotFound.ToString(),
                //        status = StatusCodes.Status404NotFound.ToString(),
                //        message = "ไม่พบรายการขาย หรือรายการได้ถูกยกเลิกแล้ว"
                //    }
                //}
            };
        }

        // Get branch id from transaction
        int branchId = resTrans.FirstOrDefault().BranchID;
        DateTime txnDate = resTrans.FirstOrDefault().TransactionDate;
        var resAuditData = await _unitOfWork.Repository<TTTransactionAudit>().QueryAsync(w => w.BranchID == branchId 
        && w.IsActive && w.TransactionDate.Date == txnDate.Date);
        if (resAuditData != null || resAuditData.Any())
        {
            //throw new Exception("Transaction has been audit.");
            return new BaseResponse<CommandResponse>
            {
                result = false,
                message = "Transaction has been audit.",
                soruce = "DB",
                status = StatusCodes.Status404NotFound.ToString(),
                data = new CommandResponse
                {
                    result = false,
                    error = new ErrorData
                    {
                        type = StatusCodes.Status404NotFound.ToString(),
                        status = StatusCodes.Status404NotFound.ToString(),
                        message = "ไม่สามารถยกเลิกรายการได้, เนื่องจากบัญชีได้ทำการตรวจสอบรายการแล้ว"
                    }
                }
                //error = new ErrorResponse
                //{
                //    error = new ErrorData
                //    {
                //        type = StatusCodes.Status404NotFound.ToString(),
                //        status = StatusCodes.Status404NotFound.ToString(),
                //        message = "ไม่สามารถยกเลิกรายการได้, เนื่องจากบัญชีได้ทำการตรวจสอบรายการแล้ว"
                //    },
                //}
            };
        }

        DateTime delDate = DateTime.Now;
        //In-Active trasnaction, detail
        resTrans.ToList().ForEach(w =>
        {
            w.Remark = $"[ถูกยกเลิกโดยพนักงาน {request.deletedby}] | " + w.Remark;
            w.IsActive = false;
            w.SetUpdatedBy(request.deletedby);
            w.SetUpdatedDate(delDate);
            w.TTTransactonDetails.ToList().ForEach(d =>
            {
                d.IsActive = false;
                d.AddDomainEvent(new TTTransactionDetailDeleteEvent(d));
            });
            w.AddDomainEvent(new TTTransactionsDeleteEvent(w));
        });

        //Returned Item in branch stock
        var itemCollections = resTrans.SelectMany(s => s.TTTransactonDetails).Select(s => new
        {
            ItemID = s.ItemID,
            Qty = s.Qty
        }).ToList();
        var resItemInBranch = (from a in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.BranchID == branchId)
                               where itemCollections.Select(s => s.ItemID).Contains(a.ItemID)
                               select a).ToList();
        resItemInBranch.ForEach(d =>
        {
            int returnedQTY = itemCollections.FirstOrDefault(w => w.ItemID == d.ItemID).Qty;
            d.Qty = d.Qty + returnedQTY;
            d.UpdatedBy = request.deletedby;
            d.SetUpdatedDate(delDate);
            d.AddDomainEvent(new TMItemInBranchUpdateEvent(d));
        });

        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            message = "Success",
            soruce = "DB",
            status = StatusCodes.Status200OK.ToString(),
            data = new CommandResponse { result = true }
        };
    }
}
