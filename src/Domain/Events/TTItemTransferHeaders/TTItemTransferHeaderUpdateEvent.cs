using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTItemTransferHeaders;

public class TTItemTransferHeaderUpdateEvent : BaseEvent
{
    public TTItemTransferHeader Item { get; set; }
    public TTItemTransferHeaderUpdateEvent(TTItemTransferHeader item)
    {
        Item = item;
    }
}