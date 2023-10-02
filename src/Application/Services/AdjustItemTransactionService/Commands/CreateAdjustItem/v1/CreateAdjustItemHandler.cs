using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItems;
using CYRetailIMS.Domain.Events.TTAdjustItemTransactions;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
public class CreateAdjustItemHandler : BaseService, IRequestHandler<CreateAdjustItemCommand, BaseResponse<CommandResponse>>
{
    public CreateAdjustItemHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateAdjustItemCommand request, CancellationToken cancellationToken)
    {
        #region Begin Transaction
        await _unitOfWork.BeginTransactionAsync();
        #endregion

        #region Re-check item & qty
        TMItem resItem = await _unitOfWork.Repository<TMItem>().FirstOrDefaultAsync(w => w.ItemID == request.itemid);
        if(resItem  == null)
        {
            throw new Exception("ไม่พบข้อมูลที่ต้องการปรับสต็อก");
        }
        #endregion

        #region Update qty in Item stock
        if(request.adjusttypeid == (int)AdjustItemType.Delete)
        {
            int minus = resItem.Qty - request.qty;
            if (resItem.Qty == 0 || resItem.Qty < request.qty || (resItem.Qty - request.qty < 0))
            {
                throw new Exception("จำนวนสินค้าไม่เพียงพอในการปรับสต็อก");
            }
            resItem.Qty -= request.qty;
        }
        else
        {
            resItem.Qty += request.qty;
        }
        
        resItem.SetUpdatedBy(request.createdby);
        resItem.SetUpdatedDate(request.createddate);
        resItem.AddDomainEvent(new TMItemUpdateEvent(resItem));
        #endregion

        #region Create Adjust Item
        TTAdjustItemTransaction adjustItemTransaction = new TTAdjustItemTransaction
        {
            AdjustTypeID = request.adjusttypeid,
            ItemID = request.itemid,
            Qty = request.qty,
            Remark = request.remark
        };
        adjustItemTransaction.ActiveStatus();
        adjustItemTransaction.SetCreatedBy(request.createdby);
        adjustItemTransaction.SetCreatedDate(request.createddate);
        adjustItemTransaction.AddDomainEvent(new TTAdjustItemTransactionCreateEvent(adjustItemTransaction));
        await _unitOfWork.Repository<TTAdjustItemTransaction>().AddAsync(adjustItemTransaction);

        #endregion

        #region Commit Transaction
        int rowAff = await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.CommitTransactionAsync();
        #endregion

        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }
}
