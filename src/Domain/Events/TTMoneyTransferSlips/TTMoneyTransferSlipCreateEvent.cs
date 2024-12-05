using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTMoneyTransferSlips;

public class TTMoneyTransferSlipCreateEvent : BaseEvent
{
    public TTMoneyTransferSlip Item { get; set; }
    public TTMoneyTransferSlipCreateEvent(TTMoneyTransferSlip item)
    {
        Item = item;
    }
}
