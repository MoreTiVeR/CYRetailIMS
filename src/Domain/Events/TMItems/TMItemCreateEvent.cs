using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Common;

namespace CYRetailIMS.Domain.Events.TMItems;
public class TMItemCreateEvent : BaseEvent
{
    public TMItem Item { get; set; }
    public TMItemCreateEvent(TMItem item)
    {
        Item = item;
    }
}
