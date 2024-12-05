using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMItemBrands;

public class TMItemBrandDeleteEvent : BaseEvent
{
    public TMItemBrand Item { get; set; }
    public TMItemBrandDeleteEvent(TMItemBrand tmItemBrand)
    {
        Item = tmItemBrand;
    }
}
