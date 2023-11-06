using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;

[Serializable]
public class GetCurrencyListResponseDTO
{
	public int currencyid { get; set; }

	public string currencycode { get; set; }

	public string currencyname { get; set; }

	public string currencysymbol { get; set; }

	public string countryname { get; set; }

	public string createdby { get; set; }

	public DateTime createddate { get; set; }

	public bool isactive { get; set; }
}
