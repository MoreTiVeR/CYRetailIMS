using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMReceiveTemplates;
public class TMReceiveTemplateCreateEvent : BaseEvent
{
    public TMReceiveTemplate Item { get; set; }
    public TMReceiveTemplateCreateEvent(TMReceiveTemplate item)
    {
        Item = item;
    }
}
