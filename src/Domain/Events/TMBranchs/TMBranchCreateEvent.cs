using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMBranchs;
public class TMBranchCreateEvent : BaseEvent
{
    public TMBranch Item { get; set; }
    public TMBranchCreateEvent(TMBranch tmBranch)
    {
        Item = tmBranch;
    }
}
