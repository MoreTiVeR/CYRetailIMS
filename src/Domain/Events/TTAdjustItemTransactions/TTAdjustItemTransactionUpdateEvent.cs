using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTAdjustItemTransactions;
public class TTAdjustItemTransactionUpdateEvent : BaseEvent
{
    public TTAdjustItemTransaction Item { get; set; }
    public TTAdjustItemTransactionUpdateEvent(TTAdjustItemTransaction adjustItemTransaction)
    {
        Item = adjustItemTransaction;
    }
}
