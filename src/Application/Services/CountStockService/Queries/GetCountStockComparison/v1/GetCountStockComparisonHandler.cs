using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockComparison.v1;

/// <summary>
/// Handler: หน้าเทียบข้อมูล — เปรียบเทียบสต๊อกระบบกับยอดที่นับได้
/// พร้อมยอดขาย/สินค้าเข้า/สินค้าออกในช่วงวันที่ระบุ
/// </summary>
public class GetCountStockComparisonHandler : BaseService,
    IRequestHandler<GetCountStockComparisonQuery, BaseResponse<List<GetCountStockComparisonResponseDTO>>>
{
    public GetCountStockComparisonHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetCountStockComparisonResponseDTO>>> Handle(
        GetCountStockComparisonQuery request, CancellationToken cancellationToken)
    {
        // 1.) Items currently in branch
        IQueryable<TMItemInBranch> itemsInBranch = await _unitOfWork.Repository<TMItemInBranch>()
            .FindWithInclude(
                w => w.BranchID == request.branchid && w.IsActive,
                i => i.Include(s => s.Item));

        // 2.) Count stock records for this branch in the audit date range
        //     Include ALL statuses (0=Draft, 1=Submitted, 2=Approved) so Draft-only data is still visible.
        IQueryable<TTCountStock> countStocksQuery = await _unitOfWork.Repository<TTCountStock>()
            .FindWithInclude(
                w => w.BranchID == request.branchid && w.IsActive,
                i => i.Include(s => s.TTCountStockDetails));

        if (request.isnewentryonly.HasValue)
        {
            if (request.isnewentryonly.Value)
            {
                countStocksQuery = countStocksQuery.Where(w =>
                    w.TTCountStockDetails.Any(d => d.ItemID.HasValue && d.ItemID.Value > 0));
            }
            else
            {
                countStocksQuery = countStocksQuery.Where(w =>
                    !w.TTCountStockDetails.Any(d => d.ItemID.HasValue && d.ItemID.Value > 0));
            }
        }

        if (request.auditstartdate.HasValue)
            countStocksQuery = countStocksQuery.Where(w => w.CountDate >= request.auditstartdate.Value);
        if (request.auditenddate.HasValue)
            countStocksQuery = countStocksQuery.Where(w => w.CountDate < request.auditenddate.Value.AddDays(1));

        // Pick the single latest comparison date within the filtered scope
        DateTime? compareDate = countStocksQuery
            .OrderByDescending(o => o.CountDate)
            .Select(s => (DateTime?)s.CountDate.Date)
            .FirstOrDefault();

        if (!compareDate.HasValue)
        {
            return new BaseResponse<List<GetCountStockComparisonResponseDTO>>
            {
                result = true,
                data = new List<GetCountStockComparisonResponseDTO>(),
                message = "Success",
                soruce = "db",
                status = StatusCodes.Status200OK.ToString()
            };
        }

        DateTime compareDateStart = compareDate.Value.Date;
        DateTime compareDateEnd = compareDateStart.AddDays(1);
        var dailyCountStocks = countStocksQuery
            .Where(w => w.CountDate >= compareDateStart && w.CountDate < compareDateEnd)
            .ToList();

        // Latest document per role for the selected day
        var latestPcHeader = dailyCountStocks
            .Where(w => w.CounterRole == "PC" || w.CounterRole == null)
            .OrderByDescending(o => o.CreatedDate)
            .ThenByDescending(o => o.CountStockID)
            .FirstOrDefault();

        var latestHeadPcHeader = dailyCountStocks
            .Where(w => w.CounterRole == "HeadPC")
            .OrderByDescending(o => o.CreatedDate)
            .ThenByDescending(o => o.CountStockID)
            .FirstOrDefault();

        // Build count maps keyed by ItemID (preferred) and SubItemTypeID (legacy fallback).
        // Use TotalCountQty so that damaged/restock/sold-before-count are all included.
        var pcDetails = latestPcHeader?.TTCountStockDetails ?? new List<TTCountStockDetail>();
        var pcByItemId = pcDetails
            .Where(w => w.ItemID.HasValue && w.ItemID.Value > 0)
            .GroupBy(g => g.ItemID!.Value)
            .ToDictionary(k => k.Key, v => v.Sum(s => s.TotalCountQty));
        var pcBySubType = pcDetails
            .Where(w => !w.ItemID.HasValue || w.ItemID.Value <= 0)
            .GroupBy(g => g.SubItemTypeID)
            .ToDictionary(k => k.Key, v => v.Sum(s => s.TotalCountQty));

        var headPcDetails = latestHeadPcHeader?.TTCountStockDetails ?? new List<TTCountStockDetail>();
        var headPcByItemId = headPcDetails
            .Where(w => w.ItemID.HasValue && w.ItemID.Value > 0)
            .GroupBy(g => g.ItemID!.Value)
            .ToDictionary(k => k.Key, v => v.Sum(s => s.TotalCountQty));
        var headPcBySubType = headPcDetails
            .Where(w => !w.ItemID.HasValue || w.ItemID.Value <= 0)
            .GroupBy(g => g.SubItemTypeID)
            .ToDictionary(k => k.Key, v => v.Sum(s => s.TotalCountQty));

        // 3.) Stock transactions (sales in/out) for the requested sales date range
        IQueryable<TTStockTransaction> stockTxQuery = await _unitOfWork.Repository<TTStockTransaction>()
            .FindWithInclude(w => w.IsActive, i => i.Include(s => s.Item));

        if (request.salesstartdate.HasValue)
            stockTxQuery = stockTxQuery.Where(w => w.TransactionDate >= request.salesstartdate.Value);
        if (request.salesenddate.HasValue)
            stockTxQuery = stockTxQuery.Where(w => w.TransactionDate < request.salesenddate.Value.AddDays(1));

        var stockIn = stockTxQuery.Where(w => w.StockTypeID == 1)
            .GroupBy(g => g.Item.SubItemTypeID)
            .Select(g => new { SubItemTypeID = g.Key, TotalQty = g.Sum(x => x.Qty) })
            .ToList();

        var stockOut = stockTxQuery.Where(w => w.StockTypeID == 2)
            .GroupBy(g => g.Item.SubItemTypeID)
            .Select(g => new { SubItemTypeID = g.Key, TotalQty = g.Sum(x => x.Qty) })
            .ToList();

        // 4.) SubItemType lookup for display names
        IQueryable<TMSubItemType> subItemTypes = await _unitOfWork.Repository<TMSubItemType>().QueryAsync();

        // 5.) Build per-item rows (one row per TMItemInBranch — no SubItemType grouping)
        var itemList = itemsInBranch.ToList();

        // Optional SubItemType name filter — keeps rows whose item belongs to a matching SubItemType
        if (!string.IsNullOrEmpty(request.subitemtypename))
        {
            var matchedIds = subItemTypes
                .Where(s => s.SubItemCode.Contains(request.subitemtypename)
                         || s.SubTypeNameTH.Contains(request.subitemtypename))
                .Select(s => (int?)s.SubItemTypeID)
                .ToList();
            itemList = itemList.Where(g => matchedIds.Contains(g.Item.SubItemTypeID)).ToList();
        }

        var result = itemList.Select(item =>
        {
            int itemId   = item.ItemID;
            int subTypeId = item.Item.SubItemTypeID ?? 0;
            var subType  = subItemTypes.FirstOrDefault(s => s.SubItemTypeID == subTypeId);

            // Per-item count (ItemID key), fall back to SubItemTypeID aggregate for legacy rows
            int pcCount = pcByItemId.TryGetValue(itemId, out var pcQty) ? pcQty
                : (pcBySubType.TryGetValue(subTypeId, out var subPcQty) ? subPcQty : 0);

            int? headPcCount = headPcByItemId.TryGetValue(itemId, out var headQty) ? headQty
                : (headPcBySubType.TryGetValue(subTypeId, out var subHeadQty) ? subHeadQty : (int?)null);

            var inQty  = stockIn.FirstOrDefault(x => x.SubItemTypeID == subTypeId);
            var outQty = stockOut.FirstOrDefault(x => x.SubItemTypeID == subTypeId);

            return new GetCountStockComparisonResponseDTO
            {
                itemid           = itemId,
                itemcode         = item.Item.ItemCode,
                itemname         = item.Item.Name,
                comparedate      = compareDate,
                subitemtypeid    = subTypeId,
                subitemtypename  = subType?.SubItemCode ?? subType?.SubTypeNameTH ?? "ไม่มีประเภทย่อย",
                cy_stockqty      = item.Qty,
                headpc_countedqty = headPcCount,
                pc_countedqty    = pcCount,
                salesqty         = outQty?.TotalQty ?? 0,
                stockinqty       = inQty?.TotalQty ?? 0,
                stockoutqty      = outQty?.TotalQty ?? 0
            };
        }).ToList();

        return new BaseResponse<List<GetCountStockComparisonResponseDTO>>
        {
            result  = true,
            data    = result,
            message = "Success",
            soruce  = "db",
            status  = StatusCodes.Status200OK.ToString()
        };
    }
}
