using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByID.v1;

[Serializable]
public record GetCurrencyByIDCommand : IRequest<BaseResponse<GetCurrencyByIDResponseDTO>>
{
    public int currencyid { get; init; }
}
