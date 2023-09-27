using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMUsers;
public class TMUsersDeleteEvent : BaseEvent
{
    public Entities.TMUsers Item { get; set; }
    public TMUsersDeleteEvent(Entities.TMUsers item)
    {
        Item = item;
    }
}
