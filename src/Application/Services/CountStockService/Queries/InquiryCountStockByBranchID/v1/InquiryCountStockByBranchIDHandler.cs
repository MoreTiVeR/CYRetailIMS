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
                             select new InquiryCountStockByBranchIDResponseDTO
                             {
                                 branchid = a.BranchID,
                                 itemid = a.ItemID,
                                 itemtypecode = a.Item.ItemCode,
                                 subitemcode = s != null ? s.SubItemCode : "N/A",
                                 storestock = a.Qty,
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

        return new BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>
        {
            result = true,
            data = resCountStockData.ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
