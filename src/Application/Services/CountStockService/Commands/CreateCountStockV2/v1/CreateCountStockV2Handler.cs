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

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStockV2.v1;

public class CreateCountStockV2Handler : BaseService,
    IRequestHandler<CreateCountStockV2Command, BaseResponse<CommandResponse>>
{
    public CreateCountStockV2Handler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(
        CreateCountStockV2Command request, CancellationToken cancellationToken)
    {
        DateTime cDateTime = request.countstockdate;

        // 1.) Create CountStock Header + Detail (per item)
        TTCountStock countStockEnt = PrepareCountStock(request, cDateTime);
        await _unitOfWork.Repository<TTCountStock>().AddAsync(countStockEnt);

        // 2.) Snapshot TMItemInBranch into TTCountStocksHistory
        IEnumerable<TMItemInBranch> itemsInBranch = await _unitOfWork.Repository<TMItemInBranch>()
            .FindWithInclude(w => w.BranchID == request.branchid, i => i.Include(s => s.Item));

        if (!itemsInBranch.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้าในสาขาที่ทำรายการ กรุณาลองใหม่อีกครั้ง");
        }

        List<TTCountStocksHistory> histories = PrepareCountStocksHistory(itemsInBranch, request.createdby, cDateTime);
        await _unitOfWork.Repository<TTCountStocksHistory>().AddRangeAsync(histories);

        await _unitOfWork.SaveChangesAsync();

        return new BaseResponse<CommandResponse>
        {
            result  = true,
            data    = new CommandResponse { result = true },
            message = "Success",
            soruce  = "db",
            status  = StatusCodes.Status200OK.ToString()
        };
    }

    private TTCountStock PrepareCountStock(CreateCountStockV2Command cmd, DateTime cDateTime)
    {
        TTCountStock stockEnt = new TTCountStock
        {
            BranchID    = cmd.branchid,
            CountDate   = cmd.countstockdate,
            TotalCount  = cmd.totalcount,
            Remark      = cmd.remark,
            CreatedBy   = cmd.createdby,
            CreatedDate = cDateTime,
            IsActive    = true
        };
        stockEnt.TTCountStockDetails = PrepareCountStockDetails(cmd.detail, cmd.createdby, cDateTime);
        stockEnt.AddDomainEvent(new TTCountStockCreateEvent(stockEnt));
        return stockEnt;
    }

    private List<TTCountStockDetail> PrepareCountStockDetails(
        List<CreateCountStockV2Detail> details, string createdBy, DateTime cDateTime)
    {
        var result = details.Select(s => new TTCountStockDetail
        {
            SubItemTypeID               = s.subitemtypeid,
            QtyInBranchOfCountStockDay  = s.qtyinbranchofcountstockday,
            QtyInBranch                 = s.qtyinbranchofcountstockday,
            CountedAmountQty            = s.physicalcountqty,
            PendingReStockQty           = 0,
            DamagedQty                  = 0,
            SaleBeforeCountQty          = 0,
            TotalCountQty               = s.physicalcountqty,
            ShortageSurplusQty          = s.shortagesurplusqty, // อาจติดลบ — V2 อนุญาต
            CreatedBy                   = createdBy,
            CreatedDate                 = cDateTime,
            IsActive                    = true
        }).ToList();

        result.ForEach(ent => ent.AddDomainEvent(new TTCountStockDetailCreateEvent(ent)));
        return result;
    }

    private List<TTCountStocksHistory> PrepareCountStocksHistory(
        IEnumerable<TMItemInBranch> itemsInBranch, string createdBy, DateTime cDateTime)
    {
        var histories = itemsInBranch.Select(s => new TTCountStocksHistory
        {
            BranchID        = s.BranchID,
            ItemID          = s.ItemID,
            Price           = s.Price,
            DiscountPercent = s.DiscountPercent,
            Qty             = s.Qty,
            NotifyMinQty    = s.NotifyMinQty,
            NotifyMaxQty    = s.NotifyMaxQty,
            CreatedBy       = createdBy,
            CreatedDate     = cDateTime,
            IsActive        = true,
            WarehouseQty    = s.Item.Qty
        }).ToList();

        histories.ForEach(ent => ent.AddDomainEvent(new TTCountStocksHistoryCreateEvent(ent)));
        return histories;
    }
}
