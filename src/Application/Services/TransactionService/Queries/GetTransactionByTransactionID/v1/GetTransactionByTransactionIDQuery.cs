using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByTransactionID.v1;
public record GetTransactionByTransactionIDQuery : IRequest<BaseResponse<GetTransactionByBranchIDResponseDTO>>
{
    public int transactionid { get; init; }
}
