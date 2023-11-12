using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTDraftItemTransfers;

public class TTDraftItemTransferUpdateEvent : BaseEvent
{
    public TTDraftItemTransfer Item { get; set; }
    public TTDraftItemTransferUpdateEvent(TTDraftItemTransfer item)
    {
        Item = item;
    }
}
