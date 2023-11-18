using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockByBrachReport.v1;
public class AvailableStockByBrachReportHandler : BaseService, IRequestHandler<AvailableStockByBrachReportQuery, BaseResponse<List<AvailableStockReportResponseDTO>>>
{
    public AvailableStockByBrachReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<AvailableStockReportResponseDTO>>> Handle(AvailableStockByBrachReportQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMItemInBranch> resItemBranch = await _unitOfWork.Repository<TMItemInBranch>().FindWithInclude(w => w.BranchID == request.branchid && w.IsActive,
            i => i.Include(x => x.Branch),
            i => i.Include(x => x.Item),
            i => i.Include(x => x.Item.Brand),
            i => i.Include(x => x.Item.ItemType),
            i => i.Include(x => x.Item.UnitOfMeasure));
        if (!resItemBranch.Any())
        {
            throw new Exception("Data not found");
        }

        var resData = resItemBranch.GroupBy(x => x.BranchID).Select(s => new
        {
            branchid = s.Key,
            branchname = s.FirstOrDefault(w => w.BranchID == s.Key).Branch.BranchName,
            itemlist = (from x in s
                        where x.Qty <= x.Item.NotifyMinQty
                        select new AvailableStockReportResponseDTO
                        {
                            itemid = x.ItemID,
                            itemname = x.Item.Name,
                            itemcode = x.Item.ItemCode,
                            cost = x.Item.Cost,
                            brandid = x.Item.BrandID,
                            brandname = x.Item.Brand.BrandName,
                            price = x.Price,
                            qty = x.Qty,
                            itemtypeid = x.Item.ItemTypeID,
                            itemtypename = x.Item.ItemType.ItemTypeName,
                            barcode = x.Item.BarCode,
                            minqty = x.Item.NotifyMinQty,
                            maxqty = x.Item.NotifyMaxQty,
                            branchid = s.Key,
                            branchname = s.FirstOrDefault(w => w.BranchID == s.Key).Branch.BranchName
                        }).ToList()
        }).OrderBy(o => o.branchid).FirstOrDefault();

        if (resData.itemlist.Count() == 0)
        {
            throw new Exception("Data not found");
        }

        return new BaseResponse<List<AvailableStockReportResponseDTO>>
        {
            result = true,
            data = resData.itemlist.Select(w => w).OrderBy(o => o.branchid).ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
