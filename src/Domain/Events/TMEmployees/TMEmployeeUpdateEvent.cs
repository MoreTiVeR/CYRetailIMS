using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMEmployees;
public class TMEmployeeUpdateEvent
{
    public TMEmployee Item { get; set; }
    public TMEmployeeUpdateEvent(TMEmployee item)
    {
        Item = item;
    }
}
