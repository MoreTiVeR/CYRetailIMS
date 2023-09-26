using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ApproveStatusService.Queries.GetApproveStatus.v1;
public class GetApproveStatusMappingProfile : Profile
{
	public GetApproveStatusMappingProfile()
	{
		CreateMap<TMApproveStatus, GetApproveStatusResponseDTO>()
			.ForMember(w => w.approvestatusid, f => f.MapFrom(w => w.ApproveStatusID))
			.ForMember(w => w.approvestatusname_th, f => f.MapFrom(w => w.ApproveStatusName_TH))
			.ForMember(w => w.approvestatusname_en, f => f.MapFrom(w => w.ApproveStatusName_EN))
			.ForMember(w => w.description, f => f.MapFrom(w => w.Description))
			.ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
			.ForMember(w => w.creadeddate, f => f.MapFrom(w => w.CreadedDate))
			.ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));
	}
}
