using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTMoneyTransfers;
public class TTMoneyTransferDeleteEvent : BaseEvent
{
    public TTMoneyTransfer Item { get; set; }
    public TTMoneyTransferDeleteEvent(TTMoneyTransfer item)
    {
        Item = item;
    }
}
