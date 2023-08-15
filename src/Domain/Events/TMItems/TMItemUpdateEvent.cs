using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMItems;
public class TMItemUpdateEvent : BaseEvent
{
    public TMItem Item { get; set; }
    public TMItemUpdateEvent(TMItem item)
    {
        Item = item;
    }
}
