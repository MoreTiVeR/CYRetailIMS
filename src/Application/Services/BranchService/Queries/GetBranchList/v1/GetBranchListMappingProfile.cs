using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
public class GetBranchListMappingProfile : Profile
{
	public GetBranchListMappingProfile()
	{
		CreateMap<TMBranch, GetBranchListResponseDTO>()
			.ForMember(w => w.branchid, f => f.MapFrom(x => x.BranchID))
			.ForMember(w => w.branchcode, f => f.MapFrom(x => x.BranchCode))
			.ForMember(w => w.branchname, f => f.MapFrom(x => x.BranchName))
			.ForMember(w => w.address1, f => f.MapFrom(x => x.TMBranchDetail.Address1))
			.ForMember(w => w.address2, f => f.MapFrom(x => x.TMBranchDetail.Address2))
			.ForMember(w => w.subdistrictid, f => f.MapFrom(x => x.TMBranchDetail.SubDistrictID))
			.ForMember(w => w.districtid, f => f.MapFrom(x => x.TMBranchDetail.DistrictID))
			.ForMember(w => w.provinceid, f => f.MapFrom(x => x.TMBranchDetail.ProvinceID))
			.ForMember(w => w.zipcode, f => f.MapFrom(x => x.TMBranchDetail.ZipCode));
	}
}
