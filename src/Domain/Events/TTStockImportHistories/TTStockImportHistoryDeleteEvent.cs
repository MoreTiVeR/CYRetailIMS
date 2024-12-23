using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTStockImportHistories;

public class TTStockImportHistoryDeleteEvent : BaseEvent
{
    public TTStockImportHistory Item { get; set; }
    public TTStockImportHistoryDeleteEvent(TTStockImportHistory importHistory)
    {
        Item = importHistory;
    }
}