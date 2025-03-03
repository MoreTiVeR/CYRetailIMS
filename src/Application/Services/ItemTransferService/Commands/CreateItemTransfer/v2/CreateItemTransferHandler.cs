using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using CYRetailIMS.Domain.Events.TMItems;
using CYRetailIMS.Domain.Events.TTDraftItemTransferDetails;
using CYRetailIMS.Domain.Events.TTDraftItemTransfers;
using CYRetailIMS.Domain.Events.TTItemTransferHeaders;
using CYRetailIMS.Domain.Events.TTItemTransfers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v2;
public class CreateItemTransferHandler : BaseService, IRequestHandler<CreateItemTransferWithDraftCommand, BaseResponse<CommandResponse>>
{
    public CreateItemTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    /// <summary>
    /// Disable Check exist branchid and itemid in TTItemTransfer 12-7-2024
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<BaseResponse<CommandResponse>> Handle(CreateItemTransferWithDraftCommand request, CancellationToken cancellationToken)
    {
        string transferRefNo = $"{request.createddate:yyMMddFFF}{request.destinationid:000}";
        #region Check exist branchid and itemid in TTItemTransfer
        //var isExist = await _unitOfWork.Repository<TTItemTransfer>().AnyAsync(w => w.DestinationID == request.destinationid
        //&& request.items.Select(s => s.itemid).Contains(w.ItemID)
        //&& w.TransferStatus == (int)TransferStatus.Pending);
        //if (isExist)
        //{
        //    throw new Exception("ไม่สามารถทำรายการได้ เนื่องจากสาขาดังกล่าวมีรายการค้างรับโอนในระบบ");
        //}
        #endregion

        #region Begin transaction
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
        #endregion

        #region Craete TTItemTransferHeader & TTItemTransfer
        TTItemTransferHeader itemTransferHeader = PrepareTTItemTransferHeader(transferRefNo, request);
        itemTransferHeader.TTItemTransfers.ToList().ForEach(e =>
        {
            e.AddDomainEvent(new TTItemTransferCreateEvent(e));
        });
        itemTransferHeader.AddDomainEvent(new TTItemTransferHeaderCreateEvent(itemTransferHeader));
        await _unitOfWork.Repository<TTItemTransferHeader>().AddAsync(itemTransferHeader);
        #endregion

        #region Update Source Branch Stock | ตัด Stock สาขาต้นทาง
        resItemInDestinationBranchList = new List<TMItemInBranch>();
        if (request.transfertypeid == (int)TransferType.WTB)
        {
            //คลัง-สาขา
            resItemInWarehouse.ToList().ForEach(s =>
            {
                int minusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
                ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
                s.Qty = s.Qty - minusQty;
                s.SetUpdatedDate(request.createddate);
                s.SetUpdatedBy(request.createdby);
                s.AddDomainEvent(new TMItemUpdateEvent(s));
            });
        }
        else
        {
            //สาขา-สาขา
            resItemInSourceBranch.ToList().ForEach(s =>
            {
                int minusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
                ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
                s.Qty = s.Qty - minusQty;
                s.SetUpdatedDate(request.createddate);
                s.SetUpdatedBy(request.createdby);
                s.AddDomainEvent(new TMItemInBranchUpdateEvent(s));
            });
        }

        #endregion

        #region เพิ่ม, อัพเดท Stock สาขาปลายทาง | Approvestatus = Approve thne Add, Update Destination Branch Stock
        if (request.transferstatus == (int)TransferStatus.Received)
        {
            if (resItemInDestinationBranch.Count() == 0)
            {
                List<TMItemInBranch> tmItemInDestinationBranch = new List<TMItemInBranch>();
                //Add  Destination Stock
                request.items.ForEach(e =>
                {
                    TMItem item = resItemInWarehouse.FirstOrDefault(w => w.ItemID == e.itemid);
                    TMItemInBranch tmItemBranch = new TMItemInBranch()
                    {
                        BranchID = request.destinationid,
                        ItemID = e.itemid,
                        DiscountPercent = 0,
                        Qty = e.qty,
                        Price = item.Price
                    };
                    tmItemBranch.SetCreatedDate(request.createddate);
                    tmItemBranch.SetCreatedBy(request.createdby);
                    tmItemBranch.ActiveStatus();
                    tmItemBranch.AddDomainEvent(new TMItemInBranchCreateEvent(tmItemBranch));
                    tmItemInDestinationBranch.Add(tmItemBranch);
                });
                await _unitOfWork.Repository<TMItemInBranch>().AddRangeAsync(tmItemInDestinationBranch);
            }
            else
            {
                //Update Destination Stock
                resItemInDestinationBranch.ToList().ForEach(s =>
                {
                    int plusQty = request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault() != null
                    ? request.items.Where(w => w.itemid == s.ItemID).FirstOrDefault().qty : 0;
                    s.Qty = s.Qty + plusQty;
                    s.SetUpdatedDate(request.createddate);
                    s.SetUpdatedBy(request.createdby);
                    s.AddDomainEvent(new TMItemInBranchUpdateEvent(s));
                });
            }
        }
        #endregion

        #region New Added for V2 -> save transaction to TTDraftItemTransfer, TTDraftItemTransferDetail
        TTDraftItemTransfer draftItemTransfer = PrepareTTDraftItemTransfer(transferRefNo, itemTransferHeader);
        draftItemTransfer.SetCreatedBy(itemTransferHeader.CreatedBy);
        draftItemTransfer.SetCreatedDate(itemTransferHeader.CreatedDate);
        draftItemTransfer.ActiveStatus();
        draftItemTransfer.TTDraftItemTransferDetails.ToList().ForEach(e =>
        {
            e.AddDomainEvent(new TTDraftItemTransferDetailCreateEvent(e));
        });
        draftItemTransfer.AddDomainEvent(new TTDraftItemTransferCreateEvent(draftItemTransfer));
        await _unitOfWork.Repository<TTDraftItemTransfer>().AddAsync(draftItemTransfer);
        #endregion

        #region Commit Tran
        await _unitOfWork.SaveChangesAsync();

        #region NEW Create ItemTransferhistory Log
        if (request.transferhistorylogs?.Count > 0)
        {
            List<TTItemTransferHistory> transferHistories = PrepareTransferHistoryLogs(request.transferhistorylogs, request.createddate, request.createdby);
            transferHistories.ForEach(e =>
            {
                e.TransferHeaderID = itemTransferHeader.TransferHeaderID;
            });
            await _unitOfWork.Repository<TTItemTransferHistory>().AddRangeAsync(transferHistories);
        }
        #endregion

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
    private TTItemTransferHeader PrepareTTItemTransferHeader(string transferRefNo, CreateItemTransferCommand itemTransferCommand)
    {
        TTItemTransferHeader ItemTransferHeader = new TTItemTransferHeader
        {
            TransferRefNo = transferRefNo,
            TransferTypeID = itemTransferCommand.transfertypeid,
            SourceBranchID = itemTransferCommand.sourceid,
            DestinationBranchID = itemTransferCommand.destinationid,
            Description = itemTransferCommand.description,
            CreatedBy = itemTransferCommand.createdby,
            CreatedDate = itemTransferCommand.createddate,
            IsActive = itemTransferCommand.isactive,
            TransferStatus = (int)TransferStatus.Pending,
            TTItemTransfers = (from a in itemTransferCommand.items
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
                                   CreatedDate = itemTransferCommand.createddate,
                                   IsActive = t.isactive,
                                   TransferStatus = t.transferstatus
                               }).ToList()
        };
        return ItemTransferHeader;
    }

    private bool ValidateQtyInBranchStock(CreateItemTransferCommand request, IEnumerable<TMItem> itemInSourceWarehouse)
    {
        if (itemInSourceWarehouse.Any(s => s.Qty <= 0))
        {
            throw new Exception($"ขออภัย, จำนวนสินค้าในสต๊อกในคลังไม่เพียงพอ!");
        }

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

    private TTDraftItemTransfer PrepareTTDraftItemTransfer(string transferRefNo, TTItemTransferHeader itemTransferHeader)
    {
        TTDraftItemTransfer draftItemTransfer = new TTDraftItemTransfer
        {
            TransferRefNo = transferRefNo,
            TransferTypeID = (int)EnumModel.TransferType.WTB,
            SourceBranchID = itemTransferHeader.SourceBranchID,
            DestinationBranchID = itemTransferHeader.DestinationBranchID,
            Description = itemTransferHeader.Description,
            TransferStatus = (int)EnumModel.TransferStatus.Received,
            TTDraftItemTransferDetails = (from a in itemTransferHeader.TTItemTransfers
                                          select new TTDraftItemTransferDetail
                                          {
                                              ItemID = a.ItemID,
                                              Qty = a.Qty,
                                              CreatedBy = itemTransferHeader.CreatedBy,
                                              CreatedDate = itemTransferHeader.CreatedDate,
                                              IsActive = itemTransferHeader.IsActive
                                          }).ToList()
        };
        return draftItemTransfer;
    }

    private List<TTItemTransferHistory> PrepareTransferHistoryLogs(List<CreateItemTransferHistoryRequest> transferHistoryRequests, 
        DateTime createdDate, string createdBy)
    {
        List<TTItemTransferHistory> res = transferHistoryRequests.Select(s => new TTItemTransferHistory
        {
            BranchID = s.branchid,
            ItemID = s.itemid,
            ItemCode = s.itemcode,
            ItemName = s.itemname,
            BrandID = s.brandid,
            QtyInStock = s.qtyinstock,
            QtyInBranch = s.qtyinbranch,
            NotifyMinQty = s.notifyminqty,
            SuggestRefillQtyBySystem = s.orderqty,
            RefillQty = s.refillqty,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            IsActive = true
        }).ToList();
        return res;
    }
    #endregion
}
