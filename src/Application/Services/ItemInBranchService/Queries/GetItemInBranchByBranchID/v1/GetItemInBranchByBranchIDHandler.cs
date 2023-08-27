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
			ii => ii.Include(xx => xx.Item),
			iii => iii.Include(xxx => xxx.Item.Brand));
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
							qty = x.Qty
						}).ToList()
		}).OrderBy(o => o.branchid).FirstOrDefault();

		//IEnumerable<GetItemInBranchByBranchIDResponseDTO> resItemBranch = (from a in await _unitOfWork.Repository<TMItemInBranch>().QueryAsync()
		//																   where a.BranchID == request.branchid && a.IsActive
		//																   group a by new { a.BranchID } into grps
		//																   select new GetItemInBranchByBranchIDResponseDTO
		//																   {
		//																	   branchid = grps.Key.BranchID,
		//																	   branchname = grps.FirstOrDefault(w => w.BranchID == grps.Key.BranchID).Branch.BranchName,
		//																	   itemlist = grps.Where(w => w.BranchID == grps.Key.BranchID).Select(s => new GetItemInBranchByBranchIDItemResponseDTO
		//																	   {
		//																		   itemid = s.ItemID,
		//																		   itemcode = s.Item.ItemCode,
		//																		   itemname = s.Item.Name,
		//																		   brandname = s.Item.Brand.BrandName,
		//																		   brandshortname = s.Item.Brand.BrandShortName,
		//																		   price = s.Price,
		//																		   discountpercent = s.DiscountPercent,
		//																		   qty = s.Qty
		//																	   }).OrderBy(o => o.itemid).ToList()
		//																   }).ToList();

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
