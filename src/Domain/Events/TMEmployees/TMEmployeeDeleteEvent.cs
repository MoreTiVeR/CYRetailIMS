using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMEmployees;

public class TMEmployeeDeleteEvent : BaseEvent
{
    public TMEmployee Item { get; set; }
    public TMEmployeeDeleteEvent(TMEmployee item)
    {
        Item = item;
    }
}
