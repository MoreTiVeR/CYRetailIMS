using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using CYRetailIMS.Domain.Events.TMUserInBranchs;
using CYRetailIMS.Domain.Events.TTItemTransactionLogs;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
public class UpdateItemInBranchHandler : BaseService, IRequestHandler<UpdateItemInBranchCommand, BaseResponse<CommandResponse>>
{
    public UpdateItemInBranchHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateItemInBranchCommand request, CancellationToken cancellationToken)
    {
        IQueryable<TMItemInBranch> resItemInBranch = await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.ItemID == request.itemid 
        && w.BranchID == request.branchid);
        if (resItemInBranch == null || !resItemInBranch.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้าในสาขา");
        }

        resItemInBranch.ToList().ForEach(e =>
        {
            //Price change -> insert TTItemTransactionLogs
            if (e.Price != request.price)
            {
                TTItemTransactionLog itemTransactionLog = new TTItemTransactionLog
                {
                    ItemID = e.ItemID,
                    BranchID = e.BranchID,
                    OldPrice = e.Price,
                    NewPrice = request.price
                };
                itemTransactionLog.SetCreatedBy(request.updatedby);
                itemTransactionLog.SetCreatedDate();
                itemTransactionLog.AddDomainEvent(new TTItemTransactionLogCreateEvent(itemTransactionLog));
                _unitOfWork.Repository<TTItemTransactionLog>().Add(itemTransactionLog);
            }

            e.Price = request.price;
            e.Qty = request.qty;
            e.NotifyMinQty = request.notifyminqty;
            e.SetUpdatedBy(request.updatedby);
            e.SetUpdatedDate(request.updateddate);
            //e.IsActive = request.isactive;
            e.AddDomainEvent(new TMItemInBranchUpdateEvent(e));
        });

        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
