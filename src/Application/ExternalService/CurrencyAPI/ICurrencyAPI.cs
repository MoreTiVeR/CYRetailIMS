using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByCode.v1;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByID.v1;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;

namespace CYRetailIMS.Application.ExternalService.CurrencyAPI;
public interface ICurrencyAPI
{
	Task<BaseResponse<List<GetCurrencyListResponseDTO>>> GetCurrencyListAsync();
	Task<BaseResponse<GetCurrencyByIDResponseDTO>> GetCurrencyByIDAsync(int currencyID);
	Task<BaseResponse<GetCurrencyByCodeResponseDTO>> GetCurrencyByCodeAsync(string currencyCode);
}
