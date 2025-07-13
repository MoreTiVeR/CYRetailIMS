using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemStockReport.v1;
public class ItemStockReportHandler : BaseService, IRequestHandler<ItemStockReportQuery, BaseResponse<ItemStockReportResponseDTO>>
{
    public ItemStockReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<ItemStockReportResponseDTO>> Handle(ItemStockReportQuery request, CancellationToken cancellationToken)
    {
        var resData = request.branchid.HasValue && request.branchid.Value > 1 ? await GetItemStockBranchAsync(request) : await GetItemStockWarehouseAsync(request);
        
        return new BaseResponse<ItemStockReportResponseDTO>
        {
            result = true,
            data = resData,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }

    private async Task<ItemStockReportResponseDTO> GetItemStockWarehouseAsync(ItemStockReportQuery request)
    {
        int totalRow = 0;

        var resData = (from a in await _unitOfWork.Repository<TMItem>().QueryAsync()
                       select new ItemStockReportDetailDTO
                       {
                           branchid = 1,
                           branchname = "สำนักงานใหญ่",
                           itemname = a.Name,
                           itemcode = a.ItemCode,
                           subitemtypeid = a.SubItemTypeID.HasValue ? a.SubItemTypeID : null,
                           subitemtypename = a.SubItemType != null ? a.SubItemType.SubTypeNameTH : null,
                           cost = a.Cost,
                           price = a.Price,
                           brandid = a.BrandID,
                           brandname = a.Brand.BrandName,
                           qty = a.Qty,
                           itemtypeid = a.ItemTypeID,
                           itemtypename = a.ItemType.ItemTypeName,
                           notifyminqty = a.NotifyMinQty,
                           notifymaxqty = a.NotifyMaxQty,
                           isactive = a.IsActive
                       });

        if(request.itemtypeid.HasValue && request.itemtypeid.Value > 0)
        {
            resData = resData.Where(w => w.itemtypeid == request.itemtypeid.Value);
        }

        if(request.subitemtypeid.HasValue && request.subitemtypeid.Value > 0)
        {
            resData = resData.Where(w => w.subitemtypeid == request.subitemtypeid.Value);
        }

        //if(request.searchvalue != null && request.searchvalue.Trim().Length > 0)
        //{
        //    resData = resData.Where(w => w.itemname.Contains(request.searchvalue) || w.itemcode.Contains(request.searchvalue));
        //}

        if(request.brandid.HasValue && request.brandid.Value > 0)
        {
            resData = resData.Where(w => w.brandid == request.brandid.Value);
        }   

        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูลสต๊อกสินค้า");
        }

        //Final Data
        List<ItemStockReportDetailDTO> resItemStock = await resData.ToListAsync();

        //Addign total row
        totalRow = resItemStock.Count();

        //Assign data
        if (!request.isexportalldata)
        {
            resItemStock = resItemStock.Skip(request.startrow).Take(request.pagesize).ToList();
        }

        return new ItemStockReportResponseDTO
        {
            totalrow = totalRow,
            data = resItemStock
        };
    }

    private async Task<ItemStockReportResponseDTO> GetItemStockBranchAsync(ItemStockReportQuery request)
    {
        int totalRow = 0;
        var resData = (from a in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync()
                       join b in await _unitOfWork.Repository<TMItem>().QueryAsync() on a.ItemID equals b.ItemID
                       where a.BranchID == request.branchid && a.IsActive
                       select new ItemStockReportDetailDTO
                       {
                           branchid = a.BranchID,
                           branchname = a.Branch.BranchName,
                           itemname = b.Name,
                           itemcode = b.ItemCode,
                           subitemtypeid = b.SubItemTypeID.HasValue ? b.SubItemTypeID : null,
                           subitemtypename = b.SubItemType != null ? b.SubItemType.SubTypeNameTH : null,
                           cost = b.Cost,
                           price = a.Price,
                           brandid = b.BrandID,
                           brandname = b.Brand.BrandName,
                           qty = a.Qty,
                           itemtypeid = b.ItemTypeID,
                           itemtypename = b.ItemType.ItemTypeName,
                           notifyminqty = a.NotifyMinQty.HasValue ? a.NotifyMinQty.Value : 0,
                           notifymaxqty = a.NotifyMaxQty.HasValue ? a.NotifyMaxQty.Value : 0,
                           isactive = a.IsActive
                       });

        if (request.itemtypeid.HasValue && request.itemtypeid.Value > 0)
        {
            resData = resData.Where(w => w.itemtypeid == request.itemtypeid.Value);
        }

        if (request.subitemtypeid.HasValue && request.subitemtypeid.Value > 0)
        {
            resData = resData.Where(w => w.subitemtypeid == request.subitemtypeid.Value);
        }

        //if (request.searchvalue != null && request.searchvalue.Trim().Length > 0)
        //{
        //    resData = resData.Where(w => w.itemname.Contains(request.searchvalue) || w.itemcode.Contains(request.searchvalue));
        //}

        if (request.brandid.HasValue && request.brandid.Value > 0)
        {
            resData = resData.Where(w => w.brandid == request.brandid.Value);
        }

        if (!resData.Any())
        {
            throw new Exception("ไม่พบข้อมูลสต๊อกสินค้า");
        }

        //Final data
        List<ItemStockReportDetailDTO> resItemStock = await resData.ToListAsync();

        //Addign total row
        totalRow = resItemStock.Count();

        //Assign data
        if (!request.isexportalldata)
        {
            resItemStock = resItemStock.Skip(request.startrow).Take(request.pagesize).ToList();
        }

        return new ItemStockReportResponseDTO
        {
            totalrow = totalRow,
            data = resItemStock
        };
    }
}
