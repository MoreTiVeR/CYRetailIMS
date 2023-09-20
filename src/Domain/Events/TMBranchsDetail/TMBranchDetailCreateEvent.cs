using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMBranchsDetail;
public class TMBranchDetailCreateEvent : BaseEvent
{
    public TMBranchDetail Item { get; set; }
    public TMBranchDetailCreateEvent(TMBranchDetail tmBranchDetail)
    {
        Item = tmBranchDetail;
    }
}
