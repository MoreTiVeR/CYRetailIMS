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

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;

public class GetItemInBranchByBranchListHandler : BaseService, IRequestHandler<GetItemInBranchByBranchListQuery, BaseResponse<List<GetItemInBranchByBranchListResponseDTO>>>
{
	public GetItemInBranchByBranchListHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	public async Task<BaseResponse<List<GetItemInBranchByBranchListResponseDTO>>> Handle(GetItemInBranchByBranchListQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<TMItemInBranch> resItemBranch = await _unitOfWork.Repository<TMItemInBranch>().FindWithInclude(w => request.branchid_list.Contains(w.BranchID) && w.IsActive,
			i => i.Include(x => x.Branch),
			ii => ii.Include(xx => xx.Item),
			iii => iii.Include(xxx => xxx.Item.Brand));
		if (!resItemBranch.Any())
		{
			throw new Exception("Data not found");
		}

		List<GetItemInBranchByBranchListResponseDTO> resData = resItemBranch.GroupBy(x => x.BranchID).Select(s => new GetItemInBranchByBranchListResponseDTO
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
							qty = x.Qty
						}).ToList()
		}).OrderBy(o => o.branchid).ToList();

		if (!resItemBranch.Any())
		{
			throw new Exception("Data not found");
		}
		return new BaseResponse<List<GetItemInBranchByBranchListResponseDTO>>
		{
			result = true,
			data = resData,
			message = "Success",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
