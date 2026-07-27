using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
public class GetItemInBranchByBranchIDHandler : BaseService, IRequestHandler<GetItemInBranchByBranchIDQuery, BaseResponse<GetItemInBranchByBranchIDResponseDTO>>
{
    public GetItemInBranchByBranchIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetItemInBranchByBranchIDResponseDTO>> Handle(GetItemInBranchByBranchIDQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMItemInBranch> resItemBranch = await _unitOfWork.Repository<TMItemInBranch>().FindWithInclude(w => w.BranchID == request.branchid,
            i => i.Include(x => x.Branch),
            i => i.Include(x => x.Item),
            i => i.Include(x => x.Item.Brand),
            i => i.Include(x => x.Item.ItemType),
            i => i.Include(x => x.Item.UnitOfMeasure),
            i => i.Include(x => x.Item.SubItemType));
        if (!resItemBranch.Any())
        {
            throw new Exception("Data not found");
        }

        GetItemInBranchByBranchIDResponseDTO resData = resItemBranch.GroupBy(x => x.BranchID).Select(s => new GetItemInBranchByBranchIDResponseDTO
        {
            branchid = s.Key,
            branchname = s.FirstOrDefault(w => w.BranchID == s.Key).Branch.BranchName,
            itemlist = (from x in s
                        select new GetItemInBranchByBranchIDItemResponseDTO
                        {
                            itemid = x.ItemID,
                            itemname = x.Item.Name,
                            itemcode = x.Item.ItemCode,
                            subitemtypeid = x.Item.SubItemTypeID,
                            subitemtypename = x.Item?.SubItemType?.SubTypeNameTH,
                            cost = x.Item.Cost,
                            brandid = x.Item.BrandID,
                            brandname = x.Item.Brand.BrandName,
                            brandshortname = x.Item.Brand.BrandShortName,
                            price = x.Price,
                            discountpercent = x.DiscountPercent,
                            qty = x.Qty,
                            description = x.Item.Description,
                            itemtypeid = x.Item.ItemTypeID,
                            itemtypename = x.Item.ItemType.ItemTypeName,
                            isactive = x.IsActive,
                            itemimageurl = x.Item.ItemImageUrl,
                            shortname = x.Item.ShortName,
                            unitofmeasureid = x.Item.UnitOfMeasure.UnitOfMeasureID,
                            unitofmeasurename = x.Item.UnitOfMeasure.UnitOfMeasureName,
                            createdby = x.CreatedBy,
                            createddate = x.CreatedDate,
                            updatedby = x.UpdatedBy,
                            updateddate = x.UpdatedDate,
                            barcode = x.Item.BarCode,
                            //notifyminqty = x.Item.NotifyMinQty,
                            //notifymaxqty = x.Item.NotifyMaxQty
                            notifyminqty = x.NotifyMinQty,
                            notifymaxqty = x.NotifyMaxQty
                        }).ToList()
        }).OrderBy(o => o.branchid).FirstOrDefault();

        if (resData == null)
        {
            throw new Exception("Data not found");
        }

        #region Update updatedby data from emp name
        List<string> userNameList = resData.itemlist.Select(s => s.createdby).Union(resData.itemlist.Select(s => s.updatedby)).Distinct().ToList();
        IEnumerable<TMUsers> userList = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => userNameList.Contains(w.UserName), i => i.Include(w => w.TMEmployees));
        var empDataList = userList.Select(s => new { s.UserName, s.TMEmployees.FirstOrDefault().FirstName }).ToList();
        resData.itemlist = resData.itemlist.Select(s =>
        {
            if (!string.IsNullOrEmpty(s.createdby))
            {
                s.createdby = empDataList.FirstOrDefault(w => w.UserName == s.createdby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.createdby).FirstName : s.createdby;
            }

            if (!string.IsNullOrEmpty(s.updatedby))
            {
                s.updatedby = empDataList.FirstOrDefault(w => w.UserName == s.updatedby) != null
                ? empDataList.FirstOrDefault(w => w.UserName == s.updatedby).FirstName : s.updatedby;
            }
            return s;
        }).ToList();
        #endregion

        return new BaseResponse<GetItemInBranchByBranchIDResponseDTO>
        {
            result = true,
            data = resData,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
