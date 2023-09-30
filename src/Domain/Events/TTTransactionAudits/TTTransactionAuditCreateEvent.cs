
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Common;

namespace CYRetailIMS.Domain.Events.TTTransactionAudits;
public class TTTransactionAuditCreateEvent : BaseEvent
{
	public TTTransactionAudit Item { get; set; }
	public TTTransactionAuditCreateEvent(TTTransactionAudit transactionAudit)
	{
		Item = transactionAudit;
	}
}
