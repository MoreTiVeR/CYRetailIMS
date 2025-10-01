using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMReceiveTemplates;
public class TMReceiptNumberUpdateEvent : BaseEvent
{
    public TMReceiptNumber Item { get; set; }
    public TMReceiptNumberUpdateEvent(TMReceiptNumber item)
    {
        Item = item;
    }
}
