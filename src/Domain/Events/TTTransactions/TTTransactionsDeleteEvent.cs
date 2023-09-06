using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTTransactions;
public class TTTransactionsDeleteEvent : BaseEvent
{
    public TTTransaction Transaction { get; set; }
    public TTTransactionsDeleteEvent(TTTransaction transaction)
    {
        Transaction = transaction;
    }
}
