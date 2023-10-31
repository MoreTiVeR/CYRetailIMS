using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactions.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactionByBranchID.v1;
public record GetAdjustItemTransactionByBranchIDQuery : IRequest<BaseResponse<List<GetAdjustItemTransactionsResponseDTO>>>
{
    public int branchid { get; init; }
}
