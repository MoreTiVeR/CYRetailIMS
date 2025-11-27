using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTTransactionDeletionLogs;
public class TTTransactionDeletionLogsCreateEvent : BaseEvent
{
    public TTTransactionDeletionLog Item { get; set; }
    public TTTransactionDeletionLogsCreateEvent(TTTransactionDeletionLog transactionDeletionLog)
    {
        Item = transactionDeletionLog;
    }
}
