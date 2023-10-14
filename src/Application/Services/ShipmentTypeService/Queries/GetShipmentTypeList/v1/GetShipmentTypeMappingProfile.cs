using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
public class GetShipmentTypeMappingProfile : Profile
{
	public GetShipmentTypeMappingProfile()
	{
		CreateMap<TMShipmentType, GetShipmentTypeResponseDTO>();
	}
}
