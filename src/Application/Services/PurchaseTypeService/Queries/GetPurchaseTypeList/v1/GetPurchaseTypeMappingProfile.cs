using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
public class GetPurchaseTypeMappingProfile : Profile
{
	public GetPurchaseTypeMappingProfile()
	{
		CreateMap<TMPurchaseType, GetPurchaseTypeResponseDTO>();
	}
}
