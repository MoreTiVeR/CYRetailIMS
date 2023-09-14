using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TMItemBrands;
public class TMItemBrandUpdateEvent : BaseEvent
{
    public TMItemBrand Item { get; set; }
    public TMItemBrandUpdateEvent(TMItemBrand tmItemBrand)
    {
        Item = tmItemBrand;
    }
}
