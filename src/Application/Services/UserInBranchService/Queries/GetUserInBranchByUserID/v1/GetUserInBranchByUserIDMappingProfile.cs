using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;

public class GetUserInBranchByUserIDMappingProfile : Profile
{
	public GetUserInBranchByUserIDMappingProfile()
	{
		//CreateMap<TMUserInBranch, GetUserInBranchByUserIDResponseDTO>()
		//	.ForMember(w => w.userid, f => f.MapFrom(x => x.UserID))
		//	.ForMember(w => w.branchs, f => f.MapFrom(x => new GetUserInBranchByUserIDBrancResponseDTO
		//	{
		//		branchid = x.Branch.BranchID,
		//		branchcode = x.Branch.BranchCode,
		//		branchname = x.Branch.BranchName
		//	}));
		CreateMap<TMUserInBranch, GetUserInBranchByUserIDResponseDTO>()
			.ForMember(w => w.userid, f => f.MapFrom(x => x.UserID))
			.AfterMap((s, d) => d.branchs = new List<GetUserInBranchByUserIDBrancResponseDTO>
			{
				new GetUserInBranchByUserIDBrancResponseDTO
				{
					branchid = s.Branch.BranchID,
					branchcode = s.Branch.BranchCode,
					branchname = s.Branch.BranchName
				}
			});
	}
}
