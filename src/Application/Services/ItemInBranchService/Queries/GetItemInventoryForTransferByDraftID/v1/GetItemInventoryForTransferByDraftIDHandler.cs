using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByDraftID.v1;

public class GetItemInventoryForTransferByDraftIDHandler : BaseService, IRequestHandler<GetItemInventoryForTransferByDraftIDQuery, BaseResponse<List<GetItemInventoryTransferResposeDTO>>>
{
    public GetItemInventoryForTransferByDraftIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetItemInventoryTransferResposeDTO>>> Handle(GetItemInventoryForTransferByDraftIDQuery request, CancellationToken cancellationToken)
    {
        #region Step.1) Get Draft transfer data by drafyid
        IEnumerable<TTDraftItemTransfer> resDraftHeader = await _unitOfWork.Repository<TTDraftItemTransfer>().FindWithInclude(w => w.TransferHeaderID == request.draftid, i => i.Include(s => s.TTDraftItemTransferDetails));
        if (!resDraftHeader.Any())
        {
            throw new Exception("ไม่พบข้อมูลฉบับร่าง");
        }
        int branchID = resDraftHeader.FirstOrDefault().DestinationBranchID;
        #endregion

        #region Step.2)  Get ข้อมูล item ที่สามารถทำโอนได้ จำนวนชิ้นที่เหลือในสาขา < จำนวนขั้นต่ำที่กำหนด ของสาขา
        IEnumerable<GetItemInventoryTransferResposeDTO> res = (from itembranch in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync(w => w.IsActive && w.BranchID == branchID)
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

        if (!res.Any())
        {
            throw new Exception("Data not found");
        }
        #endregion

        #region Prepare จำนวน QTY ที่เคย draft ไว้ จาก Step.1 โดยเอา itemid มาเทียบ
        //var source = resDraftHeader.SelectMany(s => s.TTDraftItemTransferDetails).ToList();
        //var target = res.ToList();
        //List<GetItemInventoryTransferResposeDTO> updatedTargetList = target
        //    .Join(source,
        //        target => target.itemid,   // Key from targetList
        //        source => source.ItemID,   // Key from sourceList
        //        (target, source) =>   // Result selector
        //        {
        //            target.refillqty = source.Qty; // Assign value
        //            return target;  // Return updated target
        //        }).ToList();

        IEnumerable<TTDraftItemTransferDetail> draftItems = resDraftHeader.SelectMany(s => s.TTDraftItemTransferDetails).ToList();
        //foreach (var target in res)
        //{
        //    var item = draftItems.FirstOrDefault(s => s.ItemID == target.itemid);
        //    if (item != null)
        //    {
        //        target.refillqty = item.Qty;
        //    }
        //}

        res = res.ToList().Select(e =>
        {
            //Update orderqty
            int numQty = e.notifyminqty - e.qtyinbranch;
            e.orderqty = numQty < 0 ? 0 : numQty;

            //Update refillqty base on draft
            var item = draftItems.FirstOrDefault(s => s.ItemID == e.itemid);
            if(item != null)
            {
                e.refillqty = item.Qty;
            }
            else
            {
                e.refillqty = e.orderqty;
            }
            return e;
        });
        #endregion

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
