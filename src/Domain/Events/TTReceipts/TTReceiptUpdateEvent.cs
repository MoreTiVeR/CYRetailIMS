using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMReceiveTemplates;
public class TTReceiptUpdateEvent : BaseEvent
{
    public TTReceipt Item { get; set; }
    public TTReceiptUpdateEvent(TTReceipt item)
    {
        Item = item;
    }
}
