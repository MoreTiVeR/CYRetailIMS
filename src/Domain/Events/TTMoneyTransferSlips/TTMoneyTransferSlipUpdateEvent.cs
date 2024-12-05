using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTMoneyTransferSlips;

public class TTMoneyTransferSlipUpdateEvent : BaseEvent
{
    public TTMoneyTransferSlip Item { get; set; }
    public TTMoneyTransferSlipUpdateEvent(TTMoneyTransferSlip item)
    {
        Item = item;
    }
}
