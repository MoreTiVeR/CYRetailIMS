using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTCountStocks;

public class TTCountStockDeleteEvent : BaseEvent
{
    public TTCountStock Item { get; set; }
    public TTCountStockDeleteEvent(TTCountStock countStock)
    {
        Item = countStock;
    }
}