using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItems;
using CYRetailIMS.Domain.Events.TTItemTransactionLogs;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
public class UpdateItemHandler : BaseService, IRequestHandler<UpdateItemCommand, BaseResponse<CommandResponse>>
{
    private readonly ILog4NetLogger _log;

    public UpdateItemHandler(IMapper mapper, IUnitOfWork unitOfWork, ILog4NetLogger log) : base(mapper, unitOfWork)
    {
        _log = log;
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        _log.Info($"Invoke UpdateItemHandler request: {request.ToJson()}");
        TMItem itemEnt = await _unitOfWork.Repository<TMItem>().FirstOrDefaultAsync(w => w.ItemID == request.itemid);
        if (itemEnt == null)
        {
            throw new Exception("ไม่พบข้อมูลสินค้าในระบบ");
        }

        #region If isactive = 0 -> Check exist transfer item
        if (!request.isactive)
        {
            var resItemTransfer = await _unitOfWork.Repository<TTItemTransfer>().FindListAsync(w => w.ItemID == request.itemid
            && w.TransferStatus == (int)EnumModel.TransferStatus.Pending);
            if (resItemTransfer.Any())
            {
                throw new Exception("ไม่สามารถลบสินค้าได้, เนื่องจากมีรายการค้างโอนสินค้าในระบบ");
            }

            var resDraftItem = (from a in await _unitOfWork.Repository<TTDraftItemTransfer>().QueryAsync(w => w.IsActive == true && w.TransferStatus == (int)EnumModel.TransferStatus.Draft)
                                join b in await _unitOfWork.Repository<TTDraftItemTransferDetail>().QueryAsync(w => w.IsActive == true && w.ItemID == request.itemid)
                                on a.TransferHeaderID equals b.TransferHeaderID
                                select a).AsEnumerable();
            if (resDraftItem.Any())
            {
                throw new Exception("ไม่สามารถลบสินค้าได้, เนื่องจากมีรายการร่างโอนสินค้า ค้างในระบบ");
            }
        }
        #endregion

        //Price change -> insert TTItemTransactionLogs
        if (itemEnt.Price != request.price)
        {
            TTItemTransactionLog itemTransactionLog = new TTItemTransactionLog
            {
                ItemID = itemEnt.ItemID,
                BranchID = 1, //สำนักงานใหญ่
                OldPrice = itemEnt.Price,
                NewPrice = request.price
            };
            itemTransactionLog.SetCreatedBy(request.updatedby);
            itemTransactionLog.SetCreatedDate();
            itemTransactionLog.AddDomainEvent(new TTItemTransactionLogCreateEvent(itemTransactionLog));
            _unitOfWork.Repository<TTItemTransactionLog>().Add(itemTransactionLog);
        }
        #region Update
        itemEnt.Name = request.name;
        if (request.subitemid.HasValue)
        {
            itemEnt.SubItemTypeID = request.subitemid.Value;
        }
        itemEnt.ShortName = request.shortname;
        itemEnt.BarCode = !string.IsNullOrEmpty(request.barcode) ? request.barcode : null;
        itemEnt.Description = request.description;
        itemEnt.Price = request.price;
        //itemEnt.Qty = request.qty; //ไม่สามารถก้ไขจำนวนจากเมนู อัพเดทสินค้า ต้องทำรายการแก้ไขราคาผ่าน adjust สินค้า
        itemEnt.NotifyMinQty = request.notifyqty;
        itemEnt.NotifyMaxQty = request.notifymaxqty;
        itemEnt.DiscountPercent = request.discountpercent;
        itemEnt.ItemImageUrl = !string.IsNullOrEmpty(request.itemimageurl) ? request.itemimageurl : null;
        itemEnt.IsActive = request.isactive;
        itemEnt.SetUpdatedBy(request.updatedby);
        itemEnt.SetUpdatedDate();
        #endregion

        itemEnt.AddDomainEvent(new TMItemUpdateEvent(itemEnt));
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
