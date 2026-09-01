using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
public class InquiryCountStockByIDHandler : BaseService, IRequestHandler<InquiryCountStockByIDQuery, BaseResponse<InquiryCountStockByIDResponseDTO>>
{
    public InquiryCountStockByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<InquiryCountStockByIDResponseDTO>> Handle(InquiryCountStockByIDQuery request, CancellationToken cancellationToken)
    {
        var resCountStockEntities = (from a in await _unitOfWork.Repository<TTCountStock>().QueryAsync()
                                     join b in await _unitOfWork.Repository<TTCountStockDetail>().QueryAsync() on a.CountStockID equals b.CountStockID
                                     join item in await _unitOfWork.Repository<TMItem>().QueryAsync() on b.ItemID equals item.ItemID
                                     into jItem
                                     from i in jItem.DefaultIfEmpty()
                                     join subitem in await _unitOfWork.Repository<TMSubItemType>().QueryAsync() on b.SubItemTypeID equals subitem.SubItemTypeID
                                     into jSubitemType
                                     from c in jSubitemType.DefaultIfEmpty()
                                     join d in await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive) on a.BranchID equals d.BranchID
                                     where a.CountStockID == request.countstockid
                                     select new
                                     {
                                         countstockid = a.CountStockID,
                                         branchid = a.BranchID,
                                         itemid = b.ItemID ?? 0,
                                         itemcode = i != null ? i.ItemCode : string.Empty,
                                         itemname = i != null ? i.Name : string.Empty,
                                         branchname = d.BranchName,
                                         countstockdate = a.CreatedDate,
                                         createdby = a.CreatedBy,
                                         remark = a.Remark,
                                         totalcount = a.TotalCount,
                                         countstockdetailid = b.CountStockDetailID,
                                         subitemtypeid = b.SubItemTypeID,
                                         subitemcode = c != null ? c.SubItemCode : "ไม่มีประเภทย่อย",
                                         qtyinbranchofstockday = b.QtyInBranchOfCountStockDay,
                                         storestock = b.QtyInBranch,
                                         countedqty = b.CountedAmountQty,
                                         waitingtorestock = b.PendingReStockQty,
                                         damaged = b.DamagedQty,
                                         soldbeforecount = b.SaleBeforeCountQty,
                                         totalcounted = b.TotalCountQty,
                                         difference = b.ShortageSurplusQty,
                                         itemremark = b.ItemRemark
                                     }).AsQueryable();

        if (resCountStockEntities == null || (resCountStockEntities != null && !resCountStockEntities.Any()))
        {
            throw new Exception("ไม่พบข้อมูลรายการนับสต๊อก");
        }

        var resdata = resCountStockEntities.GroupBy(g => g.countstockid).Select(s => new InquiryCountStockByIDResponseDTO
        {
            countstockid = s.Key,
            branchid = s.FirstOrDefault().branchid,
            branchname = s.FirstOrDefault().branchname,
            countstockdate = s.FirstOrDefault().countstockdate,
            createdby = s.FirstOrDefault().createdby,
            remark = s.FirstOrDefault().remark,
            totalcount = s.FirstOrDefault().totalcount,
            detail = s.Select(d => new InquiryCountStockByIDDetail
            {
                countstockdetailid = d.countstockdetailid,
                branchid = d.branchid,
                itemid = d.itemid,
                itemcode = d.itemcode,
                itemname = d.itemname,
                subitemtypeid = d.subitemtypeid,
                subitemcode = d.subitemcode,
                qtyinbranchofstockday = d.qtyinbranchofstockday,
                storestock = d.storestock,
                countedqty = d.countedqty,
                waitingtorestock = d.waitingtorestock,
                damaged = d.damaged,
                soldbeforecount = d.soldbeforecount,
                totalcounted = d.totalcounted,
                difference = d.difference,
                itemremark = d.itemremark,
            }).ToList()
        }).FirstOrDefault();


        return new BaseResponse<InquiryCountStockByIDResponseDTO>
        {
            result = true,
            data = resdata,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}