
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTDraftItemTransferDetails;
public class TTDraftItemTransferDetailUpdateEvent : BaseEvent
{
    public TTDraftItemTransferDetail Item { get; set; }
    public TTDraftItemTransferDetailUpdateEvent(TTDraftItemTransferDetail item)
    {
        Item = item;
    }
}
