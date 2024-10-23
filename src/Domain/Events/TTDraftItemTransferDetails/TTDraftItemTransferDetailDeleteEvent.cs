
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTDraftItemTransferDetails;
public class TTDraftItemTransferDetailDeleteEvent : BaseEvent
{
    public TTDraftItemTransferDetail Item { get; set; }
    public TTDraftItemTransferDetailDeleteEvent(TTDraftItemTransferDetail item)
    {
        Item = item;
    }
}
