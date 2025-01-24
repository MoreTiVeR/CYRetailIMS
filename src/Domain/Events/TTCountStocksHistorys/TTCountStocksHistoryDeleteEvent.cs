using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTCountStocksHistorys;
public class TTCountStocksHistoryDeleteEvent : BaseEvent
{
    public TTCountStocksHistory Item { get; set; }
    public TTCountStocksHistoryDeleteEvent(TTCountStocksHistory item)
    {
        Item = item;
    }
}
