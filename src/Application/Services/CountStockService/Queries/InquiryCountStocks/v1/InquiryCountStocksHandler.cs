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
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
public class InquiryCountStocksHandler : BaseService, IRequestHandler<InquiryCountStocksQuery, BaseResponse<List<InquiryCountStockResponseDTO>>>
{
    public InquiryCountStocksHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<InquiryCountStockResponseDTO>>> Handle(InquiryCountStocksQuery request, CancellationToken cancellationToken)
    {
        var resCountStockEntities = (from a in await _unitOfWork.Repository<TTCountStock>().QueryAsync(w => w.IsActive)
                                     join b in await _unitOfWork.Repository<TTCountStockDetail>().QueryAsync(w => w.IsActive) on a.CountStockID equals b.CountStockID
                                     join c in await _unitOfWork.Repository<TMSubItemType>().QueryAsync(w => w.IsActive) on b.SubItemTypeID equals c.SubItemTypeID
                                     join d in await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive) on a.BranchID equals d.BranchID
                                     select new InquiryCountStockResponseDTO
                                     {
                                         countstockid = a.CountStockID,
                                         countstockdate = a.CreatedDate,
                                         branchid = a.BranchID,
                                         branchname = d.BranchName,
                                         countstockdetailid = b.CountStockDetailID,
                                         subitemtypeid = b.SubItemTypeID,
                                         subitemtypename = c.SubTypeNameTH,
                                         qtyinbranch = b.QtyInBranch,
                                         qtyinbranchofcountstockday = b.QtyInBranchOfCountStockDay,
                                         countedamountqty = b.CountedAmountQty,
                                         pendingrestockqty = b.PendingReStockQty,
                                         damagedqty = b.DamagedQty,
                                         salebeforecountqty = b.SaleBeforeCountQty,
                                         totalcount = b.TotalCountQty,
                                         remark = a.Remark,
                                         createdby = a.CreatedBy,
                                         createddate = a.CreatedDate
                                     }).AsEnumerable();
        //var _resCountStockEntities = resCountStockEntities.ToList();
        if (!resCountStockEntities.Any())
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        //Group
        //var data = resCountStockEntities.GroupBy(g => new { g.branchid, g.subitemtypeid }).Select(s => new 
        //{
        //    branchid = s.Key.branchid,
        //    subitemtypeid = s.Key.subitemtypeid,
        //    data = s.Select(s => s).ToList()
        //}).ToList();

        //var _data = data;

        return new BaseResponse<List<InquiryCountStockResponseDTO>>
        {
            result = true,
            data = resCountStockEntities.OrderBy(s => s.countstockdate).ThenBy(o => o.branchid).ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
