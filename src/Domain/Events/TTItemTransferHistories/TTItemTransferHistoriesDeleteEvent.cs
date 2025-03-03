using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTItemTransferHistories;

public class TTItemTransferHistoriesDeleteEvent : BaseEvent
{
    public TTItemTransferHistory Item { get; set; }
    public TTItemTransferHistoriesDeleteEvent(TTItemTransferHistory item)
    {
        Item = item;
    }
}