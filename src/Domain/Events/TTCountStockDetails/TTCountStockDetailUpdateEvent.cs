using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTCountStockDetails;
public class TTCountStockDetailUpdateEvent : BaseEvent
{
    public TTCountStockDetail Item { get; set; }
    public TTCountStockDetailUpdateEvent(TTCountStockDetail countStockDetail)
    {
        Item = countStockDetail;
    }
}
