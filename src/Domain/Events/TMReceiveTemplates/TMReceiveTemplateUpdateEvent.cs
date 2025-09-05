using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMReceiveTemplates;
public class TMReceiveTemplateUpdateEvent : BaseEvent
{
    public TMReceiveTemplate Item { get; set; }
    public TMReceiveTemplateUpdateEvent(TMReceiveTemplate item)
    {
        Item = item;
    }
}
