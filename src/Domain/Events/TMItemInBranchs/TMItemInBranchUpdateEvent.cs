using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMItemInBranchs;
public class TMItemInBranchUpdateEvent : BaseEvent
{
    public TMItemInBranch ItemInBranch { get; set; }
    public TMItemInBranchUpdateEvent(TMItemInBranch itemInBranch)
    {
        ItemInBranch = itemInBranch;
    }

}