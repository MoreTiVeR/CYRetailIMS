using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTTransactionAudits;
public class TTTransactionAuditUpdateEvent : BaseEvent
{
	public TTTransactionAudit Item { get; set; }
	public TTTransactionAuditUpdateEvent(TTTransactionAudit transactionAudit)
	{
		Item = transactionAudit;
	}
}
