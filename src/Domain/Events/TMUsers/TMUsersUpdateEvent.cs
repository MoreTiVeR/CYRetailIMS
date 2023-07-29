using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMUsers;
public class TMUsersUpdateEvent
{
    public TMUser Item { get; set; }
    public TMUsersUpdateEvent(TMUser item)
    {
        Item = item;
    }
}
