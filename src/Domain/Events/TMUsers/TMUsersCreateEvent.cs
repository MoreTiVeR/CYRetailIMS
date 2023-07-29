using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMUsers;
public class TMUsersCreateEvent : BaseEvent
{
    public TMUser Item { get; set; }
    public TMUsersCreateEvent(TMUser item)
    {
        Item = item;
    }
}
