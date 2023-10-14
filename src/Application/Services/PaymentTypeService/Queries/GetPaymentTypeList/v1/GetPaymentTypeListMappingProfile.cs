using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.PaymentTypeByID.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;
public class GetPaymentTypeListMappingProfile : Profile
{

	public GetPaymentTypeListMappingProfile()
	{
		CreateMap<TMPaymentType, GetPaymentTypeListResponseDTO>();

		CreateMap<TMPaymentType, PaymentTypeByIDResponseDTO>();

	}
}
