using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.ApproveCountStock.v1;

public class ApproveCountStockHandler : BaseService, IRequestHandler<ApproveCountStockCommand, BaseResponse<CommandResponse>>
{
    public ApproveCountStockHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(ApproveCountStockCommand request, CancellationToken cancellationToken)
    {
        // --- Load and validate BEFORE opening a transaction ---
        IQueryable<TTCountStock> countStockQuery = await _unitOfWork.Repository<TTCountStock>()
            .FindWithInclude(
                w => w.CountStockID == request.countstockid && w.IsActive,
                i => i.Include(s => s.TTCountStockDetails));
        TTCountStock? countStock = countStockQuery.FirstOrDefault();

        if (countStock is null)
            throw new Exception("ไม่พบข้อมูลนับสต๊อก กรุณาลองใหม่อีกครั้ง");

        if (countStock.CounterRole != "HeadPC")
            throw new Exception("ไม่สามารถอนุมัติได้ เนื่องจากรายการนี้ไม่ได้ส่งโดยหัวหน้า PC");

        // Load branch items with navigation property (needed for SubItemType fallback)
        var branchItemsQuery = await _unitOfWork.Repository<TMItemInBranch>()
            .FindWithInclude(
                w => w.BranchID == countStock.BranchID && w.IsActive,
                i => i.Include(s => s.Item));
        var branchItems = branchItemsQuery.ToList();

        // Qty of items currently in transit OUT of this branch (deducted from source but not yet received)
        var pendingTransfersQuery = await _unitOfWork.Repository<TTItemTransfer>()
            .QueryAsync(w => w.SourceID == countStock.BranchID
                             && w.TransferStatus == (int)TransferStatus.Pending
                             && w.IsActive);
        // Map: ItemID → total qty in transit
        var inTransitQtyByItem = pendingTransfersQuery
            .GroupBy(g => g.ItemID)
            .ToDictionary(k => k.Key, v => v.Sum(s => s.Qty));

        // --- Begin transaction: status re-check inside transaction prevents double-approve ---
        await _unitOfWork.BeginTransactionAsync();

        // Re-fetch status inside transaction to guard against concurrent approvals
        var freshStatus = (await _unitOfWork.Repository<TTCountStock>()
            .QueryAsync(w => w.CountStockID == request.countstockid))
            .Select(s => s.CountStockStatusID)
            .FirstOrDefault();

        if (freshStatus != 1)
            throw new Exception("ไม่สามารถอนุมัติได้ เนื่องจากสถานะรายการไม่ถูกต้อง (อาจถูกอนุมัติไปแล้ว)");

        DateTime approvedAt = DateTime.Now;

        countStock.CountStockStatusID = 2;
        countStock.ApprovedBy  = request.approvedby;
        countStock.ApprovedDate = approvedAt;
        countStock.SetUpdatedBy(request.approvedby);
        countStock.SetUpdatedDate();
        _unitOfWork.Repository<TTCountStock>().Update(countStock);

        var stockAdjustments = new List<TTStockTransaction>();
        var approvalHistories = new List<TTCountStockApprovalHistory>();

        foreach (var detail in countStock.TTCountStockDetails)
        {
            // TotalCountQty = countedamount + pendingrestock + damaged + salebeforecount
            int targetQty = Math.Max(0, detail.TotalCountQty);

            if (detail.ItemID.HasValue && detail.ItemID.Value > 0)
            {
                // V2 per-item path
                var item = branchItems.FirstOrDefault(i => i.ItemID == detail.ItemID.Value);
                if (item == null) continue;

                // Adjust target upward if items are currently in-transit out from this branch;
                // those items were already subtracted from the snapshot Qty the HeadPC saw.
                int inTransit = inTransitQtyByItem.TryGetValue(item.ItemID, out var t) ? t : 0;
                int adjustedTarget = targetQty + inTransit;

                int beforeQty = item.Qty;
                int delta = adjustedTarget - item.Qty;
                item.Qty  = adjustedTarget;
                item.SetUpdatedBy(request.approvedby);
                item.SetUpdatedDate();
                _unitOfWork.Repository<TMItemInBranch>().Update(item);

                approvalHistories.Add(new TTCountStockApprovalHistory
                {
                    CountStockID = countStock.CountStockID,
                    CountStockDetailID = detail.CountStockDetailID,
                    BranchID = countStock.BranchID,
                    ItemID = item.ItemID,
                    SubItemTypeID = detail.SubItemTypeID,
                    QtyInBranchOfCountStockDay = detail.QtyInBranchOfCountStockDay,
                    QtyInBranchBeforeApprove = beforeQty,
                    QtyInBranchAfterApprove = item.Qty,
                    CountedAmountQty = detail.CountedAmountQty,
                    PendingReStockQty = detail.PendingReStockQty,
                    DamagedQty = detail.DamagedQty,
                    SaleBeforeCountQty = detail.SaleBeforeCountQty,
                    TotalCountQty = detail.TotalCountQty,
                    ShortageSurplusQty = detail.ShortageSurplusQty,
                    ItemRemark = detail.ItemRemark,
                    CounterRole = countStock.CounterRole ?? "PC",
                    ApprovedBy = request.approvedby,
                    CountStockDate = countStock.CountDate,
                    ApprovedDate = approvedAt,
                    CreatedBy = request.approvedby,
                    CreatedDate = approvedAt,
                    IsActive = true
                });

                // Audit: record stock adjustment transaction
                if (delta != 0)
                {
                    stockAdjustments.Add(new TTStockTransaction
                    {
                        StockTypeID     = delta > 0 ? 1 : 2, // 1=In, 2=Out
                        ItemID          = item.ItemID,
                        Qty             = Math.Abs(delta),
                        TransactionDate = approvedAt,
                        CreatedBy       = request.approvedby,
                        CreatedDate     = approvedAt,
                        IsActive        = true
                    });
                }
            }
            else
            {
                // V1 legacy path: distribute targetQty across items in the same SubItemType
                var itemsInSubType = branchItems
                    .Where(i => i.Item?.SubItemTypeID == detail.SubItemTypeID)
                    .ToList();
                if (!itemsInSubType.Any()) continue;

                int totalCurrentQty = itemsInSubType.Sum(i => i.Qty);

                // Distribute using floor + assign remainder to first item to avoid rounding loss
                int distributed = 0;
                for (int idx = 0; idx < itemsInSubType.Count; idx++)
                {
                    var item = itemsInSubType[idx];
                    int inTransit = inTransitQtyByItem.TryGetValue(item.ItemID, out var t) ? t : 0;

                    int share = totalCurrentQty > 0
                        ? (int)Math.Floor((double)item.Qty / totalCurrentQty * targetQty)
                        : targetQty / itemsInSubType.Count;

                    // Last item absorbs any leftover from floor rounding
                    if (idx == itemsInSubType.Count - 1)
                        share = (targetQty - distributed) + inTransit;
                    else
                        share += inTransit;

                    share = Math.Max(0, share);
                    int beforeQty = item.Qty;
                    int delta = share - item.Qty;
                    item.Qty = share;
                    item.SetUpdatedBy(request.approvedby);
                    item.SetUpdatedDate();
                    _unitOfWork.Repository<TMItemInBranch>().Update(item);
                    distributed += (share - inTransit);

                    approvalHistories.Add(new TTCountStockApprovalHistory
                    {
                        CountStockID = countStock.CountStockID,
                        CountStockDetailID = detail.CountStockDetailID,
                        BranchID = countStock.BranchID,
                        ItemID = item.ItemID,
                        SubItemTypeID = detail.SubItemTypeID,
                        QtyInBranchOfCountStockDay = detail.QtyInBranchOfCountStockDay,
                        QtyInBranchBeforeApprove = beforeQty,
                        QtyInBranchAfterApprove = item.Qty,
                        CountedAmountQty = detail.CountedAmountQty,
                        PendingReStockQty = detail.PendingReStockQty,
                        DamagedQty = detail.DamagedQty,
                        SaleBeforeCountQty = detail.SaleBeforeCountQty,
                        TotalCountQty = detail.TotalCountQty,
                        ShortageSurplusQty = detail.ShortageSurplusQty,
                        ItemRemark = detail.ItemRemark,
                        CounterRole = countStock.CounterRole ?? "PC",
                        ApprovedBy = request.approvedby,
                        CountStockDate = countStock.CountDate,
                        ApprovedDate = approvedAt,
                        CreatedBy = request.approvedby,
                        CreatedDate = approvedAt,
                        IsActive = true
                    });

                    if (delta != 0)
                    {
                        stockAdjustments.Add(new TTStockTransaction
                        {
                            StockTypeID     = delta > 0 ? 1 : 2,
                            ItemID          = item.ItemID,
                            Qty             = Math.Abs(delta),
                            TransactionDate = approvedAt,
                            CreatedBy       = request.approvedby,
                            CreatedDate     = approvedAt,
                            IsActive        = true
                        });
                    }
                }
            }
        }

        if (approvalHistories.Any())
            await _unitOfWork.Repository<TTCountStockApprovalHistory>().AddRangeAsync(approvalHistories);

        if (stockAdjustments.Any())
            await _unitOfWork.Repository<TTStockTransaction>().AddRangeAsync(stockAdjustments);

        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.CommitTransactionAsync();

        return new BaseResponse<CommandResponse>
        {
            result  = true,
            data    = new CommandResponse { result = true },
            message = "อนุมัติและปรับสต๊อกสำเร็จ",
            soruce  = "db",
            status  = StatusCodes.Status200OK.ToString()
        };
    }
}
