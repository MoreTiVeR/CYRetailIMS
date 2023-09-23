using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByCriteria.v1;
public class GetItemInBranchByCriteriaHandler : BaseService, IRequestHandler<GetItemInBranchByCriteriaQuery, BaseResponse<GetItemInBranchByCriteriaResponseDTO>>
{
    public GetItemInBranchByCriteriaHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetItemInBranchByCriteriaResponseDTO>> Handle(GetItemInBranchByCriteriaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TMItemInBranch> resItemBranch = await _unitOfWork.Repository<TMItemInBranch>().FindWithInclude(w =>
        w.BranchID == request.branchid && w.ItemID == request.itemid && w.IsActive,
                    i => i.Include(x => x.Branch),
                    i => i.Include(x => x.Item),
                    i => i.Include(x => x.Item.Brand),
                    i => i.Include(x => x.Item.ItemType),
                    i => i.Include(x => x.Item.UnitOfMeasure));

        //GetItemInBranchByCriteriaResponseDTO? resItemBranch2 = (from itembranch in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync()
        //                                                        join branch in await _unitOfWork.Repository<TMBranch>().QueryAsync(w => w.IsActive) on itembranch.BranchID equals branch.BranchID
        //                                                        join item in await _unitOfWork.Repository<TMItem>().QueryAsync(w => w.IsActive) on itembranch.ItemID equals item.ItemID
        //                                                        join itemtype in await _unitOfWork.Repository<TMItemType>().QueryAsync(w => w.IsActive) on item.ItemTypeID equals itemtype.ItemTypeID
        //                                                        join itembrand in await _unitOfWork.Repository<TMItemBrand>().QueryAsync(w => w.IsActive) on item.BrandID equals itembrand.BrandID
        //                                                        join itemmou in await _unitOfWork.Repository<TMUnitOfMeasure>().QueryAsync(w => w.IsActive) on item.UnitOfMeasureID equals itemmou.UnitOfMeasureID
        //                                                        where itembranch.ItemID == request.itemid
        //                                                        && itembranch.IsActive
        //                                                        && itembranch.BranchID == request.branchid
        //                                                        select new GetItemInBranchByCriteriaResponseDTO
        //                                                        {
        //                                                            branchid = itembranch.BranchID,
        //                                                            branchname = branch.BranchName,
        //                                                            item = new GetItemInBranchByBranchIDItemResponseDTO
        //                                                            {
        //                                                                itemid = itembranch.ItemID,
        //                                                                itemname = item.Name,
        //                                                                itemcode = item.ItemCode,
        //                                                                cost = item.Cost,
        //                                                                brandid = itembrand.BrandID,
        //                                                                brandname = itembrand.BrandName,
        //                                                                brandshortname = itembrand.BrandShortName,
        //                                                                price = itembranch.Price,
        //                                                                discountpercent = itembranch.DiscountPercent,
        //                                                                qty = itembranch.Qty,
        //                                                                description = item.Description,
        //                                                                itemtypeid = itemtype.ItemTypeID,
        //                                                                itemtypename = itemtype.ItemTypeName,
        //                                                                isactive = itembranch.IsActive,
        //                                                                itemimageurl = item.ItemImageUrl,
        //                                                                shortname = item.ShortName,
        //                                                                unitofmeasureid = itemmou.UnitOfMeasureID,
        //                                                                unitofmeasurename = itemmou.UnitOfMeasureName,
        //                                                            }
        //                                                        }).FirstOrDefault();

        if (!resItemBranch.Any())
        {
            throw new Exception("Data not found");
        }

        GetItemInBranchByCriteriaResponseDTO resData = resItemBranch.GroupBy(x => x.BranchID).Select(s => new GetItemInBranchByCriteriaResponseDTO
        {
            branchid = s.Key,
            branchname = s.FirstOrDefault(w => w.BranchID == s.Key).Branch.BranchName,
            item = (from x in s
                    select new GetItemInBranchByBranchIDItemResponseDTO
                    {
                        itemid = x.ItemID,
                        itemname = x.Item.Name,
                        itemcode = x.Item.ItemCode,
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
                        unitofmeasurename = x.Item.UnitOfMeasure.UnitOfMeasureName
                    }).FirstOrDefault()
        }).OrderBy(o => o.branchid).FirstOrDefault();

        if (resData == null)
        {
            throw new Exception("Data not found");
        }
        return new BaseResponse<GetItemInBranchByCriteriaResponseDTO>
        {
            result = true,
            data = resData,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
