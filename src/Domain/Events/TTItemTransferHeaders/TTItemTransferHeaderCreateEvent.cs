using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTItemTransferHeaders;

public class TTItemTransferHeaderCreateEvent : BaseEvent
{
    public TTItemTransferHeader Item { get; set; }
    public TTItemTransferHeaderCreateEvent(TTItemTransferHeader item)
    {
        Item = item;
    }
}
