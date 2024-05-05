using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByCriteria.v1;
public record GetTransactionByCriteriaQuery : IRequest<BaseResponse<GetTransactionByCriteriaResponseDTO>>
{
    public int branchid { get; init; }
    public int transactionid { get; init; }
}
