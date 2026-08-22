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
        // 1.) Fetch items in branch (current system stock)
        IQueryable<TMItemInBranch> itemsInBranch = await _unitOfWork.Repository<TMItemInBranch>()
            .FindWithInclude(
                w => w.BranchID == request.branchid && w.IsActive,
                i => i.Include(s => s.Item));

        // 2.) Fetch count stock records for this branch — no status filter for backward compat
        IQueryable<TTCountStock> countStocksQuery = await _unitOfWork.Repository<TTCountStock>()
            .FindWithInclude(
                w => w.BranchID == request.branchid && w.IsActive,
                i => i.Include(s => s.TTCountStockDetails));

        // Apply audit date filter
        if (request.auditstartdate.HasValue)
            countStocksQuery = countStocksQuery.Where(w => w.CountDate >= request.auditstartdate.Value);
        if (request.auditenddate.HasValue)
            countStocksQuery = countStocksQuery.Where(w => w.CountDate <= request.auditenddate.Value.AddDays(1));

        var pcCounts = countStocksQuery
            .Where(w => w.CounterRole == "PC" || w.CounterRole == null)
            .SelectMany(cs => cs.TTCountStockDetails.Select(d => new { d.SubItemTypeID, d.CountedAmountQty, cs.CountDate }))
            .GroupBy(g => g.SubItemTypeID)
            .Select(g => new { SubItemTypeID = g.Key, CountedQty = g.OrderByDescending(x => x.CountDate).First().CountedAmountQty })
            .ToList();

        var headPcCounts = countStocksQuery
            .Where(w => w.CounterRole == "HeadPC")
            .SelectMany(cs => cs.TTCountStockDetails.Select(d => new { d.SubItemTypeID, d.CountedAmountQty, cs.CountDate }))
            .GroupBy(g => g.SubItemTypeID)
            .Select(g => new { SubItemTypeID = g.Key, CountedQty = g.OrderByDescending(x => x.CountDate).First().CountedAmountQty })
            .ToList();

        // 3.) Fetch stock transactions (in/out) for the date range — TTStockTransaction
        IQueryable<TTStockTransaction> stockTxQuery = await _unitOfWork.Repository<TTStockTransaction>()
            .FindWithInclude(
                w => w.IsActive,
                i => i.Include(s => s.Item));

        if (request.salesstartdate.HasValue)
            stockTxQuery = stockTxQuery.Where(w => w.TransactionDate >= request.salesstartdate.Value);
        if (request.salesenddate.HasValue)
            stockTxQuery = stockTxQuery.Where(w => w.TransactionDate <= request.salesenddate.Value.AddDays(1));

        var stockIn = stockTxQuery.Where(w => w.StockTypeID == 1) // StockType In
            .GroupBy(g => g.Item.SubItemTypeID)
            .Select(g => new { SubItemTypeID = g.Key, TotalQty = g.Sum(x => x.Qty) })
            .ToList();

        var stockOut = stockTxQuery.Where(w => w.StockTypeID == 2) // StockType Out
            .GroupBy(g => g.Item.SubItemTypeID)
            .Select(g => new { SubItemTypeID = g.Key, TotalQty = g.Sum(x => x.Qty) })
            .ToList();

        // 4.) Group items by subitemtype and build comparison rows using SubItemTypeID
        // Use TMSubItemType join for proper name (subItemCode is used as display name)
        IQueryable<TMSubItemType> subItemTypes = await _unitOfWork.Repository<TMSubItemType>().QueryAsync();

        var grouped = itemsInBranch
            .GroupBy(g => g.Item.SubItemTypeID ?? 0)
            .ToList();

        // Apply subitemtype filter
        if (!string.IsNullOrEmpty(request.subitemtypename))
        {
            var matchedIds = subItemTypes
                .Where(s => s.SubItemCode.Contains(request.subitemtypename)
                         || s.SubTypeNameTH.Contains(request.subitemtypename))
                .Select(s => (int?)s.SubItemTypeID)
                .ToList();
            grouped = grouped.Where(g => matchedIds.Contains(g.Key)).ToList();
        }

        var result = grouped.Select(grp =>
        {
            int subTypeId = grp.Key;
            var subType = subItemTypes.FirstOrDefault(s => s.SubItemTypeID == subTypeId);
            var firstItem = grp.First();
            var pcCount = pcCounts.FirstOrDefault(x => x.SubItemTypeID == subTypeId);
            var headPcCount = headPcCounts.FirstOrDefault(x => x.SubItemTypeID == subTypeId);
            var inQty = stockIn.FirstOrDefault(x => x.SubItemTypeID == subTypeId);
            var outQty = stockOut.FirstOrDefault(x => x.SubItemTypeID == subTypeId);

            return new GetCountStockComparisonResponseDTO
            {
                itemcode = firstItem.Item.ItemCode,
                itemname = firstItem.Item.Name,
                subitemtypeid = subTypeId,
                subitemtypename = subType?.SubItemCode ?? subType?.SubTypeNameTH ?? "ไม่มีประเภทย่อย",
                cy_stockqty = grp.Sum(i => i.Qty),
                headpc_countedqty = headPcCount?.CountedQty,
                pc_countedqty = pcCount?.CountedQty ?? 0,
                salesqty = outQty?.TotalQty ?? 0,
                stockinqty = inQty?.TotalQty ?? 0,
                stockoutqty = outQty?.TotalQty ?? 0
            };
        }).ToList();

        return new BaseResponse<List<GetCountStockComparisonResponseDTO>>
        {
            result = true,
            data = result,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
