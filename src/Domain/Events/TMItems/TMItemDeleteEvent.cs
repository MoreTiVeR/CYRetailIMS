using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMItems;
public class TMItemDeleteEvent : BaseEvent
{
    public TMItem Item { get; set; }
    public TMItemDeleteEvent(TMItem item)
    {
        Item = item;
    }
}
