using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.TransactionDeletionLogService.Commands.DeleteTransactionDeletionLog;
public record DeleteTransactionDeletionLogCommand
{
    public int transactionid { get; init; }
    public int branchid { get; init; }
    public string reason { get; init; } = string.Empty;
}
