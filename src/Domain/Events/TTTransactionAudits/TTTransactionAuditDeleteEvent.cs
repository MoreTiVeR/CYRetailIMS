using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTTransactionAudits;

public class TTTransactionAuditDeleteEvent : BaseEvent
{
	public TTTransactionAudit Item { get; set; }
	public TTTransactionAuditDeleteEvent(TTTransactionAudit transactionAudit)
	{
		Item = transactionAudit;
	}
}
