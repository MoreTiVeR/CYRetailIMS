using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTItemTransfers;
public class TTItemTransferDeleteEvent : BaseEvent
{
    public TTItemTransfer Item { get; set; }
    public TTItemTransferDeleteEvent(TTItemTransfer item)
    {
        Item = item;
    }
}
