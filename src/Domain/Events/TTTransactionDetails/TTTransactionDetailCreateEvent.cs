using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTTransactionDetails;

public class TTTransactionDetailCreateEvent : BaseEvent
{
    public TTTransactonDetail TTTransactonDetail { get; set; }
    public TTTransactionDetailCreateEvent(TTTransactonDetail transactonDetail)
    {
        TTTransactonDetail = transactonDetail;
    }

    public ICollection<TTTransactonDetail> TTTransactonsDetailList { get; set; }
    public TTTransactionDetailCreateEvent(ICollection<TTTransactonDetail> transactonDetailList)
    {
        TTTransactonsDetailList = transactonDetailList;
    }
}
