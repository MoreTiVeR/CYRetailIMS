using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMUserInBranchs;
public class TMUserInBranchDeleteEvent : BaseEvent
{
    public TMUserInBranch Item { get; set; }
    public TMUserInBranchDeleteEvent(TMUserInBranch userInBranch)
    {
        Item = userInBranch;
    }
}
