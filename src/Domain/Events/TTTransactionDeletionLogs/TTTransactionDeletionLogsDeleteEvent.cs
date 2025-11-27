using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTTransactionDeletionLogs;
public class TTTransactionDeletionLogsDeleteEvent : BaseEvent
{
    public TTTransactionDeletionLog Item { get; set; }
    public TTTransactionDeletionLogsDeleteEvent(TTTransactionDeletionLog transactionDeletionLog)
    {
        Item = transactionDeletionLog;
    }
}
