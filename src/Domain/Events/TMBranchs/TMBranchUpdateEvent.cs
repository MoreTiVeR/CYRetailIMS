using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMBranchs;

public class TMBranchUpdateEvent : BaseEvent
{
    public TMBranch Item { get; set; }
    public TMBranchUpdateEvent(TMBranch tmBranch)
    {
        Item = tmBranch;
    }
}