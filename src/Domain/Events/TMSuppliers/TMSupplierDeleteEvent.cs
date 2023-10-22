using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMSuppliers;

public class TMSupplierDeleteEvent : BaseEvent
{
    public TMSupplier Item { get; set; }
    public TMSupplierDeleteEvent(TMSupplier tmSupplier)
    {
        Item = tmSupplier;
    }
}
