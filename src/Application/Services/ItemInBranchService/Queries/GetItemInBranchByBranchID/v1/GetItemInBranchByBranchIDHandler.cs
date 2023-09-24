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
		IEnumerable<TMItemInBranch> resItemBranch = await _unitOfWork.Repository<TMItemInBranch>().FindWithInclude(w => w.BranchID == request.branchid && w.IsActive,
			i => i.Include(x => x.Branch),
            i => i.Include(x => x.Item),
            i => i.Include(x => x.Item.Brand),
            i => i.Include(x => x.Item.ItemType));
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
							brandname = x.Item.Brand.BrandName,
							brandshortname = x.Item.Brand.BrandShortName,
							price = x.Price,
							discountpercent = x.DiscountPercent,
							qty = x.Qty,
							description = x.Item.Description,
							itemtypeid = x.Item.ItemTypeID,
							itemtypename = x.Item.ItemType.ItemTypeName,
							isactive = x.IsActive
						}).ToList()
		}).OrderBy(o => o.branchid).FirstOrDefault();

		if (!resItemBranch.Any())
		{
			throw new Exception("Data not found");
		}
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
