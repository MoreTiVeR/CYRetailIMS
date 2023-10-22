using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMSupplierContacts;
public class TMSupplierContactCreateEvent : BaseEvent
{
    public TMSupplierContact Item { get; set; }
    public TMSupplierContactCreateEvent(TMSupplierContact tmSupplierContact)
    {
        Item = tmSupplierContact;
    }
}
