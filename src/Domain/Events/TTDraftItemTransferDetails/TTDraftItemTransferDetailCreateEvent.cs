
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTDraftItemTransferDetails;
public class TTDraftItemTransferDetailCreateEvent : BaseEvent
{
    public TTDraftItemTransferDetail Item { get; set; }
    public TTDraftItemTransferDetailCreateEvent(TTDraftItemTransferDetail item)
    {
        Item = item;
    }
}
