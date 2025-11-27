using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.TransactionDeletionLogService.Queries.GetTransactionDeletionLogByCriteria.v1;
public record GetTransactionDeletionLogByCriteriaQuery
{
    public int transactionid { get; init; }
}
