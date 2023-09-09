using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;

[Serializable]
public class GetTransactionByBranchIDQuery : IRequest<BaseResponse<List<GetTransactionByBranchIDResponseDTO>>>
{
    public int branchid { get; init; }
}
