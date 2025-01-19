using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTCountStocksHistorys;
public class TTCountStocksHistoryCreateEvent : BaseEvent
{
    public TTCountStocksHistory Item { get; set; }
    public TTCountStocksHistoryCreateEvent(TTCountStocksHistory item)
    {
        Item = item;
    }
}
