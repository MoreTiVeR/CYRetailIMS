using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
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
                    i => i.Include(x => x.Item.ItemType));
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
                        brandname = x.Item.Brand.BrandName,
                        brandshortname = x.Item.Brand.BrandShortName,
                        price = x.Price,
                        discountpercent = x.DiscountPercent,
                        qty = x.Qty,
                        description = x.Item.Description,
                        itemtypeid = x.Item.ItemTypeID,
                        itemtypename = x.Item.ItemType.ItemTypeName
                    }).FirstOrDefault()
        }).OrderBy(o => o.branchid).FirstOrDefault();

        if (!resItemBranch.Any())
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
