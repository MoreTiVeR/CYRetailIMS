using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTCountStockDetails;
public class TTCountStockDetailDelete : BaseEvent
{
    public TTCountStockDetail Item { get; set; }
    public TTCountStockDetailDelete(TTCountStockDetail countStockDetail)
    {
        Item = countStockDetail;
    }
}
