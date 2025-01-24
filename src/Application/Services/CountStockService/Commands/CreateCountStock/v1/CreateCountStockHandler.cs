using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
public class CreateCountStockHandler : BaseService, IRequestHandler<CreateCountStockCommand, BaseResponse<CommandResponse>>
{
    public CreateCountStockHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreateCountStockCommand request, CancellationToken cancellationToken)
    {
        DateTime cDateTime = DateTime.Now;

        #region 1.) Create CountStock Header & Detail
        TTCountStock countStockEnt = PreapreCountStock(request, cDateTime);
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

    private TTCountStock PreapreCountStock(CreateCountStockCommand stockCommand, DateTime cDateTime)
    {
        TTCountStock stockEnt = new TTCountStock
        {
            BranchID = stockCommand.branchid,
            CountDate = stockCommand.countstockdate,
            TotalCount = stockCommand.totalcount,
            Remark = stockCommand.remark,
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
        List<TTCountStockDetail> stockDetails = countStockDetails.Select(s => new TTCountStockDetail
        {
            SubItemTypeID = s.subitemtypeid,
            QtyInBranch = s.qtyinbranch,
            CountedAmountQty = s.countedamountqty,
            PendingReStockQty = s.pendingrestockqty,
            DamagedQty = s.damagedqty,
            SaleBeforeCountQty = s.salebeforecountqty,
            TotalCountQty = s.totalcountqty,
            ShortageSurplusQty = s.shortagesurplusqty,
            CreatedBy = createdBy,
            CreatedDate = cDateTime,
            IsActive= true
        }).ToList();
        stockDetails.ForEach(ent =>
        {
            ent.AddDomainEvent(new TTCountStockDetailCreateEvent(ent));
        });
        return stockDetails;
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
