using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMSubItemTypeInItemTypes;
public class TMSubItemTypeInItemTypeCreateEvent : BaseEvent
{
    public TMSubItemTypeInItemType Item { get; set; }
    public TMSubItemTypeInItemTypeCreateEvent(TMSubItemTypeInItemType item)
    {
        Item = item;
    }
}
