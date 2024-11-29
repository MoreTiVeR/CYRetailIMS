using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTMoneyTransferSlipsDetails;

public class TTMoneyTransferSlipsDetailDeleteEvent : BaseEvent
{
    public TTMoneyTransferSlipsDetail Item { get; set; }
    public TTMoneyTransferSlipsDetailDeleteEvent(TTMoneyTransferSlipsDetail item)
    {
        Item = item;
    }
}