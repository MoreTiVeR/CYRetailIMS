using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTStockImportHistories;

public class TTStockImportHistoryUpdateEvent : BaseEvent
{
    public TTStockImportHistory Item { get; set; }
    public TTStockImportHistoryUpdateEvent(TTStockImportHistory importHistory)
    {
        Item = importHistory;
    }
}