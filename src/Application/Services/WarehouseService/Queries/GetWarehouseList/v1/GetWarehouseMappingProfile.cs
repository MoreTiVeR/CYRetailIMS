using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseList.v1;
public class GetWarehouseMappingProfile : Profile
{
	public GetWarehouseMappingProfile()
	{
		CreateMap<TMWarehouse, GetWarehouseResponseDTO>();
	}
}
