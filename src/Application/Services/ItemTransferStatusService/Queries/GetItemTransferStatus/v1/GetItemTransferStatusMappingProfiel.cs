using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;
public class GetItemTransferStatusMappingProfiel : Profile
{
	public GetItemTransferStatusMappingProfiel()
	{
		CreateMap<TMItemTransferStatus, GetItemTransferStatusResponseDTO>()
			.ForMember(w => w.transferstatusid, f => f.MapFrom(w => w.TransferStatusID))
			.ForMember(w => w.transferstatusname_th, f => f.MapFrom(w => w.TransferStatusName_TH))
			.ForMember(w => w.transferstatusname_en, f => f.MapFrom(w => w.TransferStatusName_EN))
			.ForMember(w => w.description, f => f.MapFrom(w => w.Description))
			.ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
			.ForMember(w => w.creadeddate, f => f.MapFrom(w => w.CreadedDate))
			.ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));
	}
}
