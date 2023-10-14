using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;
public class GetSupplierContactTypeMappingProfile : Profile
{
	public GetSupplierContactTypeMappingProfile()
	{
		CreateMap<TMSupplierContactType, GetSupplierContactTypeResposeDTO>();
	}
}
