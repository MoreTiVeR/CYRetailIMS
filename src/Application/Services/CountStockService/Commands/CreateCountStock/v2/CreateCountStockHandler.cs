using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTCountStockDetails;
using CYRetailIMS.Domain.Events.TTCountStocks;
using CYRetailIMS.Domain.Events.TTCountStocksHistorys;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v2;
public class CreateCountStockHandler : BaseService, IRequestHandler<CreateCountStockCommand, BaseResponse<CommandResponse>>
{
    public CreateCountStockHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateCountStockCommand request, CancellationToken cancellationToken)
    {
        DateTime cDateTime = request.countstockdate;
        string counterRole = string.IsNullOrWhiteSpace(request.counterrole) ? "PC" : request.counterrole;
        bool isPartialSave = request.ispartialsave;
        int requestedStatus = request.counterstockstatusid; // 0=Draft, 1=Submitted
        DateTime dayStart = request.countstockdate.Date;
        DateTime dayEnd = dayStart.AddDays(1);

        var existingSameDayQuery = await _unitOfWork.Repository<TTCountStock>().FindWithInclude(
            w => w.IsActive
                && w.BranchID == request.branchid
                && w.CounterRole == counterRole
                && w.CountDate >= dayStart
                && w.CountDate < dayEnd,
            i => i.Include(s => s.TTCountStockDetails));

        var existingSameDay = existingSameDayQuery
            .OrderByDescending(o => o.CreatedDate)
            .ThenByDescending(o => o.CountStockID)
            .ToList();

        TTCountStock? existingSubmitted = existingSameDay
            .FirstOrDefault(w => w.CountStockStatusID == 1 || w.CountStockStatusID == 2);

        TTCountStock? existingDraft = existingSameDay
            .FirstOrDefault(w => w.CountStockStatusID == 0);

        if (requestedStatus == 0)
        {
            if (existingSubmitted != null)
            {
                throw new Exception("รายการนับสต๊อกของวันนี้ถูกส่งแล้ว ไม่สามารถบันทึกแบบร่างซ้ำได้");
            }

            if (existingDraft != null)
            {
                await UpdateExistingDraftAsync(existingDraft, request, cDateTime, counterRole, isPartialSave);
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

        if (requestedStatus == 1)
        {
            if (existingSubmitted != null)
            {
                throw new Exception("รายการนับสต๊อกของวันนี้ถูกส่งแล้ว กรุณาทำรายการใหม่ในวันถัดไป");
            }

            if (existingDraft != null)
            {
                await UpdateExistingDraftAsync(existingDraft, request, cDateTime, counterRole, isPartialSave, forceSubmitted: true);
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

        #region 1.) Create CountStock Header & Detail
        TTCountStock countStockEnt = PreapreCountStock(request, cDateTime, counterRole);
        await _unitOfWork.Repository<TTCountStock>().AddAsync(countStockEnt);
        #endregion

        #region 3.) Create CountStocksHistory from TMItemInBranch and WarehouseQty from branch 1 office
        IEnumerable<TMItemInBranch> resItemBranch = await _unitOfWork.Repository<TMItemInBranch>().FindWithInclude(w => w.BranchID == request.branchid, 
            i => i.Include(s => s.Item));
        if (!resItemBranch.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้าในสาขาที่ทำรายการ กรุณาลองใหม่อีกครั้ง");
        }

        List<TTCountStocksHistory> countStocksHistories = PrepareCountStocksHistory(resItemBranch, request.createdby, cDateTime);
        await _unitOfWork.Repository<TTCountStocksHistory>().AddRangeAsync(countStocksHistories);
        #endregion

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

    private async Task UpdateExistingDraftAsync(TTCountStock existingDraft, CreateCountStockCommand request, DateTime cDateTime, string counterRole, bool isPartialSave, bool forceSubmitted = false)
    {
        if (existingDraft.CountStockID <= 0)
        {
            throw new Exception("ไม่พบเอกสารแบบร่างที่ถูกต้องสำหรับการอัปเดต");
        }

        if (request.detail == null || !request.detail.Any())
        {
            throw new Exception("ไม่พบรายการนับสต๊อกสำหรับบันทึกแบบร่าง");
        }

        existingDraft.CountDate = request.countstockdate;
        // Do not trust request.totalcount for partial updates from filtered UI.
        // We will recalculate from merged detail rows after applying changes.
        existingDraft.TotalCount = 0;
        existingDraft.Remark = request.remark;
        existingDraft.CounterRole = counterRole;
        existingDraft.CountStockStatusID = forceSubmitted ? 1 : request.counterstockstatusid;
        existingDraft.UpdatedBy = request.createdby;
        existingDraft.UpdatedDate = cDateTime;

        // Deduplicate by ItemID + SubItemTypeID to keep one authoritative row per item.
        var normalizedDetails = request.detail
            .Where(w => w.itemid > 0 && w.subitemtypeid > 0)
            .GroupBy(g => new { g.itemid, g.subitemtypeid })
            .Select(s => s.Last())
            .ToList();

        if (!normalizedDetails.Any())
        {
            throw new Exception("ไม่พบรายการนับสต๊อกที่มีประเภทย่อยถูกต้องสำหรับบันทึก");
        }

        var existingDetails = existingDraft.TTCountStockDetails?.ToList() ?? new List<TTCountStockDetail>();

        // Remove legacy rows that were saved before ItemID support to prevent duplicate counting.
        var legacyDetails = existingDetails
            .Where(w => !w.ItemID.HasValue || w.ItemID.Value <= 0)
            .ToList();
        if (legacyDetails.Any())
        {
            _unitOfWork.Repository<TTCountStockDetail>().DeleteRange(legacyDetails);
            existingDetails = existingDetails.Except(legacyDetails).ToList();
        }

        var existingBySubItemType = existingDetails
            .Where(w => w.ItemID.HasValue && w.ItemID.Value > 0)
            .GroupBy(g => new { itemid = g.ItemID!.Value, subitemtypeid = g.SubItemTypeID })
            .ToDictionary(k => k.Key, v => v.First());

        var incomingSubItemTypeIds = normalizedDetails
            .Select(s => new { s.itemid, s.subitemtypeid })
            .ToHashSet();

        // For full save mode, replace the full set by removing rows that are absent from payload.
        // For partial save mode, keep rows that are not included in current payload.
        if (!isPartialSave)
        {
            var removeDetails = existingDetails
                .Where(w => (w.ItemID ?? 0) > 0)
                .Where(w => !incomingSubItemTypeIds.Contains(new { itemid = w.ItemID ?? 0, subitemtypeid = w.SubItemTypeID }))
                .ToList();

            if (removeDetails.Any())
            {
                _unitOfWork.Repository<TTCountStockDetail>().DeleteRange(removeDetails);
                foreach (var removeDetail in removeDetails)
                {
                    existingBySubItemType.Remove(new { itemid = removeDetail.ItemID ?? 0, subitemtypeid = removeDetail.SubItemTypeID });
                }
            }
        }

        var insertDetails = new List<TTCountStockDetail>();

        foreach (var detail in normalizedDetails)
        {
            var incomingKey = new { detail.itemid, detail.subitemtypeid };
            if (existingBySubItemType.TryGetValue(incomingKey, out var existingDetail))
            {
                existingDetail.ItemID = detail.itemid;
                existingDetail.QtyInBranchOfCountStockDay = detail.qtyinbranchofcountstockday;
                existingDetail.QtyInBranch = detail.qtyinbranch;
                existingDetail.CountedAmountQty = detail.countedamountqty;
                existingDetail.PendingReStockQty = detail.pendingrestockqty;
                existingDetail.DamagedQty = detail.damagedqty;
                existingDetail.SaleBeforeCountQty = detail.salebeforecountqty;
                existingDetail.TotalCountQty = detail.totalcountqty;
                existingDetail.ShortageSurplusQty = detail.shortagesurplusqty;
                existingDetail.ItemRemark = detail.itemremark;
                existingDetail.UpdatedBy = request.createdby;
                existingDetail.UpdatedDate = cDateTime;
                existingDetail.IsActive = true;
                existingDetail.AddDomainEvent(new TTCountStockDetailUpdateEvent(existingDetail));
                continue;
            }

            var newDetail = PrepareSingleCountStockDetail(detail, request.createdby, cDateTime);
            newDetail.CountStockID = existingDraft.CountStockID;
            insertDetails.Add(newDetail);
        }

        if (insertDetails.Any())
        {
            await _unitOfWork.Repository<TTCountStockDetail>().AddRangeAsync(insertDetails);
        }

        existingDraft.TotalCount = existingBySubItemType.Values.Sum(s => s.TotalCountQty) + insertDetails.Sum(s => s.TotalCountQty);

        _unitOfWork.Repository<TTCountStock>().Update(existingDraft);

        IEnumerable<TMItemInBranch> resItemBranch = await _unitOfWork.Repository<TMItemInBranch>().FindWithInclude(
            w => w.BranchID == request.branchid,
            i => i.Include(s => s.Item));

        if (!resItemBranch.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้าในสาขาที่ทำรายการ กรุณาลองใหม่อีกครั้ง");
        }

        List<TTCountStocksHistory> countStocksHistories = PrepareCountStocksHistory(resItemBranch, request.createdby, cDateTime);
        await _unitOfWork.Repository<TTCountStocksHistory>().AddRangeAsync(countStocksHistories);

        await _unitOfWork.SaveChangesAsync();
    }

    private TTCountStock PreapreCountStock(CreateCountStockCommand stockCommand, DateTime cDateTime, string counterRole)
    {
        TTCountStock stockEnt = new TTCountStock
        {
            BranchID = stockCommand.branchid,
            CountDate = stockCommand.countstockdate,
            TotalCount = stockCommand.totalcount,
            Remark = stockCommand.remark,
            CountStockStatusID = stockCommand.counterstockstatusid,
            CounterRole = counterRole,
            CreatedBy = stockCommand.createdby,
            CreatedDate = cDateTime,
            IsActive = true
        };
        stockEnt.TTCountStockDetails = PrepareCountStockDetail(stockCommand.detail, stockCommand.createdby, cDateTime);
        stockEnt.AddDomainEvent(new TTCountStockCreateEvent(stockEnt));
        return stockEnt;
    }

    private List<TTCountStockDetail> PrepareCountStockDetail(List<CreateCountStockDetail> countStockDetails, string createdBy, DateTime cDateTime)
    {
        List<TTCountStockDetail> stockDetails = countStockDetails.Select(s => PrepareSingleCountStockDetail(s, createdBy, cDateTime)).ToList();
        stockDetails.ForEach(ent =>
        {
            ent.AddDomainEvent(new TTCountStockDetailCreateEvent(ent));
        });
        return stockDetails;
    }

    private TTCountStockDetail PrepareSingleCountStockDetail(CreateCountStockDetail detail, string createdBy, DateTime cDateTime)
    {
        return new TTCountStockDetail
        {
            ItemID = detail.itemid,
            SubItemTypeID = detail.subitemtypeid,
            QtyInBranchOfCountStockDay = detail.qtyinbranchofcountstockday,
            QtyInBranch = detail.qtyinbranch,
            CountedAmountQty = detail.countedamountqty,
            PendingReStockQty = detail.pendingrestockqty,
            DamagedQty = detail.damagedqty,
            SaleBeforeCountQty = detail.salebeforecountqty,
            TotalCountQty = detail.totalcountqty,
            ShortageSurplusQty = detail.shortagesurplusqty,
            ItemRemark = detail.itemremark,
            CreatedBy = createdBy,
            CreatedDate = cDateTime,
            IsActive = true
        };
    }

    private List<TTCountStocksHistory> PrepareCountStocksHistory(IEnumerable<TMItemInBranch> itemInBranches, string createdBy, DateTime cDateTime)
    {
        List<TTCountStocksHistory> countStrockHistories = itemInBranches.Select(s => new TTCountStocksHistory
        {
            BranchID = s.BranchID,
            ItemID = s.ItemID,
            Price = s.Price,
            DiscountPercent = s.DiscountPercent,
            Qty = s.Qty,
            NotifyMinQty = s.NotifyMinQty,
            NotifyMaxQty = s.NotifyMaxQty,
            CreatedBy = createdBy,
            CreatedDate = cDateTime,
            IsActive = true,
            WarehouseQty = s.Item.Qty
        }).ToList();
        countStrockHistories.ForEach(ent =>
        {
            ent.AddDomainEvent(new TTCountStocksHistoryCreateEvent(ent));
        });
        return countStrockHistories;
    }
}
