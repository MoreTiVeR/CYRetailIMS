using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTTransactions;
public class TTTransactionsUpdateEvent : BaseEvent
{
    public TTTransaction Transaction { get; set; }
    public TTTransactionsUpdateEvent(TTTransaction transaction)
    {
        Transaction = transaction;
    }
}
