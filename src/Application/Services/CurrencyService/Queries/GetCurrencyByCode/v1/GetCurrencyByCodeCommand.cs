using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByCode.v1;

[Serializable]
public record GetCurrencyByCodeCommand : IRequest<BaseResponse<GetCurrencyByCodeResponseDTO>>
{
    public string currencycode { get; init; }
}
