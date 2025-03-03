using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTItemTransferHistories;

public class TTItemTransferHistoriesUpdateEvent : BaseEvent
{
    public TTItemTransferHistory Item { get; set; }
    public TTItemTransferHistoriesUpdateEvent(TTItemTransferHistory item)
    {
        Item = item;
    }
}