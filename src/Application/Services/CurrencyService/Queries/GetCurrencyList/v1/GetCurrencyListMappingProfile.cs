using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByID.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;
public class GetCurrencyListMappingProfile : Profile
{
	public GetCurrencyListMappingProfile()
	{
		CreateMap<TMCurrency, GetCurrencyListResponseDTO>();

		CreateMap<TMCurrency, GetCurrencyByIDResponseDTO>();
	}
}
