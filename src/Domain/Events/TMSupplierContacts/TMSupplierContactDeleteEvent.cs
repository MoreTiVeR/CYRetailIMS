using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMSupplierContacts;

public class TMSupplierContactDeleteEvent : BaseEvent
{
    public TMSupplierContact Item { get; set; }
    public TMSupplierContactDeleteEvent(TMSupplierContact tmSupplierContact)
    {
        Item = tmSupplierContact;
    }
}