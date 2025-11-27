using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.TransactionDeletionLogService.Commands.UpdateTransactionDeletionLog;
public record UpdateTransactionDeletionLogCommand
{
    public int transactionid { get; init; }
    public string reason { get; init; }
}
