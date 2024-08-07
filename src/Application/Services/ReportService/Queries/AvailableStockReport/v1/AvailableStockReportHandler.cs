using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;
public class AvailableStockReportHandler : BaseService, IRequestHandler<AvailableStockReportQuery, BaseResponse<List<AvailableStockReportResponseDTO>>>
{
    public AvailableStockReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    /// <summary>
    /// รายงานสต๊อกขั้นต่ำ warehouse
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<BaseResponse<List<AvailableStockReportResponseDTO>>> Handle(AvailableStockReportQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<AvailableStockReportResponseDTO> resItems = (from a in await _unitOfWork.Repository<TMItem>().QueryAsync(w => w.IsActive)
                                                                 join b in await _unitOfWork.Repository<TMItemType>().QueryAsync(w => w.IsActive) on a.ItemTypeID equals b.ItemTypeID
                                                                 join c in await _unitOfWork.Repository<TMItemBrand>().QueryAsync(w => w.IsActive) on a.BrandID equals c.BrandID
                                                                 where a.Qty <= a.NotifyMinQty
                                                                 select new AvailableStockReportResponseDTO
                                                                 {
                                                                     itemid = a.ItemID,
                                                                     itemname = a.Name,
                                                                     itemcode = a.ItemCode,
                                                                     cost = a.Cost,
                                                                     brandid = a.BrandID,
                                                                     brandname = c.BrandName,
                                                                     price = a.Price,
                                                                     qty = a.Qty,
                                                                     itemtypeid = a.ItemTypeID,
                                                                     itemtypename = b.ItemTypeName,
                                                                     barcode = a.BarCode,
                                                                     minqty = a.NotifyMinQty,
                                                                     maxqty = a.NotifyMaxQty,
                                                                     branchid = 1,
                                                                     branchname = "คลังสำนักงานใหญ่"
                                                                 }).AsEnumerable();
        if (!resItems.Any())
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<List<AvailableStockReportResponseDTO>>
        {
            result = true,
            data = resItems.ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };

    }
}
