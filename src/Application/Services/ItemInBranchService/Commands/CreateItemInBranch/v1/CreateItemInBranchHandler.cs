using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using CYRetailIMS.Domain.Events.TMItems;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v1;
public class CreateItemInBranchHandler : BaseService, IRequestHandler<CreateItemInBranchListCommand, BaseResponse<CommandResponse>>
{
    public CreateItemInBranchHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateItemInBranchListCommand request, CancellationToken cancellationToken)
    {
        DateTime createdDate = DateTime.Now;
        string createdBy = request.items.FirstOrDefault().createdby;

        #region Check TTItemTransfer and TransferStatus=0 รออนุมัติ by (DestinationID) branchid
        var isTransferItem = await _unitOfWork.Repository<TTItemTransfer>().AnyAsync(w => w.DestinationID == request.branchid && w.TransferStatus == (int)EnumModel.TransferStatus.Pending);
        if (isTransferItem)
        {
            throw new Exception("ไม่สามารถนำเขาข้อมูลได้ เนื่องจากมีรายการค้างโอนของสาขา!");
        }
        #endregion

        #region Set current item in branch isactvie is false
        var resItemInBranch = await _unitOfWork.Repository<TMItemInBranch>().FindListAsync(w => w.BranchID == request.branchid);
        if (!resItemInBranch.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้าสาขา!");
        }
        resItemInBranch.ToList().ForEach(e =>
        {
            e.IsActive = false;
            e.SetUpdatedBy(createdBy);
            e.SetUpdatedDate(createdDate);
            e.AddDomainEvent(new TMItemInBranchUpdateEvent(e));
        });
        #endregion

        #region Create Item in branch history before import

        #endregion

        #region Update only matched CreateItemInBranchCommand request and set isactvie is true
        List<CreateItemInBranchDetailCommand> updateItemEntity = request.items.Where(w => w.isupdate).ToList();
        if (updateItemEntity.Count > 0)
        {
            List<int> itemIdList = updateItemEntity.Select(s => s.itemid).ToList();
            IEnumerable<TMItemInBranch> resUpdateItemEnt = await _unitOfWork.Repository<TMItemInBranch>().FindListAsync(w => itemIdList.Contains(w.ItemID));
            resUpdateItemEnt.ToList().ForEach(e =>
            {
                CreateItemInBranchDetailCommand reqItem = updateItemEntity.FirstOrDefault(w => w.itemid == e.ItemID);
                e.Qty = e.Qty + reqItem.qty;
                e.Price = reqItem.price;
                e.NotifyMinQty = reqItem.notifyminqty;
                e.NotifyMaxQty = reqItem.notifymaxqty;
                e.SetUpdatedBy(createdBy);
                e.SetUpdatedDate(createdDate);
                e.ActiveStatus();
                e.AddDomainEvent(new TMItemInBranchUpdateEvent(e));
            });
        }
        #endregion

        #region Create new if not matched
        List<CreateItemInBranchDetailCommand> newItemEntity = request.items.Where(w => !w.isupdate).ToList();
        if (newItemEntity.Count > 0)
        {
            List<TMItemInBranch> newItemEntities = _mapper.Map<List<TMItemInBranch>>(newItemEntity);
            newItemEntities.ForEach(e =>
            {
                e.SetCreatedBy(createdBy);
                e.SetCreatedDate(createdDate);
                e.AddDomainEvent(new TMItemInBranchCreateEvent(e));
            });
            await _unitOfWork.Repository<TMItemInBranch>().AddRangeAsync(newItemEntities);
        }
        #endregion

        #region Save change
        await _unitOfWork.SaveChangesAsync();
        #endregion

        return new BaseResponse<CommandResponse>
        {
            result = true,
            soruce = "db",
            message = "Success",
            status = StatusCodes.Status200OK.ToString()
        };

    }
}
