using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTTransactionDetails;
public class TTTransactionDetailDeleteEvent : BaseEvent
{
    public TTTransactonDetail TTTransactonDetail { get; set; }
    public TTTransactionDetailDeleteEvent(TTTransactonDetail transactonDetail)
    {
        TTTransactonDetail = transactonDetail;
    }
}