using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMUsers;
public class TMUsersDeleteEvent
{
    public TMUser Item { get; set; }
    public TMUsersDeleteEvent(TMUser item)
    {
        Item = item;
    }
}
