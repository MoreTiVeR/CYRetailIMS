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
        IQueryable<InquiryCountStockByBranchIDResponseDTO> resCountStockData;
        resCountStockData = (from a in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.IsActive == true)
                             join b in await _unitOfWork.Repository<TMItemType>().QueryAsync() on a.Item.ItemTypeID equals b.ItemTypeID
                             join c in await _unitOfWork.Repository<TMSubItemType>().QueryAsync() on a.Item.SubItemTypeID equals c.SubItemTypeID
                             into jSubitemType
                             from s in jSubitemType.DefaultIfEmpty()
                             where a.BranchID == request.branchid
                             && a.IsActive == true
                             select new InquiryCountStockByBranchIDResponseDTO
                             {
                                 branchid = a.BranchID,
                                 itemid = a.ItemID,
                                 itemtypecode = b.ItemTypeName,
                                 subitemtypeid = s != null ? s.SubItemTypeID : 0,
                                 subitemcode = s != null ? s.SubItemCode : "N/A",
                                 qtyinbranchofstockday = a.Qty,
                                 storestock = 0,
                                 countedqty = 0,
                                 waitingtorestock = 0,
                                 damaged = 0,
                                 soldbeforecount = 0,
                                 totalcounted = 0,
                                 difference = 0,
                             }).AsQueryable();

        //if (request.branchid > 0)
        //{
        //    resCountStockData = resCountStockData.Where(w => w.branchid == request.branchid);
        //}

        if (resCountStockData == null || resCountStockData?.Count() == 0)
        {
            throw new Exception("ไม่พบข้อมูลสินค้าสาขาที่ต้องการนับสต๊อก");
        }

        var resFinalData = resCountStockData.AsEnumerable().GroupBy(g => g.subitemcode)
            .Select(s => new { s.Key, data = s })
            .Select(s => new InquiryCountStockByBranchIDResponseDTO
            {
                branchid = s.data.FirstOrDefault().branchid,
                itemtypecode = s.data.FirstOrDefault(w => w.subitemcode == s.Key).itemtypecode,
                subitemtypeid = s.data.FirstOrDefault(w => w.subitemcode == s.Key).subitemtypeid,
                subitemcode = s.Key,
                qtyinbranchofstockday = s.data.Sum(q => q.qtyinbranchofstockday),
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
