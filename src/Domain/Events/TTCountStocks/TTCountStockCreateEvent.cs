using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTCountStocks;
public class TTCountStockCreateEvent : BaseEvent
{
    public TTCountStock Item { get; set; }
    public TTCountStockCreateEvent(TTCountStock countStock)
    {
        Item = countStock;
    }
}
