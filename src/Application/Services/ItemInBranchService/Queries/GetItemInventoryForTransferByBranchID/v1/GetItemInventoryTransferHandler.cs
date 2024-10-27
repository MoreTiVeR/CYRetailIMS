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

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;

public class GetItemInventoryTransferHandler : BaseService, IRequestHandler<GetItemInventoryTransferQuery, BaseResponse<List<GetItemInventoryTransferResposeDTO>>>
{
    public GetItemInventoryTransferHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetItemInventoryTransferResposeDTO>>> Handle(GetItemInventoryTransferQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<GetItemInventoryTransferResposeDTO> res = (from itembranch in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.IsActive && w.BranchID == request.branchid)
                                                               join item in await _unitOfWork.Repository<TMItem>().QueryAsync(w => w.IsActive) on itembranch.ItemID equals item.ItemID
                                                               where itembranch.Qty < itembranch.NotifyMinQty
                                                               select new GetItemInventoryTransferResposeDTO
                                                               {
                                                                   branchid = itembranch.BranchID,
                                                                   branchname = itembranch.Branch.BranchName,
                                                                   itemid = itembranch.ItemID,
                                                                   itemcode = item.ItemCode,
                                                                   itemname = item.Name,
                                                                   brandid = item.BrandID,
                                                                   brandname = item.Brand.BrandName,
                                                                   qtyinstock = item.Qty,
                                                                   qtyinbranch = itembranch.Qty,
                                                                   notifyminqty = itembranch.NotifyMinQty.HasValue ? itembranch.NotifyMinQty.Value : 0
                                                               });
        if (request.brandid.HasValue && request.brandid.Value > 0)
        {
            res = res.Where(w => w.brandid == request.brandid.Value);
        }

        #region Re-Assign OrderQTY(จำนวนสินค้าที่ต้องเติม) and RefillQTY(จำนวนที่เติมจริง)
        res = res.ToList().Select(e =>
        {
            int numQty = e.notifyminqty - e.qtyinbranch;
            e.orderqty = numQty < 0 ? 0 : numQty;
            e.refillqty = e.orderqty;
            return e;
        });
        #endregion

        if (!res.Any())
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<List<GetItemInventoryTransferResposeDTO>>
        {
            result = true,
            data = res.ToList(),
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
