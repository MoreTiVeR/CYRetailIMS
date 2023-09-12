using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using CYRetailIMS.Domain.Events.TMItems;
using CYRetailIMS.Domain.Events.TTItemTransfers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
public class CreateItemTransferHandler : BaseService, IRequestHandler<CreateItemTransferCommand, BaseResponse<CommandResponse>>
{
    public CreateItemTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateItemTransferCommand request, CancellationToken cancellationToken)
    {
        #region Validate Qty In Stock
        await _unitOfWork.BeginTransactionAsync();
        #endregion

        List<int> itemList = request.items.Select(s => s.itemid).ToList();
        IEnumerable<TMItem> resItemInWarehouse = null;
        IEnumerable<TMItemInBranch> resItemInSourceBranch = null;
        IEnumerable<TMItemInBranch> resItemInDestinationBranch = null;
        ICollection<TMItemInBranch> resItemInDestinationBranchList = null;

        #region Check Item in  Warehouse/Source Branch stock
        if (request.transfertypeid == (int)TransferType.WTB)
        {
            //คลัง-สาขา
            resItemInWarehouse = await _unitOfWork.Repository<TMItem>().QueryAsync(w => itemList.Contains(w.ItemID) && w.IsActive);
            if (resItemInWarehouse.Count() != itemList.Count)
            {
                throw new Exception("ขออภัย, ไม่พบสินค้าในคลังสินค้า!");
            }
        }
        else
        {
            //สาขา-สาขา
            resItemInSourceBranch = await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.BranchID == request.sourceid && itemList.Contains(w.ItemID) && w.IsActive);
            if (resItemInSourceBranch.Count() != itemList.Count)
            {
                throw new Exception("ขออภัย, ไม่พบสินค้าในสาขา!");
            }
        }

        #endregion

        #region Check Qty in Warehouse
        bool isAvailableStock = request.transfertypeid == (int)TransferType.WTB
            ? ValidateQtyInBranchStock(request, resItemInWarehouse) : ValidateQtyInBranchStock(request, resItemInSourceBranch);
        if (!isAvailableStock)
        {
            throw new Exception("ขออภัย, จำนวนสินค้าในสต๊อกไม่เพียงพอ!");
        }
        #endregion

        #region Validate Item in Destination Branch
        resItemInDestinationBranch = await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.BranchID == request.destinationid && itemList.Contains(w.ItemID) && w.IsActive);
        if (resItemInDestinationBranch.Count() != itemList.Count)
        {
            throw new Exception("ขออภัย, ไม่พบสินค้าที่ต้องการโอนในสาขาปลายทาง! กรุณาตรวจสอบรายการโอนสินค้าใหม่อีกครั้ง");
        }
        #endregion

        #region Craete TTItemTransfer
        ICollection<TTItemTransfer> itemTransferEntities = PrepreTTItemTransfer(request);
        itemTransferEntities.ToList().ForEach(e =>
        {
            e.AddDomainEvent(new TTItemTransferCreateEvent(e));
        });
        await _unitOfWork.Repository<TTItemTransfer>().AddRangeAsync(itemTransferEntities);
        #endregion

        #region Update Source Branch Stock | ตัด Stock สาขาต้นทาง
        resItemInDestinationBranchList = new List<TMItemInBranch>();
        if (request.transfertypeid == (int)TransferType.WTB)
        {
            //คลัง-สาขา
            //resItemInWarehouse = resItemInWarehouse.Select(s =>
            //{
            //    int minusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
            //    ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
            //    s.Qty = s.Qty - minusQty;
            //    return s;
            //}).ToList();
            resItemInWarehouse.ToList().ForEach(s =>
            {
                int minusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
                ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
                s.Qty = s.Qty - minusQty;
                s.AddDomainEvent(new TMItemUpdateEvent(s));
            });
        }
        else
        {
            //สาขา-สาขา
            //resItemInSourceBranch = resItemInSourceBranch.Select(s =>
            //{
            //    int minusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
            //    ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
            //    s.Qty = s.Qty - minusQty;
            //    //s.AddDomainEvent(new TMItemInBranchUpdateEvent(s));
            //    return s;
            //}).ToList();

            //resItemInSourceBranch.ToList().ForEach(e =>
            //{
            //    e.AddDomainEvent(new TMItemInBranchUpdateEvent(e));
            //});
            resItemInSourceBranch.ToList().ForEach(s =>
            {
                int minusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
                ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
                s.Qty = s.Qty - minusQty;
                s.AddDomainEvent(new TMItemInBranchUpdateEvent(s));
            });
        }

        #endregion

        #region Update Destination Branch Stock | ตัด Stock สาขาปลายทาง
        //resItemInDestinationBranch = resItemInDestinationBranch.Select(s =>
        //{
        //    int plusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
        //    ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
        //    s.Qty = s.Qty + plusQty;
        //    //s.AddDomainEvent(new TMItemInBranchUpdateEvent(s));
        //    return s;
        //}).ToList();
        //resItemInDestinationBranch.ToList().ForEach(e =>
        //{
        //    e.AddDomainEvent(new TMItemInBranchUpdateEvent(e));
        //});
        resItemInDestinationBranch.ToList().ForEach(s =>
        {
            int plusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
            ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
            s.Qty = s.Qty + plusQty;
            s.AddDomainEvent(new TMItemInBranchUpdateEvent(s));
        });
        #endregion

        #region Commit Tran
        await _unitOfWork.SaveChangesAsync();
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

    #region Private Method
    private ICollection<TTItemTransfer> PrepreTTItemTransfer(CreateItemTransferCommand itemTransferCommand)
    {
        return (from a in itemTransferCommand.items
                let t = itemTransferCommand
                select new TTItemTransfer
                {
                    TransferTypeID = t.transfertypeid,
                    SourceID = t.sourceid,
                    DestinationID = t.destinationid,
                    ItemID = a.itemid,
                    Qty = a.qty,
                    Description = t.description,
                    CreatedBy = t.createdby,
                    CreadedDate = itemTransferCommand.creadeddate,
                    IsActive = t.isactive,
                    ApproveStatus = t.approvestatus
                }).ToList();
    }

    private bool ValidateQtyInBranchStock(CreateItemTransferCommand request, IEnumerable<TMItem> itemInSourceWarehouse)
    {
        request.items.ForEach(item =>
        {
            if (!itemInSourceWarehouse.Any(stock => stock.Qty > 0 && item.qty <= stock.Qty))
            {
                throw new Exception($"ขออภัย, จำนวนสินค้าในสต๊อกในคลังไม่เพียงพอ!");
            }
        });
        return true;
    }

    private bool ValidateQtyInBranchStock(CreateItemTransferCommand request, IEnumerable<TMItemInBranch> itemInSourceBranch)
    {
        request.items.ForEach(item =>
        {
            if (!itemInSourceBranch.Any(stock => stock.Qty > 0 && item.qty <= stock.Qty))
            {
                throw new Exception($"ขออภัย, จำนวนสินค้าในสต๊อกสาขาไม่เพียงพอ!");
            }
        });
        return true;
    }
    #endregion
}
