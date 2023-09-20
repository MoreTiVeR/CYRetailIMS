using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMBranchsDetail;
public class TMBranchDetailUpdateEvent : BaseEvent
{
    public TMBranchDetail Item { get; set; }
    public TMBranchDetailUpdateEvent(TMBranchDetail tmBranchDetail)
    {
        Item = tmBranchDetail;
    }
}
