using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
public class GetBranchByIDMappingProfile : Profile
{
	public GetBranchByIDMappingProfile()
	{
		//CreateMap<TMBranchDetail, GetBranchByIDResponseDTO>()
		//	.ForMember(w => w.address1, f => f.MapFrom(x => x.Address1))
		//	.ForMember(w => w.address2, f => f.MapFrom(x => x.Address2))
		//	.ForMember(w => w.subdistrictcode, f => f.MapFrom(x => x.SubDistrictCode))
		//	.ForMember(w => w.districtcode, f => f.MapFrom(x => x.DistrictCode))
		//	.ForMember(w => w.provincecode, f => f.MapFrom(x => x.ProvinceCode))
		//	.ForMember(w => w.zipcode, f => f.MapFrom(x => x.ZipCode));

		CreateMap<TMBranch, GetBranchByIDResponseDTO>()
			.ForMember(w => w.branchid, f => f.MapFrom(x => x.BranchID))
			.ForMember(w => w.branchcode, f => f.MapFrom(x => x.BranchCode))
			.ForMember(w => w.branchname, f => f.MapFrom(x => x.BranchName))
			.ForMember(w => w.address1, f => f.MapFrom(x => x.TMBranchDetail.Address1))
			.ForMember(w => w.address2, f => f.MapFrom(x => x.TMBranchDetail.Address2))
			.ForMember(w => w.subdistrictcode, f => f.MapFrom(x => x.TMBranchDetail.SubDistrictCode))
			.ForMember(w => w.districtcode, f => f.MapFrom(x => x.TMBranchDetail.DistrictCode))
			.ForMember(w => w.provincecode, f => f.MapFrom(x => x.TMBranchDetail.ProvinceCode))
			.ForMember(w => w.zipcode, f => f.MapFrom(x => x.TMBranchDetail.ZipCode));
	}
}
