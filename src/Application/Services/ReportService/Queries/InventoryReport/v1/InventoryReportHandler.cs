
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.ReportService.Queries.InventoryReport.v1;
public class InventoryReportHandler : BaseService, IRequestHandler<InventoryReportQuery, BaseResponse<List<InventoryReportResponseDTO>>>
{
    public InventoryReportHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<InventoryReportResponseDTO>>> Handle(InventoryReportQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<InventoryReportResponseDTO> resTotalSaleOfMonth = await GetInventoryDataBySearchTypeAsync(request);
        if (resTotalSaleOfMonth == null || !resTotalSaleOfMonth.Any())
        {
            throw new Exception("Data not found");
        }
        List<InventoryReportResponseDTO> res = (from a in resTotalSaleOfMonth
                                                group a by a.itemid into grps
                                                let item = grps.FirstOrDefault(w => w.itemid == grps.Key)
                                                let totalsale = grps.Sum(s => s.totalsale)
                                                select new InventoryReportResponseDTO
                                                {
                                                    itemid = grps.Key,
                                                    itemcode = item.itemcode,
                                                    itemname = item.itemname,
                                                    qtyinstock = item.qtyinstock,
                                                    notifymin = item.notifymin,
                                                    notifymax = item.notifymax,
                                                    totalsale = totalsale
                                                }).ToList();
        return new BaseResponse<List<InventoryReportResponseDTO>>
        {
            result = true,
            data = res,
            soruce = "db",
            message = "Success",
            status = StatusCodes.Status200OK.ToString()
        };
    }

    private async Task<IEnumerable<InventoryReportResponseDTO>> GetInventoryDataBySearchTypeAsync(InventoryReportQuery request)
    {
        if (request.searchtype == 1)
        {
            IEnumerable<InventoryReportResponseDTO> resTotalSaleOfMonth = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync(w => w.IsActive == true 
                                                                           && w.TransactionDate.Date == request.reportdate.Date)
                                                                           join trandetail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync(w => w.IsActive == true) on tran.TransactionID equals trandetail.TransactionID
                                                                           join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on trandetail.ItemID equals item.ItemID
                                                                           select new InventoryReportResponseDTO
                                                                           {
                                                                               itemid = trandetail.ItemID,
                                                                               itemcode = item.ItemCode,
                                                                               itemname = item.Name,
                                                                               qtyinstock = item.Qty,
                                                                               notifymin = item.NotifyMinQty,
                                                                               notifymax = item.NotifyMaxQty.HasValue ? item.NotifyMaxQty.Value : 0,
                                                                               totalsale = trandetail.Qty
                                                                           }).AsEnumerable();
            return resTotalSaleOfMonth;
        }
        else if (request.searchtype == 2)
        {
            IEnumerable<InventoryReportResponseDTO> resTotalSaleOfMonth = (from tran in await _unitOfWork.Repository<TTTransaction>().QueryAsync(w => w.IsActive == true 
                                                                           && w.TransactionDate.Month == request.reportdate.Month 
                                                                           && w.TransactionDate.Year == request.reportdate.Year)
                                                                           join trandetail in await _unitOfWork.Repository<TTTransactonDetail>().QueryAsync(w => w.IsActive == true) on tran.TransactionID equals trandetail.TransactionID
                                                                           join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on trandetail.ItemID equals item.ItemID
                                                                           select new InventoryReportResponseDTO
                                                                           {
                                                                               itemid = trandetail.ItemID,
                                                                               itemcode = item.ItemCode,
                                                                               itemname = item.Name,
                                                                               qtyinstock = item.Qty,
                                                                               notifymin = item.NotifyMinQty,
                                                                               notifymax = item.NotifyMaxQty.HasValue ? item.NotifyMaxQty.Value : 0,
                                                                               totalsale = trandetail.Qty
                                                                           }).AsEnumerable();
            return resTotalSaleOfMonth;
        }
        else
        {
            return default;
        }
        
    }
}
