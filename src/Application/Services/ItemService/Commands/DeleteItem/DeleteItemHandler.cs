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
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
public class DeleteItemHandler : BaseService, IRequestHandler<DeleteItemCommand, BaseResponse<CommandResponse>>
{
    private readonly ILog4NetLogger _log;
    public DeleteItemHandler(IMapper mapper, IUnitOfWork unitOfWork, ILog4NetLogger log) : base(mapper, unitOfWork)
    {
        _log = log;
    }

    /// <summary>
    /// Validate if have row in TTItemTransfer, TTDraftItemTransferDetail
    /// then can't delete
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<BaseResponse<CommandResponse>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        _log.Info($"Invoke DeleteItemHandler request: {request.ToJson()}");
        TMItem itemEnt = await _unitOfWork.Repository<TMItem>().FirstOrDefaultAsync(w => w.ItemID == request.itemid);
        if (itemEnt == null)
        {
            throw new Exception("ไม่พบข้อมูลสินค้าในระบบ");
        }

        #region Check if qty > 0 then can't delete
        if(itemEnt.Qty > 0)
        {
            throw new Exception("ไม่สามารถลบสินค้าได้, เนื่องจากมีสต๊อกคงเหลือในระบบ");
        }
        #endregion

        #region Check Have stock in branch then can't delete
        var resItemInBranch = await _unitOfWork.Repository<TMItemInBranch>().FindListAsync(w => w.ItemID == request.itemid && (w.Qty > 0 || w.IsActive));
        if (resItemInBranch.Any())
        {
            throw new Exception("ไม่สามารถลบสินค้าได้, เนื่องจากมีสต๊อกอยู่ในสาขา");
        }
        #endregion

        #region Check exist transfer item
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

        #endregion

        #region Update
        //itemEnt = _mapper.Map<TMItem>(request);
        itemEnt.IsActive = false;
        itemEnt.SetUpdatedBy(request.deletedby);
        itemEnt.SetUpdatedDate();
        #endregion

        itemEnt.AddDomainEvent(new TMItemDeleteEvent(itemEnt));
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
