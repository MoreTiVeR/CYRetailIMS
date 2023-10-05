using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactionByID.v1;

[Serializable]
public class GetAdjustItemTransactionByIDQuery : IRequest<BaseResponse<GetAdjustItemTransactionByIDResponseDTO>>
{
    public int adjusttransactionid { get; init; }
}
