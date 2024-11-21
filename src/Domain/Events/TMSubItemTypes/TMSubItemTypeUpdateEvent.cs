using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMSubItemTypes;

public class TMSubItemTypeUpdateEvent : BaseEvent
{
    public TMSubItemType Item { get; set; }
    public TMSubItemTypeUpdateEvent(TMSubItemType item)
    {
        Item = item;
    }
}
