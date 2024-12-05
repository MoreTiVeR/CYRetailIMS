using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTMoneyTransferSlipsDetails;

public class TTMoneyTransferSlipsDetailCreateEvent : BaseEvent
{
    public TTMoneyTransferSlipsDetail Item { get; set; }
    public TTMoneyTransferSlipsDetailCreateEvent(TTMoneyTransferSlipsDetail item)
    {
        Item = item;
    }
}