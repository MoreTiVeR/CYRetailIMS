using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
public class InquiryCountStockByBranchIDHandler : BaseService, IRequestHandler<InquiryCountStockByBranchIDQuery, BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>>
{
    public InquiryCountStockByBranchIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>> Handle(InquiryCountStockByBranchIDQuery request, CancellationToken cancellationToken)
    {
        var itemInBranchQuery = (await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.IsActive == true))
            ?? Enumerable.Empty<TMItemInBranch>().AsQueryable();
        var itemTypeQuery = (await _unitOfWork.Repository<TMItemType>().QueryAsync())
            ?? Enumerable.Empty<TMItemType>().AsQueryable();
        var subItemTypeQuery = (await _unitOfWork.Repository<TMSubItemType>().QueryAsync())
            ?? Enumerable.Empty<TMSubItemType>().AsQueryable();

        var resCountStockData = (from a in itemInBranchQuery
                                 join b in itemTypeQuery on a.Item.ItemTypeID equals b.ItemTypeID
                                 join c in subItemTypeQuery on a.Item.SubItemTypeID equals c.SubItemTypeID
                                 into jSubitemType
                                 from s in jSubitemType.DefaultIfEmpty()
                                 where a.BranchID == request.branchid
                                 && a.IsActive == true
                                 select new InquiryCountStockByBranchIDResponseDTO
                                 {
                                     branchid = a.BranchID,
                                     itemid = a.ItemID,
                                     itemcode = a.Item.ItemCode,
                                     itemname = a.Item.Name,
                                     itemtypecode = b.ItemTypeName,
                                     // Use item's own subtype id as source of truth.
                                     // This prevents losing valid subtype id when TMSubItemType row is missing/inactive.
                                     subitemtypeid = a.Item.SubItemTypeID ?? 0,
                                     subitemcode = s != null ? s.SubItemCode : "ไม่มีประเภทย่อย",
                                     qtyinbranchofstockday = a.Qty,
                                     storestock = 0,
                                     countedqty = 0,
                                     waitingtorestock = 0,
                                     damaged = 0,
                                     soldbeforecount = 0,
                                     totalcounted = 0,
                                     difference = 0,
                                 }).ToList();

        if (!resCountStockData.Any())
        {
            throw new Exception("ไม่พบข้อมูลสินค้าสาขาที่ต้องการนับสต๊อก");
        }

        // หน้านับสต๊อกแบบใหม่: ต้องการข้อมูลระดับรายสินค้า (รหัสสินค้า/ชื่อสินค้า)
        // จึงไม่รวมกลุ่มตามประเภทย่อย และเรียงตามประเภทย่อย -> รหัสสินค้า
        if (request.itemlevel)
        {
            var itemLevelData = resCountStockData
                .OrderBy(o => o.subitemcode)
                .ThenBy(o => o.itemcode)
                .ToList();
            return new BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>
            {
                result = true,
                data = itemLevelData,
                message = "Success",
                soruce = "db",
                status = StatusCodes.Status200OK.ToString()
            };
        }

        var resFinalData = resCountStockData.GroupBy(g => g.subitemcode)
            .Select(s => new InquiryCountStockByBranchIDResponseDTO
            {
            branchid = s.First().branchid,
            itemtypecode = s.First().itemtypecode,
            subitemtypeid = s.First().subitemtypeid,
            subitemcode = s.Key ?? "ไม่มีประเภทย่อย",
            qtyinbranchofstockday = s.Sum(q => q.qtyinbranchofstockday),
                storestock = 0,
                countedqty = 0,
                waitingtorestock = 0,
                damaged = 0,
                soldbeforecount = 0,
                totalcounted = 0,
                difference = 0,
            }).ToList();
        return new BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>
        {
            result = true,
            data = resFinalData,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
