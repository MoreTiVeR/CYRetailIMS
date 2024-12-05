using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTMoneyTransferSlips;

public class TTMoneyTransferSlipDeleteEvent : BaseEvent
{
    public TTMoneyTransferSlip Item { get; set; }
    public TTMoneyTransferSlipDeleteEvent(TTMoneyTransferSlip item)
    {
        Item = item;
    }
}
